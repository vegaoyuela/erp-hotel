import { readFile, writeFile } from 'node:fs/promises'
import { fileURLToPath } from 'node:url'
import openapiTS, { astToString } from 'openapi-typescript'

const schemaFile = new URL('../../docs/plan de implementación/2026-09-27/contratos/openapi-v1.json', import.meta.url)
const typesFile = new URL('../../frontend/src/types/api.generated.ts', import.meta.url)
const check = process.argv.includes('--check')
const liveIndex = process.argv.indexOf('--live-schema')
const stable = value => Array.isArray(value) ? value.map(stable) : value && typeof value === 'object'
  ? Object.fromEntries(Object.entries(value).sort(([a], [b]) => a < b ? -1 : a > b ? 1 : 0).map(([key, child]) => [key, stable(child)])) : value
const canonical = value => JSON.stringify(stable(value), null, 2) + '\n'
const source = JSON.parse(await readFile(liveIndex >= 0 ? process.argv[liveIndex + 1] : schemaFile, 'utf8'))
const schema = canonical(source)
const types = '// Generated from the Hotel ERP OpenAPI contract. Run npm run api:generate.\n' + astToString(await openapiTS(JSON.parse(schema)))
if (check) {
  if (schema !== await readFile(schemaFile, 'utf8')) throw new Error('El OpenAPI real difiere del snapshot: regenere y revise el contrato.')
  if (types !== await readFile(typesFile, 'utf8')) throw new Error('Los tipos TypeScript no corresponden al OpenAPI. Ejecute npm run api:generate.')
  console.log('[QA] OpenAPI y tipos TypeScript sincronizados')
} else {
  await writeFile(schemaFile, schema)
  await writeFile(typesFile, types)
  console.log(`Contrato y tipos generados: ${fileURLToPath(typesFile)}`)
}
