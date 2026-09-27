import test from 'node:test'
import assert from 'node:assert/strict'
import { readFile } from 'node:fs/promises'
import { validateDossier } from '../lib/profile-dossier.mjs'

const fixtureUrl = new URL('./fixtures/profile-valid-synthetic.json', import.meta.url)
const validDossier = JSON.parse(await readFile(fixtureUrl, 'utf8'))

test('acepta un expediente sintético completo para QA', () => {
  const result = validateDossier(validDossier)
  assert.equal(result.valid, true)
  assert.deepEqual(result.checks, { N01: 'passed', N02: 'passed', N03: 'passed', N04: 'passed', N05: 'passed', N06: 'passed' })
})

test('rechaza el expediente sintético cuando se exige fuente oficial', () => {
  const result = validateDossier(validDossier, { requireOfficial: true })
  assert.equal(result.valid, false)
  assert.ok(result.issues.some(issue => issue.code === 'OFFICIAL_REQUIRED'))
})

test('rechaza decisiones ausentes o duplicadas', () => {
  const dossier = structuredClone(validDossier)
  dossier.decisions = [dossier.decisions[0], dossier.decisions[0]]
  const result = validateDossier(dossier)
  assert.equal(result.valid, false)
  assert.ok(result.issues.some(issue => issue.code === 'DECISION_DUPLICATE'))
  assert.ok(result.issues.some(issue => issue.code === 'DECISION_MISSING'))
})

test('el resultado no copia valores del expediente', () => {
  const result = validateDossier(validDossier)
  const serialized = JSON.stringify(result)
  assert.equal(serialized.includes(validDossier.hotel.legalName), false)
  assert.equal(serialized.includes(validDossier.hotel.rtn), false)
})

test('rechaza fechas de calendario inexistentes', () => {
  const dossier = structuredClone(validDossier)
  dossier.billing.authorizations[0].validUntil = '2026-02-31'
  const result = validateDossier(dossier)
  assert.equal(result.valid, false)
  assert.ok(result.issues.some(issue => issue.code === 'AUTH_VALID_UNTIL'))
})
