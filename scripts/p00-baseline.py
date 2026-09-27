#!/usr/bin/env python3
"""Crea y restaura un respaldo PostgreSQL de línea base sin exponer secretos."""

from __future__ import annotations

import argparse
import hashlib
import json
import os
import subprocess
import sys
from datetime import datetime, timezone
from pathlib import Path


def parse_connection(value: str) -> dict[str, str]:
    pairs: dict[str, str] = {}
    for part in value.split(";"):
        if not part.strip():
            continue
        if "=" not in part:
            raise ValueError("La cadena contiene un componente sin '='.")
        key, raw = part.split("=", 1)
        pairs[key.strip().lower().replace(" ", "")] = raw.strip()
    aliases = {
        "host": "host",
        "port": "port",
        "database": "database",
        "username": "username",
        "userid": "username",
        "user": "username",
        "password": "password",
    }
    result = {aliases[key]: item for key, item in pairs.items() if key in aliases}
    missing = {"host", "database", "username", "password"} - result.keys()
    if missing:
        raise ValueError("Faltan campos de conexión: " + ", ".join(sorted(missing)))
    result.setdefault("port", "5432")
    return result


def pg_env(connection: dict[str, str]) -> dict[str, str]:
    environment = os.environ.copy()
    environment["PGPASSWORD"] = connection["password"]
    return environment


def pg_args(tool: str, connection: dict[str, str], database: str | None = None) -> list[str]:
    return [
        tool,
        "-h", connection["host"],
        "-p", connection["port"],
        "-U", connection["username"],
        "-d", database or connection["database"],
    ]


def pg_server_args(tool: str, connection: dict[str, str]) -> list[str]:
    return [
        tool,
        "-h", connection["host"],
        "-p", connection["port"],
        "-U", connection["username"],
    ]


def run(command: list[str], connection: dict[str, str], *, capture: bool = True) -> subprocess.CompletedProcess[str]:
    return subprocess.run(
        command,
        check=True,
        text=True,
        capture_output=capture,
        env=pg_env(connection),
    )


def scalar(connection: dict[str, str], database: str, query: str) -> str:
    result = run(pg_args("psql", connection, database) + ["-X", "-A", "-t", "-c", query], connection)
    return result.stdout.strip()


def quoted_identifier(value: str) -> str:
    return '"' + value.replace('"', '""') + '"'


def table_counts(connection: dict[str, str], database: str) -> dict[str, int]:
    result = run(
        pg_args("psql", connection, database)
        + [
            "-X", "-A", "-t", "-c",
            "SELECT tablename FROM pg_catalog.pg_tables WHERE schemaname='public' ORDER BY tablename;",
        ],
        connection,
    )
    tables = [line for line in result.stdout.splitlines() if line]
    counts: dict[str, int] = {}
    for table in tables:
        query = f"SELECT count(*) FROM public.{quoted_identifier(table)};"
        counts[table] = int(scalar(connection, database, query))
    return counts


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as stream:
        for block in iter(lambda: stream.read(1024 * 1024), b""):
            digest.update(block)
    return digest.hexdigest()


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--output", type=Path, required=True, help="JSON de evidencia sin secretos.")
    parser.add_argument("--backup-dir", type=Path, required=True, help="Directorio ignorado por Git para el dump.")
    parser.add_argument(
        "--classification",
        default="synthetic-qa",
        choices=["synthetic-qa", "official-copy"],
        help="Clasificación explícita de la fuente.",
    )
    parser.add_argument(
        "--docker-container",
        help="Contenedor PostgreSQL de la misma versión para ejecutar pg_dump/pg_restore.",
    )
    args = parser.parse_args()

    raw_connection = os.environ.get("HOTEL_ERP_BASELINE_CONNECTION", "")
    if not raw_connection:
        print("Configure HOTEL_ERP_BASELINE_CONNECTION.", file=sys.stderr)
        return 2
    connection = parse_connection(raw_connection)
    source_database = connection["database"]
    stamp = datetime.now(timezone.utc).strftime("%Y%m%dT%H%M%SZ")
    restore_database = f"hotel_erp_p00_restore_{os.getpid()}"
    args.backup_dir.mkdir(parents=True, exist_ok=True)
    args.output.parent.mkdir(parents=True, exist_ok=True)
    backup_path = args.backup_dir / f"{source_database}_{stamp}.dump"

    created_restore = False
    evidence: dict[str, object] = {
        "generated_at": datetime.now(timezone.utc).isoformat(),
        "classification": args.classification,
        "source_database": source_database,
        "source_host": connection["host"],
        "source_port": int(connection["port"]),
        "contains_secret_values": False,
        "dump_restore_runtime": args.docker_container or "host-tools",
    }
    try:
        evidence["postgres_version"] = scalar(connection, source_database, "SHOW server_version;")
        evidence["source_size_bytes"] = int(
            scalar(connection, source_database, "SELECT pg_database_size(current_database());")
        )
        source_counts = table_counts(connection, source_database)
        evidence["source_table_counts"] = source_counts

        if args.docker_container:
            dump_result = subprocess.run(
                [
                    "docker", "exec", args.docker_container,
                    "pg_dump", "-U", connection["username"], "-d", source_database,
                    "--format=custom", "--no-owner", "--no-privileges",
                ],
                check=True,
                capture_output=True,
            )
            backup_path.write_bytes(dump_result.stdout)
        else:
            run(
                pg_args("pg_dump", connection, source_database)
                + ["--format=custom", "--no-owner", "--no-privileges", "--file", str(backup_path)],
                connection,
            )
        evidence["backup"] = {
            "path": str(backup_path.resolve()),
            "size_bytes": backup_path.stat().st_size,
            "sha256": sha256(backup_path),
        }

        if args.docker_container:
            subprocess.run(
                ["docker", "exec", args.docker_container, "createdb", "-U", connection["username"], restore_database],
                check=True,
                capture_output=True,
            )
        else:
            run(pg_server_args("createdb", connection) + [restore_database], connection)
        created_restore = True
        if args.docker_container:
            with backup_path.open("rb") as stream:
                subprocess.run(
                    [
                        "docker", "exec", "-i", args.docker_container,
                        "pg_restore", "-U", connection["username"], "-d", restore_database,
                        "--no-owner", "--no-privileges",
                    ],
                    check=True,
                    stdin=stream,
                    capture_output=True,
                )
        else:
            run(
                pg_args("pg_restore", connection, restore_database)
                + ["--no-owner", "--no-privileges", str(backup_path)],
                connection,
            )
        restored_counts = table_counts(connection, restore_database)
        mismatches = {
            table: {"source": count, "restored": restored_counts.get(table)}
            for table, count in source_counts.items()
            if restored_counts.get(table) != count
        }
        missing_after_restore = sorted(set(restored_counts) - set(source_counts))
        evidence["restore_verification"] = {
            "temporary_database": restore_database,
            "table_counts_match": not mismatches and not missing_after_restore,
            "mismatches": mismatches,
            "unexpected_tables": missing_after_restore,
            "restored_size_bytes": int(
                scalar(connection, restore_database, "SELECT pg_database_size(current_database());")
            ),
        }
        if mismatches or missing_after_restore:
            raise RuntimeError("La restauración no conserva los conteos de tablas.")
        evidence["status"] = "passed"
        return_code = 0
    except Exception as exc:  # noqa: BLE001 - se registra un mensaje controlado en evidencia.
        evidence["status"] = "failed"
        evidence["error"] = f"{type(exc).__name__}: {exc}"
        return_code = 1
    finally:
        if created_restore:
            if args.docker_container:
                subprocess.run(
                    [
                        "docker", "exec", args.docker_container,
                        "dropdb", "-U", connection["username"], "--force", restore_database,
                    ],
                    check=False,
                    capture_output=True,
                )
            else:
                subprocess.run(
                    pg_server_args("dropdb", connection) + ["--force", restore_database],
                    check=False,
                    text=True,
                    capture_output=True,
                    env=pg_env(connection),
                )
        evidence["temporary_database_removed"] = not bool(
            scalar(
                connection,
                "postgres",
                "SELECT 1 FROM pg_database WHERE datname = "
                + "'" + restore_database.replace("'", "''") + "';",
            )
        )
        args.output.write_text(json.dumps(evidence, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
        print(args.output)
    return return_code


if __name__ == "__main__":
    raise SystemExit(main())
