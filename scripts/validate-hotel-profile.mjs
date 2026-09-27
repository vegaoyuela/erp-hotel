#!/usr/bin/env node
import { readFile, writeFile, mkdir } from 'node:fs/promises'
import { resolve, dirname, relative } from 'node:path'
import { validateDossier, sha256 } from './lib/profile-dossier.mjs'

const args = process.argv.slice(2)
const inputArgument = args.find(argument => !argument.startsWith('--'))
const outputIndex = args.indexOf('--output')
const outputArgument = outputIndex >= 0 ? args[outputIndex + 1] : undefined
const requireOfficial = args.includes('--require-official')

if (!inputArgument || (outputIndex >= 0 && !outputArgument)) {
  console.error('Uso: node scripts/validate-hotel-profile.mjs <expediente.json> [--require-official] [--output resultado.json]')
  process.exit(2)
}

const inputPath = resolve(inputArgument)
let raw
let dossier
try {
  raw = await readFile(inputPath)
  dossier = JSON.parse(raw.toString('utf8'))
} catch (error) {
  const report = {
    schemaVersion: 1,
    generatedAt: new Date().toISOString(),
    status: 'failed',
    containsSourceValues: false,
    inputSha256: raw ? sha256(raw) : null,
    checks: { N01: 'failed', N02: 'not_run', N03: 'not_run', N04: 'not_run', N05: 'not_run', N06: 'not_run' },
    issues: [{ check: 'N01', code: 'INPUT_READ', path: '$', message: error instanceof Error ? error.message : 'No se pudo leer el expediente.' }],
  }
  await emit(report, outputArgument)
  process.exit(1)
}

const validation = validateDossier(dossier, { requireOfficial })
const report = {
  schemaVersion: 1,
  generatedAt: new Date().toISOString(),
  status: validation.valid ? 'passed' : 'failed',
  containsSourceValues: false,
  classification: dossier.classification ?? 'unknown',
  inputSha256: sha256(raw),
  checks: validation.checks,
  issues: validation.issues,
}
await emit(report, outputArgument)
process.exit(validation.valid ? 0 : 1)

async function emit(report, output) {
  const serialized = `${JSON.stringify(report, null, 2)}\n`
  if (output) {
    const destination = resolve(output)
    await mkdir(dirname(destination), { recursive: true })
    await writeFile(destination, serialized, 'utf8')
    console.log(`Resultado guardado en ${relative(process.cwd(), destination)}`)
  } else {
    process.stdout.write(serialized)
  }
}
