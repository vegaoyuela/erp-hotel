import type { paths, components } from './api.generated'

// ASP.NET accepts numeric strings in requests but emits numeric JSON tokens in responses.
export type ApiResponse<T> = number extends T ? number | Extract<T, null | undefined> : T extends (infer Item)[] ? ApiResponse<Item>[] : T extends object ? { [Key in keyof T]-?: ApiResponse<T[Key]> } : T

type GuestPage = paths['/api/Guests/paged']['get']['responses'][200]['content']['application/json']
export type PagedResult<T> = Omit<ApiResponse<GuestPage>, 'items'> & { items: T[] }
export type ApiGuest = ApiResponse<components['schemas']['GuestDto']>
export type ApiInvoice = ApiResponse<components['schemas']['InvoiceDto']>
export type ApiAuditLog = ApiResponse<components['schemas']['AuditLogDto']>
