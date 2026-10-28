import { readdirSync, readFileSync } from 'node:fs';
import { fileURLToPath } from 'node:url';
import { join } from 'node:path';

const directory = fileURLToPath(new URL('../backend/src/hotel-erp.Api/Database/Repositories/', import.meta.url));
const forbidden = /\b(?:SaveChanges(?:Async)?|Execute(?:Update|Delete|Sql\w*)(?:Async)?|BeginTransaction(?:Async)?|Commit(?:Async)?)\s*\(/g;
const violations = [];
const files = readdirSync(directory).filter(name => name.endsWith('.cs'));

for (const name of files) {
  const source = readFileSync(join(directory, name), 'utf8');
  // The rule applies to executable code; remove comments and literals first.
  const code = source.replace(/\/\*[\s\S]*?\*\/|\/\/[^\n]*|@"(?:""|[^"])*"|"(?:\\.|[^"\\])*"/g,
    match => match.replace(/[^\n]/g, ' '));
  for (const match of code.matchAll(forbidden)) {
    const line = code.slice(0, match.index).split('\n').length;
    violations.push(`${name}:${line}: ${match[0]}`);
  }
}

if (violations.length > 0) {
  console.error('Los repositorios deben preparar cambios; la unidad de trabajo confirma su persistencia.');
  console.error(violations.join('\n'));
  process.exitCode = 1;
} else {
  console.log(`[QA] ${files.length} archivos de repositorio: sin persistencia ni transacciones implícitas`);
}
