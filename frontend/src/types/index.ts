import type { components } from './api.generated'

import type { ApiResponse as Wire } from './api'

export interface User {
  id: string
  username: string
  email: string
  firstName: string
  lastName: string
  isActive: boolean
  mustChangePassword: boolean
  roles: string[]
  permissions: string[]
}

export interface AuthResponse {
  success: boolean
  message?: string
  accessToken?: string
  expiresAt?: string
  user?: User
}

export interface LoginRequest {
  username: string
  password: string
}

export interface RegisterRequest {
  username: string
  password: string
  email: string
  firstName: string
  lastName: string
}

export interface RoomType {
  id: string
  name: string
  description?: string
  pricePerNight: number
  capacity: number
}

export interface Room {
  id: string
  roomNumber: string
  floor: number
  roomTypeId: string
  roomTypeName: string
  status: string
  observations?: string
  pricePerNight: number
  capacity: number
}

export type Reservation = Wire<components['schemas']['ReservationDto']>

export type Guest = Wire<components['schemas']['GuestDto']>

export type Customer = Wire<components['schemas']['CustomerDto']>

export type Category = Wire<components['schemas']['CategoryDto']>

export type Product = Wire<components['schemas']['ProductDto']>

export type InventoryMovement = Wire<components['schemas']['InventoryMovementDto']>

export interface CAI {
  id: string
  caiNumber: string
  issueDate: string
  dueDate: string
  initialRange: string
  finalRange: string
  currentCorrelative: string
  status: string
  isExpiringSoon: boolean
}

export type Invoice = Wire<components['schemas']['InvoiceDto']>

type InvoiceItemResponse = Wire<components['schemas']['InvoiceItemDto']>
export type InvoiceItem = Omit<InvoiceItemResponse, 'id' | 'originalInvoiceItemId' | 'folioItemId'> & Partial<Pick<InvoiceItemResponse, 'id' | 'originalInvoiceItemId' | 'folioItemId'>>

export type CashRegister = Wire<components['schemas']['CashRegisterDto']>

export interface PaymentApplication {
  id: string
  invoiceId: string
  correlativeNumber: string
  amount: number
}

export interface RefundApplication {
  id: string
  creditNoteId: string
  creditNoteCorrelativeNumber: string
  amount: number
}

export interface Refund {
  id: string
  refundNumber: string
  paymentId: string
  paymentNumber: string
  recordedByUserId: string
  recordedByUserName: string
  method: 'Efectivo' | 'Tarjeta' | 'Transferencia'
  currency: 'HNL'
  amount: number
  externalReference?: string | null
  reason: string
  refundDate: string
  status: 'Confirmado' | 'Anulado'
  cashRegisterId?: string | null
  cashRegisterName?: string | null
  accountingEntryId?: string | null
  applications: RefundApplication[]
}

export interface Payment {
  id: string
  paymentNumber: string
  recordedByUserId: string
  recordedByUserName: string
  method: 'Efectivo' | 'Tarjeta' | 'Transferencia'
  currency: 'HNL'
  amount: number
  cashReceived?: number | null
  cashChange?: number | null
  externalReference?: string | null
  paymentDate: string
  status: 'Confirmado' | 'Anulado' | 'ParcialmenteReembolsado' | 'Reembolsado'
  cashRegisterId?: string | null
  cashRegisterName?: string | null
  accountingEntryId?: string | null
  refundedAmount: number
  cardSettledAmount: number
  applications: PaymentApplication[]
  refunds: Refund[]
}

export type EligibleCardPayment = Wire<components['schemas']['EligibleCardPaymentDto']>

export type CardSettlementApplication = Wire<components['schemas']['CardSettlementApplicationDto']>

export type CardSettlement = Wire<components['schemas']['CardSettlementDto']>

export type CashMovement = Wire<components['schemas']['CashMovementDto']>

export interface DocumentAuthorization {
  id: string
  documentType: string
  caiNumber: string
  issueDate: string
  dueDate: string
  initialRange: string
  finalRange: string
  currentCorrelative: string
  status: string
  isExpiringSoon: boolean
  hasAttachment: boolean
}

export interface BusinessSettings {
  businessName: string
  rtn: string
  address: string
  phone: string
  email: string
  logoBase64?: string
  footer: string
  isvRate: number
  touristTaxRate: number
  fiscalProfileStatus: 'Borrador' | 'Aprobado' | 'Retirado'
  fiscalProfileVersion: number
  fiscalValidFrom?: string
  fiscalValidUntil?: string
  fiscalApprovedAt?: string
  fiscalApprovedByUserId?: string
  fiscalApprovalNote?: string
  fiscalRetiredAt?: string
  fiscalRetiredByUserId?: string
  fiscalRetirementReason?: string
  fiscalDecisionManifestSha256?: string
  fiscalEvidenceReference?: string
  printPrinterName?: string
  printWidth: number
  printLogoHeight: number
  printFontSize: string
  printLineSpacing: number
  showLogo: boolean
  showHeader: boolean
  showFiscal: boolean
  showGuest: boolean
  showItems: boolean
  showTotals: boolean
  showPayment: boolean
  showFooter: boolean
  headerAlign: string
  separatorChar: string
  marginLeft: number
}

export interface FiscalProfileRevision {
  version: number
  status: 'Aprobado' | 'Retirado'
  validFrom: string
  validUntil?: string
  decisionManifestSha256: string
  evidenceReference: string
  snapshotSha256: string
  approvedAt: string
  approvedByUserId: string
  approvalNote: string
  retiredAt?: string
  retiredByUserId?: string
  retirementReason?: string
}

export interface InvoicePrintData {
  header: { logo: string; hotel: string; rtn: string; direccion: string; telefono: string }
  tipo: string
  factura: { cai: string; correlativo: string; rango: string; fecha: string; fechaLimite: string }
  cliente: { nombre: string; rtn: string; checkIn?: string; checkOut?: string }
  items: { desc: string; cant: number; precio: number; total: number }[]
  totales: { subtotal: number; isv: number; tasaTuristico: number; descuentos?: number; total: number }
  pago: { metodo: string; efectivoRecibido?: number; cambio?: number; letras: string }
  footer: string
}

export interface AccountingAccount {
  id: string
  accountNumber: string
  accountName: string
  accountType: string
  parentAccountId?: string
  isActive: boolean
  balance: number
  children: AccountingAccount[]
}

export type AccountingEntry = Wire<components['schemas']['AccountingEntryDto']>

export type EntryItem = Wire<components['schemas']['EntryItemDto']>

export interface DashboardStat {
  title: string
  value: string
  trend?: string
}

export interface DashboardAlert {
  title: string
  description: string
  severity: string
}

export interface DashboardInvoice {
  correlativeNumber: string
  customerName: string
  totalAmount: number
  status: string
  invoiceDate: string
}

export interface DashboardReservation {
  guestName: string
  roomNumber: string
  status: string
  checkInDate: string
  checkOutDate: string
}

export interface DashboardCash {
  registerName: string
  balance: number
}

export interface DashboardSummary {
  stats: DashboardStat[]
  alerts: DashboardAlert[]
  recentInvoices: DashboardInvoice[]
  upcomingReservations: DashboardReservation[]
  cash?: DashboardCash | null
  occupiedRooms: number
  freeRooms: number
  pendingCheckIns: number
  pendingCheckOuts: number
}

export interface TrialBalanceItem {
  accountId: string
  accountNumber: string
  accountName: string
  accountType: string
  previousBalance: number
  debit: number
  credit: number
  balance: number
}

export interface AccountMovement {
  date: string
  description: string
  debit: number
  credit: number
  balance: number
  referenceType: string
}

export interface CreateAccountRequest {
  accountNumber: string
  accountName: string
  accountType: string
  parentAccountId?: string
}

export interface Discount {
  id: string
  name: string
  description?: string
  discountType: string
  value: number
  isActive: boolean
  applicableTo?: string
  requiresDocument: boolean
  minAge?: number
  priority: number
}

export type FolioItem = Wire<components['schemas']['FolioItemDto']>

export type Folio = Wire<components['schemas']['FolioDto']>

export type BackupLog = Wire<components['schemas']['BackupLogDto']>

export interface Role {
  id: string
  name: string
  description?: string
  permissions?: string[]
}

export interface CreateUserRequest {
  username: string
  password: string
  email: string
  firstName: string
  lastName: string
  roles?: string[]
}

export interface UpdateUserRequest {
  firstName?: string
  lastName?: string
  email?: string
  isActive?: boolean
  roles?: string[]
}

export type FolioList = Wire<components['schemas']['FolioListDto']>
export type InventorySummary = Wire<components['schemas']['InventorySummaryDto']>

export type CashRegisterOperation = Wire<components['schemas']['CashRegisterOperationDto']>

export type AccountingEntryList = Wire<components['schemas']['AccountingEntryListDto']>

export type JournalSummary = Wire<components['schemas']['JournalSummaryDto']>

export type EligibleCardSummary = Wire<components['schemas']['EligibleCardSummaryDto']>
