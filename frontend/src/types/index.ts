import type { components } from './api.generated'

import type { ApiResponse as Wire } from './api'

export type User = Wire<components['schemas']['UserDto']>

export type AuthResponse = Wire<components['schemas']['AuthResponse']>

export type LoginRequest = components['schemas']['LoginRequest']

export type RegisterRequest = components['schemas']['RegisterRequest']

export type RoomType = Wire<components['schemas']['RoomTypeDto']>

export type Room = Wire<components['schemas']['RoomDto']>

export type Reservation = Wire<components['schemas']['ReservationDto']>

export type Guest = Wire<components['schemas']['GuestDto']>

export type Customer = Wire<components['schemas']['CustomerDto']>

export type Category = Wire<components['schemas']['CategoryDto']>

export type Product = Wire<components['schemas']['ProductDto']>

export type InventoryMovement = Wire<components['schemas']['InventoryMovementDto']>

export type CAI = Wire<components['schemas']['CAIDto']>

export type Invoice = Wire<components['schemas']['InvoiceDto']>

type InvoiceItemResponse = Wire<components['schemas']['InvoiceItemDto']>
export type InvoiceItem = Omit<InvoiceItemResponse, 'id' | 'originalInvoiceItemId' | 'folioItemId'> & Partial<Pick<InvoiceItemResponse, 'id' | 'originalInvoiceItemId' | 'folioItemId'>>

export type CashRegister = Wire<components['schemas']['CashRegisterDto']>

export type PaymentApplication = Wire<components['schemas']['PaymentApplicationDto']>

export type RefundApplication = Wire<components['schemas']['RefundApplicationDto']>

export type Refund = Wire<components['schemas']['RefundDto']>

export type Payment = Wire<components['schemas']['PaymentDto']>

export type EligibleCardPayment = Wire<components['schemas']['EligibleCardPaymentDto']>

export type CardSettlementApplication = Wire<components['schemas']['CardSettlementApplicationDto']>

export type CardSettlement = Wire<components['schemas']['CardSettlementDto']>

export type CashMovement = Wire<components['schemas']['CashMovementDto']>

export type DocumentAuthorization = Wire<components['schemas']['DocumentAuthorizationDto']>

export type BusinessSettings = Wire<components['schemas']['BusinessSettingsDto']>

export type FiscalProfileRevision = Wire<components['schemas']['FiscalProfileRevisionDto']>


export type AccountingAccount = Wire<components['schemas']['AccountingAccountDto']>

export type AccountingEntry = Wire<components['schemas']['AccountingEntryDto']>

export type EntryItem = Wire<components['schemas']['EntryItemDto']>

export type DashboardStat = Wire<components['schemas']['DashboardStatDto']>

export type DashboardAlert = Wire<components['schemas']['DashboardAlertDto']>

export type DashboardInvoice = Wire<components['schemas']['DashboardInvoiceDto']>

export type DashboardReservation = Wire<components['schemas']['DashboardReservationDto']>

export type DashboardCash = Wire<components['schemas']['DashboardCashDto']>

export type DashboardSummary = Wire<components['schemas']['DashboardSummaryDto']>

export type TrialBalanceItem = Wire<components['schemas']['TrialBalanceItem']>

export type AccountMovement = Wire<components['schemas']['AccountMovementDto']>

export type CreateAccountRequest = components['schemas']['CreateAccountRequest']

export type Discount = Wire<components['schemas']['DiscountDto']>

export type FolioItem = Wire<components['schemas']['FolioItemDto']>

export type Folio = Wire<components['schemas']['FolioDto']>

export type BackupLog = Wire<components['schemas']['BackupLogDto']>

export type Role = Wire<components['schemas']['RoleDto']>

export type CreateUserRequest = components['schemas']['CreateUserRequest']

export type UpdateUserRequest = components['schemas']['UpdateUserRequest']

export type FolioList = Wire<components['schemas']['FolioListDto']>
export type InventorySummary = Wire<components['schemas']['InventorySummaryDto']>

export type CashRegisterOperation = Wire<components['schemas']['CashRegisterOperationDto']>

export type AccountingEntryList = Wire<components['schemas']['AccountingEntryListDto']>

export type JournalSummary = Wire<components['schemas']['JournalSummaryDto']>

export type EligibleCardSummary = Wire<components['schemas']['EligibleCardSummaryDto']>

export type AuthorizationOption = Wire<components['schemas']['AuthorizationOptionDto']>

export type GuestStats = Wire<components['schemas']['GuestStatsDto']>

export type CheckInResult = Wire<components['schemas']['CheckInResult']>

export type PrintPreview = Wire<components['schemas']['PrintPreviewDto']>
export type AuditIntegrity = Wire<components['schemas']['AuditIntegrityResult']>

export type RoleOption = Wire<components['schemas']['RoleOptionDto']>
