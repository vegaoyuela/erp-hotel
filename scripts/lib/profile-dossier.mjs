import { createHash } from 'node:crypto'

export const DECISION_CODES = Array.from({ length: 15 }, (_, index) => `DEC-${String(index + 1).padStart(2, '0')}`)

const SHA256_PATTERN = /^[a-f0-9]{64}$/i
const DATE_PATTERN = /^\d{4}-\d{2}-\d{2}$/

const hasText = (value, minimum = 1) => typeof value === 'string' && value.trim().length >= minimum
const isSha256 = value => typeof value === 'string' && SHA256_PATTERN.test(value)
const isDate = value => {
  if (typeof value !== 'string' || !DATE_PATTERN.test(value)) return false
  const parsed = new Date(`${value}T00:00:00Z`)
  return !Number.isNaN(parsed.valueOf()) && parsed.toISOString().slice(0, 10) === value
}

export function sha256(value) {
  return createHash('sha256').update(value).digest('hex')
}

export function validateDossier(dossier, { requireOfficial = false } = {}) {
  const checks = Object.fromEntries(Array.from({ length: 6 }, (_, index) => [`N0${index + 1}`, []]))
  const add = (check, code, path, message) => checks[check].push({ code, path, message })

  if (!dossier || typeof dossier !== 'object' || Array.isArray(dossier)) {
    add('N01', 'DOSSIER_INVALID', '$', 'El expediente debe ser un objeto JSON.')
    return result(checks)
  }

  if (dossier.schemaVersion !== 1) add('N01', 'SCHEMA_VERSION', 'schemaVersion', 'Use la versión 1 del esquema.')
  if (!['synthetic', 'official-external'].includes(dossier.classification)) {
    add('N01', 'CLASSIFICATION', 'classification', 'Clasifique el expediente como synthetic u official-external.')
  }
  if (requireOfficial && dossier.classification !== 'official-external') {
    add('N01', 'OFFICIAL_REQUIRED', 'classification', 'Esta aprobación requiere el expediente oficial externo.')
  }

  const hotel = dossier.hotel ?? {}
  if (!hasText(hotel.legalName, 3)) add('N01', 'LEGAL_NAME', 'hotel.legalName', 'Falta la razón social.')
  if (!hasText(hotel.tradeName, 3)) add('N01', 'TRADE_NAME', 'hotel.tradeName', 'Falta el nombre comercial.')
  if (!/^\d{14}$/.test(hotel.rtn ?? '')) add('N01', 'RTN', 'hotel.rtn', 'El RTN debe contener 14 dígitos.')
  if (!hasText(hotel.address, 10)) add('N01', 'ADDRESS', 'hotel.address', 'Falta el domicilio fiscal completo.')
  if (!Array.isArray(hotel.phones) || !hotel.phones.some(phone => hasText(phone, 7))) add('N01', 'PHONE', 'hotel.phones', 'Registre al menos un teléfono.')
  if (!/^[^@\s]+@[^@\s]+\.[^@\s]+$/.test(hotel.email ?? '')) add('N01', 'EMAIL', 'hotel.email', 'Registre un correo válido.')

  const billing = dossier.billing ?? {}
  if (!/^\d{3}$/.test(billing.establishmentCode ?? '')) add('N01', 'ESTABLISHMENT', 'billing.establishmentCode', 'El establecimiento debe contener tres dígitos.')
  if (!/^\d{3}$/.test(billing.emissionPointCode ?? '')) add('N01', 'EMISSION_POINT', 'billing.emissionPointCode', 'El punto de emisión debe contener tres dígitos.')
  if (!hasText(billing.modality, 3)) add('N02', 'BILLING_MODALITY', 'billing.modality', 'Falta la modalidad de facturación aprobada.')
  if (!hasText(billing.systemRegistrationReference, 5)) add('N02', 'SYSTEM_REGISTRATION', 'billing.systemRegistrationReference', 'Falta la referencia externa del registro o trámite aplicable.')
  if (!Array.isArray(billing.authorizations) || billing.authorizations.length === 0) {
    add('N02', 'AUTHORIZATIONS', 'billing.authorizations', 'Registre al menos una autorización de documento.')
  } else {
    billing.authorizations.forEach((authorization, index) => {
      const base = `billing.authorizations[${index}]`
      if (!hasText(authorization.documentType, 3)) add('N02', 'DOCUMENT_TYPE', `${base}.documentType`, 'Falta el tipo documental.')
      if (!isSha256(authorization.caiReferenceHash)) add('N02', 'CAI_HASH', `${base}.caiReferenceHash`, 'Registre el SHA-256 de la evidencia CAI, no el documento privado.')
      if (!hasText(authorization.initialRange, 5)) add('N02', 'INITIAL_RANGE', `${base}.initialRange`, 'Falta el rango inicial.')
      if (!hasText(authorization.finalRange, 5)) add('N02', 'FINAL_RANGE', `${base}.finalRange`, 'Falta el rango final.')
      if (!isDate(authorization.validUntil)) add('N02', 'AUTH_VALID_UNTIL', `${base}.validUntil`, 'Falta una fecha final válida.')
    })
  }

  if (!Array.isArray(dossier.obligations) || dossier.obligations.length === 0) {
    add('N03', 'OBLIGATIONS', 'obligations', 'Registre todas las obligaciones activas o una decisión sustentada de no aplicación.')
  } else {
    dossier.obligations.forEach((obligation, index) => {
      const base = `obligations[${index}]`
      for (const field of ['code', 'responsible', 'sourceReference', 'testReference', 'treatment']) {
        if (!hasText(obligation[field], 3)) add('N03', `OBLIGATION_${field.toUpperCase()}`, `${base}.${field}`, `Falta ${field} en la obligación.`)
      }
    })
  }

  const format = dossier.documentFormat ?? {}
  if (!hasText(format.sourceReference, 5)) add('N04', 'FORMAT_SOURCE', 'documentFormat.sourceReference', 'Falta la referencia de la especificación vigente.')
  if (!isSha256(format.sourceSha256)) add('N04', 'FORMAT_HASH', 'documentFormat.sourceSha256', 'Falta el SHA-256 de la especificación.')
  if (!hasText(format.signedCaseReference, 5)) add('N04', 'SIGNED_CASE', 'documentFormat.signedCaseReference', 'Falta la referencia del caso de comparación firmado.')

  const printing = dossier.printing ?? {}
  if (!hasText(printing.printerModel, 3)) add('N05', 'PRINTER_MODEL', 'printing.printerModel', 'Falta el modelo de impresora.')
  if (![58, 76, 80].includes(printing.paperWidthMm)) add('N05', 'PAPER_WIDTH', 'printing.paperWidthMm', 'El ancho debe ser 58, 76 u 80 mm.')
  if (!Number.isInteger(printing.copies) || printing.copies < 1 || printing.copies > 3) add('N05', 'COPIES', 'printing.copies', 'Indique entre una y tres copias.')
  if (!hasText(printing.documentRetentionReference, 5)) add('N05', 'PRINT_RETENTION', 'printing.documentRetentionReference', 'Falta la referencia de conservación del documento.')

  const decisions = Array.isArray(dossier.decisions) ? dossier.decisions : []
  const byCode = new Map()
  for (const [index, decision] of decisions.entries()) {
    if (!DECISION_CODES.includes(decision?.code)) {
      add('N06', 'DECISION_CODE', `decisions[${index}].code`, 'El código de decisión no pertenece a DEC-01–DEC-15.')
      continue
    }
    if (byCode.has(decision.code)) add('N06', 'DECISION_DUPLICATE', `decisions[${index}].code`, `La decisión ${decision.code} está duplicada.`)
    byCode.set(decision.code, decision)
  }
  for (const code of DECISION_CODES) {
    const decision = byCode.get(code)
    if (!decision) {
      add('N06', 'DECISION_MISSING', `decisions.${code}`, `Falta ${code}.`)
      continue
    }
    if (!['approved', 'not_applicable'].includes(decision.status)) add('N06', 'DECISION_STATUS', `decisions.${code}.status`, 'La decisión debe estar aprobada o sustentada como no aplicable.')
    if (!hasText(decision.summary, 10)) add('N06', 'DECISION_SUMMARY', `decisions.${code}.summary`, 'La resolución debe explicar el criterio adoptado.')
    if (!Array.isArray(decision.evidenceReferences) || decision.evidenceReferences.length === 0 || decision.evidenceReferences.some(reference => !hasText(reference, 5))) {
      add('N06', 'DECISION_EVIDENCE', `decisions.${code}.evidenceReferences`, 'Cada decisión requiere al menos una referencia a evidencia externa.')
    }
  }

  const approvals = dossier.approvals ?? {}
  for (const role of ['manager', 'accountant']) {
    const approval = approvals[role] ?? {}
    if (!hasText(approval.reference, 5)) add('N06', 'APPROVAL_REFERENCE', `approvals.${role}.reference`, `Falta la referencia de aprobación de ${role}.`)
    if (!isDate(approval.approvedAt)) add('N06', 'APPROVAL_DATE', `approvals.${role}.approvedAt`, `Falta la fecha de aprobación de ${role}.`)
  }

  return result(checks)
}

function result(checks) {
  const summary = Object.fromEntries(Object.entries(checks).map(([code, issues]) => [code, issues.length === 0 ? 'passed' : 'failed']))
  const issues = Object.entries(checks).flatMap(([check, entries]) => entries.map(issue => ({ check, ...issue })))
  return { valid: issues.length === 0, checks: summary, issues }
}
