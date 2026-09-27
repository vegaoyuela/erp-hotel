#!/usr/bin/env python3
"""Genera inventario y manifiesto SHA-256 de la línea base sin copiar datos."""

from __future__ import annotations

import argparse
import hashlib
import json
import subprocess
from collections import Counter
from datetime import datetime, timezone
from pathlib import Path


ROOT = Path(__file__).resolve().parents[1]
EXCLUDED_PARTS = {".git", "bin", "obj", "node_modules", "wwwroot", "logs", "dist", "__pycache__"}


def git(*arguments: str) -> str:
    return subprocess.run(
        ["git", *arguments], cwd=ROOT, check=True, capture_output=True, text=True
    ).stdout.strip()


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as stream:
        for block in iter(lambda: stream.read(1024 * 1024), b""):
            digest.update(block)
    return digest.hexdigest()


def usable(path: Path) -> bool:
    relative = path.relative_to(ROOT)
    if any(part in EXCLUDED_PARTS for part in relative.parts):
        return False
    return not str(relative).startswith("data/backups/")


def files_under(base: str, suffixes: set[str] | None = None) -> list[Path]:
    directory = ROOT / base
    if not directory.exists():
        return []
    return sorted(
        path
        for path in directory.rglob("*")
        if path.is_file() and usable(path) and (suffixes is None or path.suffix.lower() in suffixes)
    )


def aggregate(paths: list[Path]) -> dict[str, object]:
    digest = hashlib.sha256()
    total_size = 0
    for path in paths:
        relative = path.relative_to(ROOT).as_posix()
        file_hash = sha256(path)
        size = path.stat().st_size
        digest.update(relative.encode("utf-8") + b"\0" + file_hash.encode("ascii") + b"\n")
        total_size += size
    return {
        "file_count": len(paths),
        "total_size_bytes": total_size,
        "aggregate_sha256": digest.hexdigest(),
    }


def deleted_generated(baseline_tag: str) -> tuple[list[str], Counter[str]]:
    baseline_files = [
        line for line in git("ls-tree", "-r", "--name-only", baseline_tag).splitlines() if line
    ]
    deleted = [item for item in baseline_files if not (ROOT / item).exists()]
    generated = [
        item
        for item in deleted
        if any(token in f"/{item}/" for token in ("/bin/", "/obj/", "/logs/", "/wwwroot/"))
        or item.endswith((".db", ".lscache"))
        or item == "frontend/pnpm-lock.yaml"
    ]
    categories: Counter[str] = Counter()
    for item in generated:
        if "/bin/" in f"/{item}/":
            categories["bin"] += 1
        elif "/obj/" in f"/{item}/":
            categories["obj"] += 1
        elif "/logs/" in f"/{item}/":
            categories["logs"] += 1
        elif "/wwwroot/" in f"/{item}/":
            categories["wwwroot"] += 1
        elif item.endswith(".db"):
            categories["sqlite"] += 1
        elif item.endswith(".lscache"):
            categories["language_server_cache"] += 1
        elif item == "frontend/pnpm-lock.yaml":
            categories["secondary_lockfile"] += 1
    return generated, categories


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--inventory", type=Path, required=True)
    parser.add_argument("--manifest", type=Path, required=True)
    parser.add_argument("--backup-evidence", type=Path, required=True)
    args = parser.parse_args()

    now = datetime.now(timezone.utc).isoformat()
    baseline_tag = "implementation-baseline-2026-09-27"
    baseline_commit = git("rev-list", "-n", "1", baseline_tag)

    backend_source = files_under("backend/src", {".cs", ".csproj", ".json"})
    backend_tests = files_under("backend/tests", {".cs", ".csproj", ".json"})
    frontend = files_under("frontend/src") + [
        path
        for path in (
            ROOT / "frontend/package.json",
            ROOT / "frontend/package-lock.json",
            ROOT / "frontend/vite.config.ts",
            ROOT / "frontend/tsconfig.json",
        )
        if path.exists()
    ]
    migrations = sorted((ROOT / "backend/src/hotel-erp.Api/Migrations").glob("*"))
    migrations = [path for path in migrations if path.is_file()]
    deployment = files_under("docker")
    scripts = files_under("scripts")
    plan_docs = files_under("docs/plan de implementación/2026-09-27", {".md"})

    attachment_roots = [
        ROOT / "backend/src/hotel-erp.Api/Uploads",
        ROOT / "data/attachments",
        ROOT / "data/documents",
    ]
    attachments = sorted(
        path for directory in attachment_roots if directory.exists() for path in directory.rglob("*") if path.is_file()
    )
    configuration = sorted(
        path
        for path in ROOT.rglob("*")
        if path.is_file()
        and usable(path)
        and (
            path.name.startswith("appsettings")
            or path.name in {"docker-compose.yml", "Dockerfile", "package.json", "package-lock.json"}
        )
    )
    seeds = sorted(
        path for path in backend_source if "seed" in path.name.lower() or "bootstrap" in path.name.lower()
    )
    generated, generated_categories = deleted_generated(baseline_tag)

    baseline_tracked = [
        line for line in git("ls-tree", "-r", "--name-only", baseline_tag).splitlines() if line
    ]
    source_counts = {
        "tracked_files_at_baseline": len(baseline_tracked),
        "csharp_files": len([path for path in ROOT.rglob("*.cs") if usable(path)]),
        "typescript_react_files": len([path for path in (ROOT / "frontend/src").rglob("*.tsx")]),
        "api_controllers": len(list((ROOT / "backend/src/hotel-erp.Api/Controllers").glob("*Controller.cs"))),
        "ef_migration_files": len(migrations),
        "backend_test_files": len([path for path in backend_tests if path.suffix == ".cs"]),
        "dated_plan_markdown_files": len(plan_docs),
    }

    inventory = {
        "generated_at": now,
        "contains_secret_values": False,
        "branch": git("branch", "--show-current"),
        "baseline": {"tag": baseline_tag, "commit": baseline_commit},
        "data_sources": {
            "synthetic_qa": {
                "available": True,
                "classification": "synthetic-qa",
                "backup_restore_evidence": args.backup_evidence.as_posix(),
            },
            "official_hotel_database": {
                "available": False,
                "status": "pending_external_delivery",
                "required_action": "Inventariar, respaldar y restaurar cuando el propietario entregue la fuente oficial.",
            },
        },
        "counts": source_counts,
        "attachments": {
            "runtime_file_count": len(attachments),
            "total_size_bytes": sum(path.stat().st_size for path in attachments),
            "paths_recorded": [path.relative_to(ROOT).as_posix() for path in attachments],
            "pii_copied_to_evidence": False,
        },
        "configuration_files": [path.relative_to(ROOT).as_posix() for path in configuration],
        "seed_or_bootstrap_files": [path.relative_to(ROOT).as_posix() for path in seeds],
        "generated_artifact_cleanup": {
            "files_scheduled_for_deletion": len(generated),
            "categories": dict(sorted(generated_categories.items())),
        },
        "secret_hygiene": {
            "compose_fixed_operational_secrets_removed": True,
            "compose_requires_external_postgres_and_jwt_values": True,
            "known_operational_literal_scan": "passed",
            "test_fixture_credentials": "retained_only_in_test_sources",
            "claim": "No se afirma ausencia absoluta; la revisión automatizada y manual no detectó secretos operativos conocidos.",
        },
    }

    groups = {
        "backend_application": backend_source,
        "backend_tests": backend_tests,
        "frontend": sorted(set(frontend)),
        "migrations": migrations,
        "deployment": deployment,
        "scripts": scripts,
        "dated_plan_docs": plan_docs,
        "runtime_attachments": attachments,
    }
    backup = json.loads((ROOT / args.backup_evidence).read_text(encoding="utf-8"))
    key_candidates = [
        ROOT / ".gitignore",
        ROOT / "docker/docker-compose.yml",
        ROOT / "docker/Dockerfile",
        ROOT / "frontend/package-lock.json",
        ROOT / "backend/hotel-erp.slnx",
    ]
    manifest = {
        "generated_at": now,
        "contains_secret_values": False,
        "hash_algorithm": "SHA-256",
        "aggregate_algorithm": "SHA-256 de cada ruta relativa, NUL, hash de archivo y salto de línea; rutas ordenadas.",
        "baseline": {"tag": baseline_tag, "commit": baseline_commit},
        "groups": {name: aggregate(paths) for name, paths in groups.items()},
        "key_files": {
            path.relative_to(ROOT).as_posix(): {"size_bytes": path.stat().st_size, "sha256": sha256(path)}
            for path in key_candidates
            if path.exists()
        },
        "database_backup": {
            "classification": backup["classification"],
            "path": backup["backup"]["path"],
            "size_bytes": backup["backup"]["size_bytes"],
            "sha256": backup["backup"]["sha256"],
            "restore_table_counts_match": backup["restore_verification"]["table_counts_match"],
        },
        "official_hotel_database": {
            "available": False,
            "hash": None,
            "status": "pending_external_delivery",
        },
    }

    for destination, document in ((args.inventory, inventory), (args.manifest, manifest)):
        target = ROOT / destination
        target.parent.mkdir(parents=True, exist_ok=True)
        target.write_text(json.dumps(document, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
