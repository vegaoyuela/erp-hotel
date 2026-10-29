// Navigation and route guards use the same effective permissions as the API.
export const access = {
  guests: ['manage_reservations', 'manage_accounting'],
  folios: ['manage_reservations', 'create_invoices', 'manage_cash', 'manage_accounting'],
  invoices: ['create_invoices', 'adjust_invoices', 'manage_cash', 'manage_accounting', 'manage_taxes'],
  payments: ['manage_cash', 'manage_accounting'],
} as const

export const routePermissions: Record<string, readonly string[]> = {
  '/dashboard': [],
  '/rooms': ['manage_reservations'],
  '/rooms/types': ['manage_reservations'],
  '/rooms/map': ['manage_reservations'],
  '/reservations': ['manage_reservations'],
  '/checkin': ['manage_reservations'],
  '/checkout': ['manage_reservations'],
  '/guests': access.guests,
  '/customers': access.guests,
  '/folios': access.folios,
  '/invoices': access.invoices,
  '/authorizations': ['manage_taxes'],
  '/cash': ['manage_cash'],
  '/card-settlements': ['manage_accounting'],
  '/inventory': ['manage_inventory'],
  '/reports': ['view_reports'],
  '/accounting/chart-of-accounts': ['manage_accounting'],
  '/accounting/journal-entries': ['manage_accounting'],
  '/accounting/trial-balance': ['manage_accounting'],
  '/users': ['manage_users'],
  '/audit-logs': ['view_audit'],
  '/discounts': ['manage_settings'],
  '/backups': ['manage_backups'],
  '/settings': ['manage_settings'],
}
