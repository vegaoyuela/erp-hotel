// Generated from the Hotel ERP OpenAPI contract. Run npm run api:generate.
export interface paths {
    "/api/Accounts": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Accounts: GetAll */
        get: operations["Accounts_GetAll"];
        put?: never;
        /** Accounts: Create */
        post: operations["Accounts_Create"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Accounts/flat": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Accounts: GetAllFlat */
        get: operations["Accounts_GetAllFlat"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Accounts/journal-entries": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Accounts: GetJournalEntries */
        get: operations["Accounts_GetJournalEntries"];
        put?: never;
        /** Accounts: CreateJournalEntry */
        post: operations["Accounts_CreateJournalEntry"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Accounts/journal-entries/paged": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Accounts: GetJournalPage */
        get: operations["Accounts_GetJournalPage"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Accounts/journal-entries/summary": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Accounts: GetJournalSummary */
        get: operations["Accounts_GetJournalSummary"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Accounts/journal-entries/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Accounts: GetJournalEntry */
        get: operations["Accounts_GetJournalEntry"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Accounts/trial-balance": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Accounts: GetTrialBalance */
        get: operations["Accounts_GetTrialBalance"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Accounts/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Accounts: GetById */
        get: operations["Accounts_GetById"];
        /** Accounts: Update */
        put: operations["Accounts_Update"];
        post?: never;
        /** Accounts: Deactivate */
        delete: operations["Accounts_Deactivate"];
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Accounts/{id}/movements": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Accounts: GetMovements */
        get: operations["Accounts_GetMovements"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Auth/change-password": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** Auth: ChangePassword */
        post: operations["Auth_ChangePassword"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Auth/login": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** Auth: Login */
        post: operations["Auth_Login"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Auth/logout": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** Auth: Logout */
        post: operations["Auth_Logout"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Auth/me": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Auth: GetCurrentUser */
        get: operations["Auth_GetCurrentUser"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Auth/refresh": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** Auth: Refresh */
        post: operations["Auth_Refresh"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Auth/register": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** Auth: Register */
        post: operations["Auth_Register"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Backup/logs": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Backup: GetLogs */
        get: operations["Backup_GetLogs"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Backup/logs/paged": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Backup: GetLogsPage */
        get: operations["Backup_GetLogsPage"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Backup/manual": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** Backup: CreateManualBackup */
        post: operations["Backup_CreateManualBackup"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/CAI": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** CAI: GetAll */
        get: operations["CAI_GetAll"];
        put?: never;
        /** CAI: Create */
        post: operations["CAI_Create"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/CAI/active": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** CAI: GetActive */
        get: operations["CAI_GetActive"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/CAI/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** CAI: GetById */
        get: operations["CAI_GetById"];
        put?: never;
        post?: never;
        /** CAI: Delete */
        delete: operations["CAI_Delete"];
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Customers": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Customers: GetAll */
        get: operations["Customers_GetAll"];
        put?: never;
        /** Customers: Create */
        post: operations["Customers_Create"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Customers/paged": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Customers: GetPage */
        get: operations["Customers_GetPage"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Customers/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Customers: GetById */
        get: operations["Customers_GetById"];
        /** Customers: Update */
        put: operations["Customers_Update"];
        post?: never;
        /** Customers: Delete */
        delete: operations["Customers_Delete"];
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Discounts": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Discounts: GetAll */
        get: operations["Discounts_GetAll"];
        put?: never;
        /** Discounts: Create */
        post: operations["Discounts_Create"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Discounts/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Discounts: GetById */
        get: operations["Discounts_GetById"];
        /** Discounts: Update */
        put: operations["Discounts_Update"];
        post?: never;
        /** Discounts: Delete */
        delete: operations["Discounts_Delete"];
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Folios": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Folios: GetAll */
        get: operations["Folios_GetAll"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Folios/by-reservation/{reservationId}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Folios: GetByReservation */
        get: operations["Folios_GetByReservation"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Folios/paged": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Folios: GetPage */
        get: operations["Folios_GetPage"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Folios/{folioId}/items": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** Folios: AddItem */
        post: operations["Folios_AddItem"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Folios/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Folios: GetById */
        get: operations["Folios_GetById"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Guests": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Guests: GetAll */
        get: operations["Guests_GetAll"];
        put?: never;
        /** Guests: Create */
        post: operations["Guests_Create"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Guests/paged": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Guests: GetPage */
        get: operations["Guests_GetPage"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Guests/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Guests: GetById */
        get: operations["Guests_GetById"];
        /** Guests: Update */
        put: operations["Guests_Update"];
        post?: never;
        /** Guests: Delete */
        delete: operations["Guests_Delete"];
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Guests/{id}/classification": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        /** Guests: UpdateClassification */
        patch: operations["Guests_UpdateClassification"];
        trace?: never;
    };
    "/api/Guests/{id}/stats": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Guests: GetStats */
        get: operations["Guests_GetStats"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Inventory/categories": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Inventory: GetCategories */
        get: operations["Inventory_GetCategories"];
        put?: never;
        /** Inventory: CreateCategory */
        post: operations["Inventory_CreateCategory"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Inventory/categories/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        /** Inventory: UpdateCategory */
        put: operations["Inventory_UpdateCategory"];
        post?: never;
        /** Inventory: DeleteCategory */
        delete: operations["Inventory_DeleteCategory"];
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Inventory/movements": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Inventory: GetMovements */
        get: operations["Inventory_GetMovements"];
        put?: never;
        /** Inventory: CreateMovement */
        post: operations["Inventory_CreateMovement"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Inventory/movements/paged": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Inventory: GetMovementsPage */
        get: operations["Inventory_GetMovementsPage"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Inventory/products": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Inventory: GetProducts */
        get: operations["Inventory_GetProducts"];
        put?: never;
        /** Inventory: CreateProduct */
        post: operations["Inventory_CreateProduct"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Inventory/products/paged": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Inventory: GetProductsPage */
        get: operations["Inventory_GetProductsPage"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Inventory/products/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        /** Inventory: UpdateProduct */
        put: operations["Inventory_UpdateProduct"];
        post?: never;
        /** Inventory: DeleteProduct */
        delete: operations["Inventory_DeleteProduct"];
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Inventory/summary": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Inventory: GetSummary */
        get: operations["Inventory_GetSummary"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Invoices": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Invoices: GetAll */
        get: operations["Invoices_GetAll"];
        put?: never;
        /** Invoices: Create */
        post: operations["Invoices_Create"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Invoices/date-range": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Invoices: GetByDateRange */
        get: operations["Invoices_GetByDateRange"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Invoices/paged": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Invoices: GetPage */
        get: operations["Invoices_GetPage"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Invoices/search": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Invoices: Search */
        get: operations["Invoices_Search"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Invoices/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Invoices: GetById */
        get: operations["Invoices_GetById"];
        /** Invoices: Update */
        put: operations["Invoices_Update"];
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Invoices/{id}/cancel": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** Invoices: Cancel */
        post: operations["Invoices_Cancel"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Invoices/{id}/credit-note": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** Invoices: CreateCreditNote */
        post: operations["Invoices_CreateCreditNote"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Invoices/{id}/debit-note": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** Invoices: CreateDebitNote */
        post: operations["Invoices_CreateDebitNote"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Print/invoice/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** Print: PrintInvoice */
        post: operations["Print_PrintInvoice"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Print/invoice/{id}/preview": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Print: GetPreview */
        get: operations["Print_GetPreview"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Print/printers": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Print: GetPrinters */
        get: operations["Print_GetPrinters"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Print/test": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Print: TestPrinter */
        get: operations["Print_TestPrinter"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Print/test-preview": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Print: GetTestPreview */
        get: operations["Print_GetTestPreview"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Print/test-print": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** Print: PrintTest */
        post: operations["Print_PrintTest"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Print/test-ruler": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** Print: GetTestRuler */
        post: operations["Print_GetTestRuler"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Reservations": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Reservations: GetAll */
        get: operations["Reservations_GetAll"];
        put?: never;
        /** Reservations: Create */
        post: operations["Reservations_Create"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Reservations/calendar": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Reservations: GetCalendar */
        get: operations["Reservations_GetCalendar"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Reservations/checkin": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** Reservations: CheckIn */
        post: operations["Reservations_CheckIn"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Reservations/checkout/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** Reservations: CheckOut */
        post: operations["Reservations_CheckOut"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Reservations/date-range": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Reservations: GetByDateRange */
        get: operations["Reservations_GetByDateRange"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Reservations/paged": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Reservations: GetPage */
        get: operations["Reservations_GetPage"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Reservations/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Reservations: GetById */
        get: operations["Reservations_GetById"];
        /** Reservations: Update */
        put: operations["Reservations_Update"];
        post?: never;
        /** Reservations: Delete */
        delete: operations["Reservations_Delete"];
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Reservations/{id}/cancel": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** Reservations: Cancel */
        post: operations["Reservations_Cancel"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Reservations/{id}/confirm": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** Reservations: Confirm */
        post: operations["Reservations_Confirm"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Roles": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Roles: GetAll */
        get: operations["Roles_GetAll"];
        put?: never;
        /** Roles: Create */
        post: operations["Roles_Create"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Roles/permissions": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Roles: GetPermissions */
        get: operations["Roles_GetPermissions"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Roles/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Roles: GetById */
        get: operations["Roles_GetById"];
        /** Roles: Update */
        put: operations["Roles_Update"];
        post?: never;
        /** Roles: Delete */
        delete: operations["Roles_Delete"];
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Rooms": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Rooms: GetAll */
        get: operations["Rooms_GetAll"];
        put?: never;
        /** Rooms: Create */
        post: operations["Rooms_Create"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Rooms/available": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Rooms: GetAvailable */
        get: operations["Rooms_GetAvailable"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Rooms/status/{status}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Rooms: GetByStatus */
        get: operations["Rooms_GetByStatus"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Rooms/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Rooms: GetById */
        get: operations["Rooms_GetById"];
        /** Rooms: Update */
        put: operations["Rooms_Update"];
        post?: never;
        /** Rooms: Delete */
        delete: operations["Rooms_Delete"];
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Suppliers": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Suppliers: GetAll */
        get: operations["Suppliers_GetAll"];
        put?: never;
        /** Suppliers: Create */
        post: operations["Suppliers_Create"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Suppliers/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Suppliers: GetById */
        get: operations["Suppliers_GetById"];
        /** Suppliers: Update */
        put: operations["Suppliers_Update"];
        post?: never;
        /** Suppliers: Delete */
        delete: operations["Suppliers_Delete"];
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Users": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Users: GetAll */
        get: operations["Users_GetAll"];
        put?: never;
        /** Users: Create */
        post: operations["Users_Create"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Users/paged": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Users: GetPage */
        get: operations["Users_GetPage"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Users/role-options/paged": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Users: GetRoleOptions */
        get: operations["Users_GetRoleOptions"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Users/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Users: GetById */
        get: operations["Users_GetById"];
        /** Users: Update */
        put: operations["Users_Update"];
        post?: never;
        /** Users: Delete */
        delete: operations["Users_Delete"];
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Users/{id}/reset-password": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** Users: ResetPassword */
        post: operations["Users_ResetPassword"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/Users/{userId}/roles/{roleId}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** Users: AssignRole */
        post: operations["Users_AssignRole"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/audit-logs": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** AuditLogs: GetLogs */
        get: operations["AuditLogs_GetLogs"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/audit-logs/actions": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** AuditLogs: GetActions */
        get: operations["AuditLogs_GetActions"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/audit-logs/export-integrity": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** AuditLogs: ExportIntegrity */
        get: operations["AuditLogs_ExportIntegrity"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/audit-logs/paged": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** AuditLogs: GetPage */
        get: operations["AuditLogs_GetPage"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/audit-logs/verify-integrity": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** AuditLogs: VerifyIntegrity */
        get: operations["AuditLogs_VerifyIntegrity"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/card-settlements": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** CardSettlements: GetAll */
        get: operations["CardSettlements_GetAll"];
        put?: never;
        /** CardSettlements: Create */
        post: operations["CardSettlements_Create"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/card-settlements/eligible-payments": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** CardSettlements: GetEligiblePayments */
        get: operations["CardSettlements_GetEligiblePayments"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/card-settlements/eligible-payments/paged": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** CardSettlements: GetEligiblePage */
        get: operations["CardSettlements_GetEligiblePage"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/card-settlements/eligible-payments/summary": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** CardSettlements: GetEligibleSummary */
        get: operations["CardSettlements_GetEligibleSummary"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/card-settlements/paged": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** CardSettlements: GetPage */
        get: operations["CardSettlements_GetPage"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/card-settlements/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** CardSettlements: GetById */
        get: operations["CardSettlements_GetById"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/cash-registers": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** CashRegisters: GetAll */
        get: operations["CashRegisters_GetAll"];
        put?: never;
        /** CashRegisters: Create */
        post: operations["CashRegisters_Create"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/cash-registers/paged": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** CashRegisters: GetPage */
        get: operations["CashRegisters_GetPage"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/cash-registers/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** CashRegisters: GetById */
        get: operations["CashRegisters_GetById"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/cash-registers/{id}/close": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** CashRegisters: Close */
        post: operations["CashRegisters_Close"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/cash-registers/{id}/movements": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** CashRegisters: GetMovements */
        get: operations["CashRegisters_GetMovements"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/cash-registers/{id}/movements/paged": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** CashRegisters: GetMovementsPage */
        get: operations["CashRegisters_GetMovementsPage"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/cash-registers/{id}/open": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** CashRegisters: Open */
        post: operations["CashRegisters_Open"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/dashboard/summary": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Dashboard: GetSummary */
        get: operations["Dashboard_GetSummary"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/data/export/guests": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** DataExport: ExportGuestsXlsx */
        get: operations["DataExport_ExportGuestsXlsx"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/data/export/invoices": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** DataExport: ExportInvoicesXlsx */
        get: operations["DataExport_ExportInvoicesXlsx"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/data/export/reservations": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** DataExport: ExportReservationsXlsx */
        get: operations["DataExport_ExportReservationsXlsx"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/data/export/rooms": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** DataExport: ExportRoomsXlsx */
        get: operations["DataExport_ExportRoomsXlsx"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/document-authorizations": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** DocumentAuthorizations: GetAll */
        get: operations["DocumentAuthorizations_GetAll"];
        put?: never;
        /** DocumentAuthorizations: Create */
        post: operations["DocumentAuthorizations_Create"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/document-authorizations/active/{documentType}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** DocumentAuthorizations: GetActive */
        get: operations["DocumentAuthorizations_GetActive"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/document-authorizations/options/paged": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** DocumentAuthorizations: GetOptionsPage */
        get: operations["DocumentAuthorizations_GetOptionsPage"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/document-authorizations/with-file": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** DocumentAuthorizations: CreateWithFile */
        post: operations["DocumentAuthorizations_CreateWithFile"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/document-authorizations/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** DocumentAuthorizations: GetById */
        get: operations["DocumentAuthorizations_GetById"];
        put?: never;
        post?: never;
        /** DocumentAuthorizations: Delete */
        delete: operations["DocumentAuthorizations_Delete"];
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/document-authorizations/{id}/file": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** DocumentAuthorizations: GetFile */
        get: operations["DocumentAuthorizations_GetFile"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/payments": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Payments: GetAll */
        get: operations["Payments_GetAll"];
        put?: never;
        /** Payments: Create */
        post: operations["Payments_Create"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/payments/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Payments: GetById */
        get: operations["Payments_GetById"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/payments/{paymentId}/refunds": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Refunds: GetAll */
        get: operations["Refunds_GetAll"];
        put?: never;
        /** Refunds: Create */
        post: operations["Refunds_Create"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/payments/{paymentId}/refunds/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Refunds: GetById */
        get: operations["Refunds_GetById"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/purchase-invoices": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** PurchaseInvoices: GetAll */
        get: operations["PurchaseInvoices_GetAll"];
        put?: never;
        /** PurchaseInvoices: Create */
        post: operations["PurchaseInvoices_Create"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/purchase-invoices/date-range": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** PurchaseInvoices: GetByDateRange */
        get: operations["PurchaseInvoices_GetByDateRange"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/purchase-invoices/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** PurchaseInvoices: GetById */
        get: operations["PurchaseInvoices_GetById"];
        put?: never;
        post?: never;
        /** PurchaseInvoices: Delete */
        delete: operations["PurchaseInvoices_Delete"];
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/purchase-invoices/{id}/pay": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        /** PurchaseInvoices: MarkAsPaid */
        put: operations["PurchaseInvoices_MarkAsPaid"];
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/reports/financial/balance-sheet": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Reports: GetBalanceSheet */
        get: operations["Reports_GetBalanceSheet"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/reports/financial/income-statement": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Reports: GetIncomeStatement */
        get: operations["Reports_GetIncomeStatement"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/reports/sar/purchases-book": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Reports: GetPurchasesBookXlsx */
        get: operations["Reports_GetPurchasesBookXlsx"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/reports/sar/sales-book": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Reports: GetSalesBookXlsx */
        get: operations["Reports_GetSalesBookXlsx"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/reports/tax-summary": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Reports: GetTaxSummary */
        get: operations["Reports_GetTaxSummary"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/room-types": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** RoomTypes: GetAll */
        get: operations["RoomTypes_GetAll"];
        put?: never;
        /** RoomTypes: Create */
        post: operations["RoomTypes_Create"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/room-types/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** RoomTypes: GetById */
        get: operations["RoomTypes_GetById"];
        /** RoomTypes: Update */
        put: operations["RoomTypes_Update"];
        post?: never;
        /** RoomTypes: Delete */
        delete: operations["RoomTypes_Delete"];
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/settings/business": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Settings: Get */
        get: operations["Settings_Get"];
        /** Settings: Update */
        put: operations["Settings_Update"];
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/settings/business/approve": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** Settings: ApproveFiscalProfile */
        post: operations["Settings_ApproveFiscalProfile"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/settings/business/history": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** Settings: GetFiscalProfileHistory */
        get: operations["Settings_GetFiscalProfileHistory"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/settings/business/retire": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        /** Settings: RetireFiscalProfile */
        post: operations["Settings_RetireFiscalProfile"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/tax-configurations": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        /** TaxConfigurations: GetAll */
        get: operations["TaxConfigurations_GetAll"];
        put?: never;
        /** TaxConfigurations: Create */
        post: operations["TaxConfigurations_Create"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/api/tax-configurations/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        /** TaxConfigurations: Update */
        put: operations["TaxConfigurations_Update"];
        post?: never;
        /** TaxConfigurations: Delete */
        delete: operations["TaxConfigurations_Delete"];
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/health": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get: operations["Health_Get"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
}
export type webhooks = Record<string, never>;
export interface components {
    schemas: {
        AccountMovementDto: {
            /** Format: double */
            balance?: number | string;
            /** Format: double */
            credit?: number | string;
            /** Format: date-time */
            date?: string;
            /** Format: double */
            debit?: number | string;
            description?: string;
            referenceType?: string;
        };
        AccountingAccountDto: {
            accountName?: string;
            accountNumber?: string;
            accountType?: string;
            /** Format: double */
            balance?: number | string;
            children?: components["schemas"]["AccountingAccountDto"][];
            /** Format: uuid */
            id?: string;
            isActive?: boolean;
            /** Format: uuid */
            parentAccountId?: null | string;
        };
        AccountingEntryDto: {
            description?: string;
            entryType?: string;
            /** Format: uuid */
            id?: string;
            items?: components["schemas"]["EntryItemDto"][];
            /** Format: uuid */
            referenceId?: null | string;
            /** Format: double */
            totalCredit?: number | string;
            /** Format: double */
            totalDebit?: number | string;
            /** Format: date */
            transactionDate?: string;
        };
        AccountingEntryListDto: {
            description?: string;
            entryType?: string;
            /** Format: uuid */
            id?: string;
            /** Format: int32 */
            itemCount?: number | string;
            /** Format: double */
            totalCredit?: number | string;
            /** Format: double */
            totalDebit?: number | string;
            /** Format: date */
            transactionDate?: string;
        };
        AddFolioItemRequest: {
            description: string;
            /** Format: double */
            discountPercentage: number | string;
            isExempt: boolean;
            isTouristTaxable: boolean;
            /** Format: double */
            isvRate: number | string;
            /** Format: int32 */
            quantity: number | string;
            /** Format: double */
            unitPrice: number | string;
        };
        AdminResetPasswordRequest: {
            newPassword: string;
        };
        ApproveFiscalProfileRequest: {
            approvalNote: string;
            decisionManifestSha256: string;
            evidenceReference: string;
            /** Format: date */
            validFrom: string;
            /** Format: date */
            validUntil: null | string;
        };
        AuditChainExport: {
            /** Format: date-time */
            generatedAt: string;
            integrity: components["schemas"]["AuditIntegrityResult"];
            records: components["schemas"]["AuditChainExportEntry"][];
            /** Format: int32 */
            schemaVersion: number | string;
        };
        AuditChainExportEntry: {
            action: string;
            changes: null | string;
            correlativeNumber: null | string;
            /** Format: uuid */
            entityId: null | string;
            entityName: null | string;
            hash: string;
            /** Format: date-time */
            hondurasTimestamp: string;
            /** Format: uuid */
            id: string;
            paymentMethod: null | string;
            previousHash: null | string;
            /** Format: date-time */
            timestamp: string;
            /** Format: uuid */
            userId: null | string;
        };
        AuditIntegrityResult: {
            /** Format: uuid */
            brokenLogId?: null | string;
            errorCode?: null | string;
            errorMessage?: null | string;
            isValid: boolean;
            /** Format: int32 */
            totalRecordsVerified: number | string;
            /** Format: date-time */
            verifiedAt: string;
        };
        AuditLogDto: {
            action?: string;
            changes?: null | string;
            correlativeNumber?: null | string;
            /** Format: uuid */
            entityId?: null | string;
            entityName?: null | string;
            hash?: null | string;
            /** Format: date-time */
            hondurasTimestamp?: string;
            /** Format: uuid */
            id?: string;
            paymentMethod?: null | string;
            previousHash?: null | string;
            /** Format: date-time */
            timestamp?: string;
            userFullName?: null | string;
            /** Format: uuid */
            userId?: null | string;
            userName?: null | string;
        };
        AuthResponse: {
            accessToken?: null | string;
            /** Format: date-time */
            expiresAt?: null | string;
            message?: null | string;
            success?: boolean;
            user?: null | components["schemas"]["UserDto"];
        };
        AuthorizationOptionDto: {
            caiNumber?: string;
            currentCorrelative?: string;
            documentType?: string;
            /** Format: date */
            dueDate?: string;
            finalRange?: string;
            hasAttachment?: boolean;
            /** Format: uuid */
            id?: string;
            initialRange?: string;
            isExpiringSoon?: boolean;
            /** Format: date */
            issueDate?: string;
            source?: string;
            status?: string;
        };
        BackupLogDto: {
            /** Format: date-time */
            completedAt?: null | string;
            errorMessage?: null | string;
            fileName?: string;
            /** Format: uuid */
            id?: string;
            sha256Hash?: null | string;
            /** Format: int64 */
            sizeBytes?: number | string;
            /** Format: date-time */
            startedAt?: string;
            status?: string;
            /** Format: int32 */
            uploadAttempts?: number | string;
            /** Format: date-time */
            uploadedAt?: null | string;
        };
        BalanceSheetDto: {
            /** Format: date-time */
            asOfDate: string;
            assets: components["schemas"]["FinancialStatementLineDto"][];
            equity: components["schemas"]["FinancialStatementLineDto"][];
            isBalanced: boolean;
            liabilities: components["schemas"]["FinancialStatementLineDto"][];
            /** Format: double */
            totalAssets: number | string;
            /** Format: double */
            totalEquity: number | string;
            /** Format: double */
            totalLiabilities: number | string;
            /** Format: double */
            totalLiabilitiesAndEquity: number | string;
        };
        BusinessSettingsDto: {
            address?: string;
            businessName?: string;
            email?: string;
            fiscalApprovalNote?: null | string;
            /** Format: date-time */
            fiscalApprovedAt?: null | string;
            /** Format: uuid */
            fiscalApprovedByUserId?: null | string;
            fiscalDecisionManifestSha256?: null | string;
            fiscalEvidenceReference?: null | string;
            fiscalProfileStatus?: string;
            /** Format: int32 */
            fiscalProfileVersion?: number | string;
            /** Format: date-time */
            fiscalRetiredAt?: null | string;
            /** Format: uuid */
            fiscalRetiredByUserId?: null | string;
            fiscalRetirementReason?: null | string;
            /** Format: date */
            fiscalValidFrom?: null | string;
            /** Format: date */
            fiscalValidUntil?: null | string;
            footer?: string;
            headerAlign?: string;
            /** Format: double */
            isvRate?: number | string;
            logoBase64?: null | string;
            /** Format: int32 */
            marginLeft?: number | string;
            phone?: string;
            printFontSize?: string;
            /** Format: int32 */
            printLineSpacing?: number | string;
            /** Format: int32 */
            printLogoHeight?: number | string;
            printPrinterName?: null | string;
            /** Format: int32 */
            printWidth?: number | string;
            rtn?: string;
            separatorChar?: string;
            showFiscal?: boolean;
            showFooter?: boolean;
            showGuest?: boolean;
            showHeader?: boolean;
            showItems?: boolean;
            showLogo?: boolean;
            showPayment?: boolean;
            showTotals?: boolean;
            /** Format: double */
            touristTaxRate?: number | string;
        };
        CAIDto: {
            caiNumber?: string;
            currentCorrelative?: string;
            /** Format: date */
            dueDate?: string;
            finalRange?: string;
            /** Format: uuid */
            id?: string;
            initialRange?: string;
            isExpiringSoon?: boolean;
            /** Format: date */
            issueDate?: string;
            status?: string;
        };
        CancelReservationRequest: {
            /** Format: int32 */
            expectedVersion: number | string;
            reason: string;
        };
        CardSettlementApplicationDto: {
            /** Format: double */
            amount?: number | string;
            /** Format: uuid */
            id?: string;
            /** Format: date-time */
            paymentDate?: string;
            /** Format: uuid */
            paymentId?: string;
            paymentNumber?: string;
            paymentReference?: string;
        };
        CardSettlementApplicationRequest: {
            /** Format: double */
            amount: number | string;
            /** Format: uuid */
            paymentId: string;
        };
        CardSettlementDto: {
            /** Format: uuid */
            accountingEntryId?: null | string;
            applications?: components["schemas"]["CardSettlementApplicationDto"][];
            /** Format: double */
            bankDepositAmount?: number | string;
            /** Format: double */
            commissionAmount?: number | string;
            currency?: string;
            externalReference?: string;
            /** Format: double */
            grossAmount?: number | string;
            /** Format: uuid */
            id?: string;
            /** Format: uuid */
            recordedByUserId?: string;
            recordedByUserName?: string;
            /** Format: date */
            settlementDate?: string;
            settlementNumber?: string;
            status?: string;
            /** Format: double */
            withholdingAmount?: number | string;
        };
        CashMovementDto: {
            /** Format: double */
            amount?: number | string;
            /** Format: double */
            balanceAfter?: number | string;
            /** Format: uuid */
            cashRegisterId?: string;
            cashRegisterName?: string;
            /** Format: double */
            countedAmount?: null | number | string;
            description?: null | string;
            /** Format: double */
            difference?: null | number | string;
            /** Format: double */
            expectedAmount?: null | number | string;
            /** Format: uuid */
            id?: string;
            /** Format: date-time */
            movementDate?: string;
            movementType?: string;
            notes?: null | string;
            /** Format: uuid */
            referenceId?: null | string;
            /** Format: uuid */
            userId?: string;
            userName?: string;
        };
        CashRegisterDto: {
            /** Format: double */
            currentBalance?: number | string;
            description?: null | string;
            /** Format: uuid */
            id?: string;
            isActive?: boolean;
            isOpen?: boolean;
            /** Format: date-time */
            lastMovementDate?: null | string;
            name?: string;
        };
        CashRegisterOperationDto: {
            /** Format: double */
            countedAmount?: null | number | string;
            /** Format: double */
            currentBalance?: number | string;
            /** Format: double */
            difference?: null | number | string;
            /** Format: double */
            expectedAmount?: null | number | string;
            message?: string;
            /** Format: uuid */
            movementId?: string;
        };
        CategoryDto: {
            description?: null | string;
            /** Format: uuid */
            id?: string;
            name?: string;
        };
        ChangePasswordRequest: {
            currentPassword: string;
            newPassword: string;
        };
        CheckInRequest: {
            discountIds?: null | string[];
            /** Format: int32 */
            expectedVersion: number | string;
            /** Format: uuid */
            reservationId: string;
            /** Format: uuid */
            roomId: string;
        };
        CheckInResult: {
            /** Format: double */
            discountAmount: number | string;
            /** Format: double */
            discountPercent: number | string;
            /** Format: uuid */
            folioId: string;
            /** Format: double */
            isv: number | string;
            /** Format: double */
            isvRate: number | string;
            message: string;
            /** Format: int32 */
            nights: number | string;
            /** Format: double */
            sellingPricePerNight: number | string;
            /** Format: double */
            subtotal: number | string;
            /** Format: double */
            total: number | string;
            /** Format: double */
            touristTax: number | string;
            /** Format: double */
            touristTaxRate: number | string;
            /** Format: int32 */
            version: number | string;
        };
        CheckOutRequest: {
            /** Format: uuid */
            invoiceId: string;
        };
        CheckOutResult: {
            /** Format: uuid */
            invoiceId: string;
            message: string;
        };
        CloseCashRegisterRequest: {
            /** Format: double */
            countedAmount: number | string;
            notes: null | string;
        };
        ConfirmReservationRequest: {
            /** Format: int32 */
            expectedVersion: number | string;
        };
        CreateAccountRequest: {
            accountName: string;
            accountNumber: string;
            accountType: string;
            /** Format: uuid */
            parentAccountId?: null | string;
        };
        CreateAccountingEntryRequest: {
            description: string;
            entryType: null | string;
            items: components["schemas"]["CreateEntryItemRequest"][];
            /** Format: date */
            transactionDate: string;
        };
        CreateCAIRequest: {
            caiNumber: string;
            /** Format: date */
            dueDate: string;
            finalRange: string;
            initialRange: string;
            /** Format: date */
            issueDate: string;
        };
        CreateCardSettlementRequest: {
            applications: components["schemas"]["CardSettlementApplicationRequest"][];
            /** Format: double */
            bankDepositAmount?: number | string;
            /** Format: double */
            commissionAmount?: number | string;
            currency: string;
            externalReference: string;
            /** Format: double */
            grossAmount?: number | string;
            /** Format: date */
            settlementDate?: string;
            /** Format: double */
            withholdingAmount?: number | string;
        };
        CreateCashRegisterRequest: {
            description: null | string;
            name: string;
        };
        CreateCategoryRequest: {
            description?: null | string;
            name: string;
        };
        CreateCreditNoteRequest: {
            /** Format: uuid */
            caiId: string;
            /** Format: uuid */
            documentAuthorizationId: null | string;
            documentType?: string;
            /** Format: uuid */
            guestId?: null | string;
            items: components["schemas"]["CreditNoteItemRequest"][];
            /** Format: uuid */
            originalInvoiceId: string;
            reason: string;
            taxpayerType?: null | string;
        };
        CreateCustomerRequest: {
            address: null | string;
            email: null | string;
            exonerationOrderNumber?: null | string;
            /** Format: date */
            exonerationValidFrom?: null | string;
            /** Format: date */
            exonerationValidTo?: null | string;
            /** @default false */
            isIsvExempt: boolean;
            /** @default false */
            isTouristTaxExempt: boolean;
            name: string;
            phone: null | string;
            rtn: null | string;
            sagRegistryNumber?: null | string;
            sefinExonerationCertificateNumber?: null | string;
            taxpayerType?: null | string;
        };
        CreateDebitNoteRequest: {
            /** Format: uuid */
            caiId: string;
            /** Format: uuid */
            documentAuthorizationId: null | string;
            documentType?: string;
            /** Format: uuid */
            guestId?: null | string;
            items: components["schemas"]["InvoiceItemDto"][];
            /** Format: uuid */
            originalInvoiceId: string;
            reason: string;
            taxpayerType?: null | string;
        };
        CreateDiscountRequest: {
            applicableTo?: null | string;
            description?: null | string;
            discountType?: string;
            isActive?: boolean;
            /** Format: int32 */
            minAge?: null | number | string;
            name: string;
            /** Format: int32 */
            priority?: number | string;
            requiresDocument?: boolean;
            /** Format: double */
            value?: number | string;
        };
        CreateDocumentAuthorizationRequest: {
            caiNumber: string;
            documentType: string;
            /** Format: date */
            dueDate: string;
            finalRange: string;
            initialRange: string;
            /** Format: date */
            issueDate: string;
        };
        CreateEntryItemRequest: {
            /** Format: uuid */
            accountId: string;
            /** Format: double */
            credit: number | string;
            /** Format: double */
            debit: number | string;
            description: null | string;
        };
        CreateGuestRequest: {
            classification: null | string;
            company: null | string;
            /** Format: date */
            dateOfBirth: null | string;
            documentNumber: null | string;
            documentType: null | string;
            email: null | string;
            exonerationOrderNumber?: null | string;
            /** Format: date */
            exonerationValidFrom?: null | string;
            /** Format: date */
            exonerationValidTo?: null | string;
            firstName: string;
            guestRTN: null | string;
            hasVehicle: boolean;
            /** @default false */
            isIsvExempt: boolean;
            /** @default false */
            isTouristTaxExempt: boolean;
            lastName: string;
            nationality: null | string;
            origin: null | string;
            phone: null | string;
            preferences: null | string;
            sagRegistryNumber?: null | string;
            sefinExonerationCertificateNumber?: null | string;
            taxpayerType?: null | string;
            vehiclePlate: null | string;
        };
        CreateInventoryMovementRequest: {
            movementType: string;
            /** Format: uuid */
            productId: string;
            /** Format: int32 */
            quantity: number | string;
            /** Format: uuid */
            referenceId?: null | string;
            /** Format: int32 */
            targetStock?: null | number | string;
            /** Format: double */
            unitPrice?: null | number | string;
        };
        CreateInvoiceRequest: {
            /** Format: uuid */
            caiId?: string;
            /** Format: double */
            cashReceived?: null | number | string;
            /** Format: uuid */
            cashRegisterId?: null | string;
            customerAddress?: null | string;
            /** Format: uuid */
            customerId?: null | string;
            customerName: string;
            /** Format: uuid */
            discountId?: null | string;
            /** Format: uuid */
            documentAuthorizationId?: null | string;
            documentType?: string;
            exonerationOrderNumber?: null | string;
            /** Format: uuid */
            folioId?: null | string;
            /** Format: uuid */
            guestId?: null | string;
            isIsvExempt?: boolean;
            isTouristTaxExempt?: boolean;
            items?: components["schemas"]["InvoiceItemDto"][];
            /** Format: uuid */
            originalInvoiceId?: null | string;
            paymentMethod?: string;
            paymentReference?: null | string;
            reason?: null | string;
            rtnCliente?: null | string;
            sagRegistryNumber?: null | string;
            sefinExonerationCertificateNumber?: null | string;
            taxpayerType?: null | string;
        };
        CreatePaymentRequest: {
            /** Format: double */
            amount?: number | string;
            applications: components["schemas"]["PaymentApplicationRequest"][];
            /** Format: double */
            cashReceived?: null | number | string;
            /** Format: uuid */
            cashRegisterId?: null | string;
            currency: string;
            externalReference?: null | string;
            method: string;
        };
        CreateProductRequest: {
            /** Format: uuid */
            categoryId: null | string;
            /** Format: int32 */
            currentStock: number | string;
            description: null | string;
            /** @default true */
            isActive: boolean;
            /** Format: int32 */
            minStockLevel: number | string;
            name: string;
            sku: null | string;
            /** Format: double */
            unitPrice: number | string;
        };
        CreatePurchaseInvoiceRequest: {
            caiNumber: null | string;
            /** Format: date-time */
            invoiceDate: string;
            invoiceNumber: string;
            items: components["schemas"]["PurchaseInvoiceItemDto"][];
            notes: null | string;
            /** Format: uuid */
            supplierId: string;
            supplierRTN: null | string;
        };
        CreateRefundRequest: {
            /** Format: double */
            amount?: number | string;
            applications: components["schemas"]["RefundApplicationRequest"][];
            /** Format: uuid */
            cashRegisterId?: null | string;
            externalReference?: null | string;
            reason: string;
        };
        CreateReservationRequest: {
            /** Format: int32 */
            adults: number | string;
            /** Format: double */
            advancePayment: number | string;
            /** Format: date */
            checkInDate: string;
            /** Format: date */
            checkOutDate: string;
            /** Format: int32 */
            children: number | string;
            /** Format: uuid */
            guestId: string;
            notes: null | string;
            paymentMethod: null | string;
            /** Format: uuid */
            roomId: string;
        };
        CreateRoleRequest: {
            description: null | string;
            name: string;
            permissions: null | string[];
        };
        CreateRoomRequest: {
            /** Format: int32 */
            floor: number | string;
            observations: null | string;
            roomNumber: string;
            /** Format: uuid */
            roomTypeId: string;
        };
        CreateRoomTypeRequest: {
            /** Format: int32 */
            capacity: number | string;
            description: null | string;
            name: string;
            /** Format: double */
            pricePerNight: number | string;
        };
        CreateSupplierRequest: {
            address: null | string;
            contactPerson: null | string;
            email: null | string;
            name: string;
            phone: null | string;
            rtn: null | string;
        };
        CreateTaxConfigurationRequest: {
            applicableTo: null | string;
            isActive: boolean;
            name: string;
            /** Format: double */
            rate: number | string;
        };
        CreateUserRequest: {
            email: string;
            firstName: string;
            lastName: string;
            password: string;
            roles: null | string[];
            username: string;
        };
        CreditNoteItemRequest: {
            /** Format: uuid */
            originalInvoiceItemId: string;
            /** Format: int32 */
            quantity: number | string;
        };
        CurrentUserDto: {
            email: null | string;
            fullName: null | string;
            id: null | string;
            mustChangePassword: boolean;
            permissions: string[];
            roles: string[];
            username: null | string;
        };
        CustomerDto: {
            address?: null | string;
            email?: null | string;
            exonerationOrderNumber?: null | string;
            /** Format: date */
            exonerationValidFrom?: null | string;
            /** Format: date */
            exonerationValidTo?: null | string;
            /** Format: uuid */
            id?: string;
            isIsvExempt?: boolean;
            isTouristTaxExempt?: boolean;
            name?: string;
            phone?: null | string;
            rtn?: null | string;
            sagRegistryNumber?: null | string;
            sefinExonerationCertificateNumber?: null | string;
            taxpayerType?: string;
        };
        DashboardAlertDto: {
            description?: string;
            severity?: string;
            title?: string;
        };
        DashboardCashDto: {
            /** Format: double */
            balance?: number | string;
            registerName?: string;
        };
        DashboardInvoiceDto: {
            correlativeNumber?: string;
            customerName?: string;
            /** Format: date-time */
            invoiceDate?: string;
            status?: string;
            /** Format: double */
            totalAmount?: number | string;
        };
        DashboardReservationDto: {
            /** Format: date */
            checkInDate?: string;
            /** Format: date */
            checkOutDate?: string;
            guestName?: string;
            roomNumber?: string;
            status?: string;
        };
        DashboardStatDto: {
            title?: string;
            trend?: null | string;
            value?: string;
        };
        DashboardSummaryDto: {
            alerts?: components["schemas"]["DashboardAlertDto"][];
            cash?: null | components["schemas"]["DashboardCashDto"];
            /** Format: int32 */
            freeRooms?: number | string;
            /** Format: int32 */
            occupiedRooms?: number | string;
            /** Format: int32 */
            pendingCheckIns?: number | string;
            /** Format: int32 */
            pendingCheckOuts?: number | string;
            recentInvoices?: components["schemas"]["DashboardInvoiceDto"][];
            stats?: components["schemas"]["DashboardStatDto"][];
            upcomingReservations?: components["schemas"]["DashboardReservationDto"][];
        };
        DiscountDto: {
            applicableTo?: null | string;
            description?: null | string;
            discountType?: string;
            /** Format: uuid */
            id?: string;
            isActive?: boolean;
            /** Format: int32 */
            minAge?: null | number | string;
            name?: string;
            /** Format: int32 */
            priority?: number | string;
            requiresDocument?: boolean;
            /** Format: double */
            value?: number | string;
        };
        DocumentAuthorizationDto: {
            caiNumber?: string;
            currentCorrelative?: string;
            documentType?: string;
            /** Format: date */
            dueDate?: string;
            finalRange?: string;
            hasAttachment?: boolean;
            /** Format: uuid */
            id?: string;
            initialRange?: string;
            isExpiringSoon?: boolean;
            /** Format: date */
            issueDate?: string;
            status?: string;
        };
        EligibleCardPaymentDto: {
            /** Format: double */
            amount?: number | string;
            /** Format: double */
            availableAmount?: number | string;
            externalReference?: string;
            /** Format: uuid */
            id?: string;
            /** Format: date-time */
            paymentDate?: string;
            paymentNumber?: string;
            /** Format: double */
            refundedAmount?: number | string;
            /** Format: double */
            settledAmount?: number | string;
        };
        EligibleCardSummaryDto: {
            /** Format: double */
            availableAmount: number | string;
            /** Format: int32 */
            totalPayments: number | string;
        };
        EntryItemDto: {
            /** Format: uuid */
            accountId?: string;
            accountName?: string;
            accountNumber?: string;
            /** Format: double */
            credit?: number | string;
            /** Format: double */
            debit?: number | string;
            description?: null | string;
            /** Format: uuid */
            id?: string;
        };
        FinancialStatementLineDto: {
            accountName: string;
            accountNumber: string;
            /** Format: double */
            amount: number | string;
        };
        FiscalProfileRevisionDto: {
            approvalNote: string;
            /** Format: date-time */
            approvedAt: string;
            /** Format: uuid */
            approvedByUserId: string;
            decisionManifestSha256: string;
            evidenceReference: string;
            /** Format: date-time */
            retiredAt: null | string;
            /** Format: uuid */
            retiredByUserId: null | string;
            retirementReason: null | string;
            snapshotSha256: string;
            status: string;
            /** Format: date */
            validFrom: string;
            /** Format: date */
            validUntil: null | string;
            /** Format: int32 */
            version: number | string;
        };
        FolioChargeResult: {
            /** Format: uuid */
            itemId: string;
            message: string;
            /** Format: double */
            totalAmount: number | string;
        };
        FolioDto: {
            /** Format: date-time */
            closingDate?: null | string;
            /** Format: uuid */
            guestId?: string;
            guestName?: string;
            /** Format: uuid */
            id?: string;
            items?: components["schemas"]["FolioItemDto"][];
            /** Format: date-time */
            openingDate?: string;
            /** Format: uuid */
            reservationId?: string;
            /** Format: uuid */
            roomId?: string;
            roomNumber?: string;
            status?: string;
            /** Format: double */
            totalAmount?: number | string;
        };
        FolioItemDto: {
            description?: string;
            /** Format: double */
            discountPercentage?: number | string;
            /** Format: uuid */
            id?: string;
            isExempt?: boolean;
            isTouristTaxable?: boolean;
            /** Format: double */
            isvRate?: number | string;
            /** Format: double */
            lineTotal?: number | string;
            /** Format: int32 */
            quantity?: number | string;
            /** Format: double */
            unitPrice?: number | string;
        };
        FolioListDto: {
            /** Format: date-time */
            closingDate?: null | string;
            guestName?: string;
            /** Format: uuid */
            id?: string;
            /** Format: int32 */
            itemCount?: number | string;
            /** Format: date-time */
            openingDate?: string;
            roomNumber?: string;
            status?: string;
            /** Format: double */
            totalAmount?: number | string;
        };
        GuestClassificationDto: {
            classification: null | string;
        };
        GuestDto: {
            classification?: null | string;
            company?: null | string;
            /** Format: date */
            dateOfBirth?: null | string;
            documentNumber?: null | string;
            documentType?: null | string;
            email?: null | string;
            exonerationOrderNumber?: null | string;
            /** Format: date */
            exonerationValidFrom?: null | string;
            /** Format: date */
            exonerationValidTo?: null | string;
            firstName?: string;
            fullName?: null | string;
            guestRTN?: null | string;
            hasVehicle?: boolean;
            /** Format: uuid */
            id?: string;
            isIsvExempt?: boolean;
            isTouristTaxExempt?: boolean;
            lastName?: string;
            nationality?: null | string;
            origin?: null | string;
            phone?: null | string;
            preferences?: null | string;
            sagRegistryNumber?: null | string;
            sefinExonerationCertificateNumber?: null | string;
            taxpayerType?: string;
            vehiclePlate?: null | string;
        };
        GuestStatsDto: {
            classification?: string;
            isFrequent?: boolean;
            lastVisit?: null | string;
            /** Format: int32 */
            totalVisits?: number | string;
        };
        HealthResponseDto: {
            status: string;
            /** Format: date-time */
            timestamp: string;
        };
        /** Format: binary */
        IFormFile: string;
        IncomeStatementDto: {
            expenses: components["schemas"]["FinancialStatementLineDto"][];
            /** Format: date-time */
            from: string;
            /** Format: double */
            netIncome: number | string;
            revenues: components["schemas"]["FinancialStatementLineDto"][];
            /** Format: date-time */
            to: string;
            /** Format: double */
            totalExpenses: number | string;
            /** Format: double */
            totalRevenues: number | string;
        };
        InventoryMovementDto: {
            /** Format: uuid */
            id?: string;
            /** Format: date-time */
            movementDate?: string;
            movementType?: string;
            /** Format: int32 */
            newStock?: number | string;
            /** Format: uuid */
            productId?: string;
            productName?: string;
            /** Format: int32 */
            quantity?: number | string;
            /** Format: uuid */
            referenceId?: null | string;
            /** Format: double */
            totalValue?: null | number | string;
            /** Format: double */
            unitPrice?: null | number | string;
            userName?: null | string;
        };
        InventorySummaryDto: {
            /** Format: int32 */
            activeProducts: number | string;
            /** Format: int32 */
            lowStockProducts: number | string;
            /** Format: int32 */
            totalMovements: number | string;
            /** Format: int32 */
            totalProducts: number | string;
        };
        InvoiceDto: {
            /** Format: uuid */
            appliedDiscountId?: null | string;
            appliedDiscountNameSnapshot?: null | string;
            /** Format: double */
            appliedDiscountPercentageSnapshot?: null | number | string;
            /** Format: date-time */
            authorizationDueDateSnapshot?: null | string;
            authorizationRangeSnapshot?: null | string;
            /** Format: double */
            balanceDue?: number | string;
            /** Format: uuid */
            caiId?: string;
            caiNumber?: string;
            caiNumberSnapshot?: null | string;
            /** Format: double */
            cashChange?: null | number | string;
            /** Format: double */
            cashReceived?: null | number | string;
            correlativeNumber?: string;
            /** Format: double */
            creditedAmount?: number | string;
            customerAddress?: null | string;
            /** Format: uuid */
            customerId?: null | string;
            customerName?: string;
            /** Format: double */
            discountsAmount?: number | string;
            /** Format: uuid */
            documentAuthorizationId?: null | string;
            documentType?: string;
            /** Format: double */
            exemptAmount?: number | string;
            /** Format: double */
            exoneratedAmount?: number | string;
            exonerationOrderNumber?: null | string;
            /** Format: uuid */
            folioId?: null | string;
            /** Format: uuid */
            guestId?: null | string;
            /** Format: uuid */
            id?: string;
            /** Format: date-time */
            invoiceDate?: string;
            isIsvExempt?: boolean;
            isTouristTaxExempt?: boolean;
            /** Format: double */
            isV15Amount?: number | string;
            /** Format: double */
            isV18Amount?: number | string;
            /** Format: double */
            isvAmount?: number | string;
            items?: components["schemas"]["InvoiceItemDto"][];
            originalCorrelativeNumber?: null | string;
            /** Format: uuid */
            originalInvoiceId?: null | string;
            /** Format: double */
            paidAmount?: number | string;
            paymentMethod?: null | string;
            reason?: null | string;
            rtnCliente?: null | string;
            sagRegistryNumber?: null | string;
            sefinExonerationCertificateNumber?: null | string;
            status?: string;
            /** Format: double */
            subTotal?: number | string;
            /** Format: double */
            taxableAmount?: number | string;
            taxpayerType?: string;
            /** Format: double */
            totalAmount?: number | string;
            /** Format: double */
            touristTaxAmount?: number | string;
        };
        InvoiceItemDto: {
            description: string;
            /** Format: double */
            discountPercentage?: number | string;
            /** Format: uuid */
            folioItemId?: null | string;
            /** Format: uuid */
            id?: string;
            isExempt?: boolean;
            isTouristTaxable?: boolean;
            /** Format: double */
            isvRate?: number | string;
            /** Format: double */
            lineTotal?: number | string;
            /** Format: uuid */
            originalInvoiceItemId?: null | string;
            /** Format: int32 */
            quantity?: number | string;
            /** Format: double */
            unitPrice?: number | string;
        };
        JournalSummaryDto: {
            /** Format: double */
            credit: number | string;
            /** Format: double */
            debit: number | string;
        };
        LoginRequest: {
            password: string;
            username: string;
        };
        MessageResponseDto: {
            message: string;
        };
        OpenCashRegisterRequest: {
            /** Format: double */
            initialAmount: number | string;
        };
        PagedResultOfAccountingEntryListDto: {
            items: components["schemas"]["AccountingEntryListDto"][];
            /** Format: int32 */
            page: number | string;
            /** Format: int32 */
            pageSize: number | string;
            /** Format: int32 */
            totalItems: number | string;
            /** Format: int32 */
            totalPages: number | string;
        };
        PagedResultOfAuditLogDto: {
            items: components["schemas"]["AuditLogDto"][];
            /** Format: int32 */
            page: number | string;
            /** Format: int32 */
            pageSize: number | string;
            /** Format: int32 */
            totalItems: number | string;
            /** Format: int32 */
            totalPages: number | string;
        };
        PagedResultOfAuthorizationOptionDto: {
            items: components["schemas"]["AuthorizationOptionDto"][];
            /** Format: int32 */
            page: number | string;
            /** Format: int32 */
            pageSize: number | string;
            /** Format: int32 */
            totalItems: number | string;
            /** Format: int32 */
            totalPages: number | string;
        };
        PagedResultOfBackupLogDto: {
            items: components["schemas"]["BackupLogDto"][];
            /** Format: int32 */
            page: number | string;
            /** Format: int32 */
            pageSize: number | string;
            /** Format: int32 */
            totalItems: number | string;
            /** Format: int32 */
            totalPages: number | string;
        };
        PagedResultOfCardSettlementDto: {
            items: components["schemas"]["CardSettlementDto"][];
            /** Format: int32 */
            page: number | string;
            /** Format: int32 */
            pageSize: number | string;
            /** Format: int32 */
            totalItems: number | string;
            /** Format: int32 */
            totalPages: number | string;
        };
        PagedResultOfCashMovementDto: {
            items: components["schemas"]["CashMovementDto"][];
            /** Format: int32 */
            page: number | string;
            /** Format: int32 */
            pageSize: number | string;
            /** Format: int32 */
            totalItems: number | string;
            /** Format: int32 */
            totalPages: number | string;
        };
        PagedResultOfCashRegisterDto: {
            items: components["schemas"]["CashRegisterDto"][];
            /** Format: int32 */
            page: number | string;
            /** Format: int32 */
            pageSize: number | string;
            /** Format: int32 */
            totalItems: number | string;
            /** Format: int32 */
            totalPages: number | string;
        };
        PagedResultOfCustomerDto: {
            items: components["schemas"]["CustomerDto"][];
            /** Format: int32 */
            page: number | string;
            /** Format: int32 */
            pageSize: number | string;
            /** Format: int32 */
            totalItems: number | string;
            /** Format: int32 */
            totalPages: number | string;
        };
        PagedResultOfEligibleCardPaymentDto: {
            items: components["schemas"]["EligibleCardPaymentDto"][];
            /** Format: int32 */
            page: number | string;
            /** Format: int32 */
            pageSize: number | string;
            /** Format: int32 */
            totalItems: number | string;
            /** Format: int32 */
            totalPages: number | string;
        };
        PagedResultOfFolioListDto: {
            items: components["schemas"]["FolioListDto"][];
            /** Format: int32 */
            page: number | string;
            /** Format: int32 */
            pageSize: number | string;
            /** Format: int32 */
            totalItems: number | string;
            /** Format: int32 */
            totalPages: number | string;
        };
        PagedResultOfGuestDto: {
            items: components["schemas"]["GuestDto"][];
            /** Format: int32 */
            page: number | string;
            /** Format: int32 */
            pageSize: number | string;
            /** Format: int32 */
            totalItems: number | string;
            /** Format: int32 */
            totalPages: number | string;
        };
        PagedResultOfInventoryMovementDto: {
            items: components["schemas"]["InventoryMovementDto"][];
            /** Format: int32 */
            page: number | string;
            /** Format: int32 */
            pageSize: number | string;
            /** Format: int32 */
            totalItems: number | string;
            /** Format: int32 */
            totalPages: number | string;
        };
        PagedResultOfInvoiceDto: {
            items: components["schemas"]["InvoiceDto"][];
            /** Format: int32 */
            page: number | string;
            /** Format: int32 */
            pageSize: number | string;
            /** Format: int32 */
            totalItems: number | string;
            /** Format: int32 */
            totalPages: number | string;
        };
        PagedResultOfProductDto: {
            items: components["schemas"]["ProductDto"][];
            /** Format: int32 */
            page: number | string;
            /** Format: int32 */
            pageSize: number | string;
            /** Format: int32 */
            totalItems: number | string;
            /** Format: int32 */
            totalPages: number | string;
        };
        PagedResultOfReservationDto: {
            items: components["schemas"]["ReservationDto"][];
            /** Format: int32 */
            page: number | string;
            /** Format: int32 */
            pageSize: number | string;
            /** Format: int32 */
            totalItems: number | string;
            /** Format: int32 */
            totalPages: number | string;
        };
        PagedResultOfRoleOptionDto: {
            items: components["schemas"]["RoleOptionDto"][];
            /** Format: int32 */
            page: number | string;
            /** Format: int32 */
            pageSize: number | string;
            /** Format: int32 */
            totalItems: number | string;
            /** Format: int32 */
            totalPages: number | string;
        };
        PagedResultOfUserDto: {
            items: components["schemas"]["UserDto"][];
            /** Format: int32 */
            page: number | string;
            /** Format: int32 */
            pageSize: number | string;
            /** Format: int32 */
            totalItems: number | string;
            /** Format: int32 */
            totalPages: number | string;
        };
        PaymentApplicationDto: {
            /** Format: double */
            amount?: number | string;
            correlativeNumber?: string;
            /** Format: uuid */
            id?: string;
            /** Format: uuid */
            invoiceId?: string;
        };
        PaymentApplicationRequest: {
            /** Format: double */
            amount: number | string;
            /** Format: uuid */
            invoiceId: string;
        };
        PaymentDto: {
            /** Format: uuid */
            accountingEntryId?: null | string;
            /** Format: double */
            amount?: number | string;
            applications?: components["schemas"]["PaymentApplicationDto"][];
            /** Format: double */
            cardSettledAmount?: number | string;
            /** Format: double */
            cashChange?: null | number | string;
            /** Format: double */
            cashReceived?: null | number | string;
            /** Format: uuid */
            cashRegisterId?: null | string;
            cashRegisterName?: null | string;
            currency?: string;
            externalReference?: null | string;
            /** Format: uuid */
            id?: string;
            method?: string;
            /** Format: date-time */
            paymentDate?: string;
            paymentNumber?: string;
            /** Format: uuid */
            recordedByUserId?: string;
            recordedByUserName?: string;
            /** Format: double */
            refundedAmount?: number | string;
            refunds?: components["schemas"]["RefundDto"][];
            status?: string;
        };
        PermissionDto: {
            description?: null | string;
            /** Format: uuid */
            id?: string;
            name?: string;
        };
        PrintPreviewDto: {
            logoBase64: null | string;
            /** Format: int32 */
            printLogoHeight: number | string;
            /** Format: int32 */
            printWidth: number | string;
            text: string;
        };
        PrintedInvoiceDto: {
            message: string;
            printer: string;
        };
        PrinterConnectionDto: {
            message: string;
            success: boolean;
        };
        ProblemDetails: {
            detail?: null | string;
            instance?: null | string;
            /** Format: int32 */
            status?: null | number | string;
            title?: null | string;
            type?: null | string;
        };
        ProductDto: {
            /** Format: uuid */
            categoryId?: null | string;
            categoryName?: null | string;
            /** Format: int32 */
            currentStock?: number | string;
            description?: null | string;
            /** Format: uuid */
            id?: string;
            isActive?: boolean;
            /** Format: int32 */
            minStockLevel?: number | string;
            name?: string;
            sku?: null | string;
            /** Format: double */
            unitPrice?: number | string;
        };
        PurchaseInvoiceDto: {
            caiNumber?: null | string;
            /** Format: uuid */
            id?: string;
            /** Format: date-time */
            invoiceDate?: string;
            invoiceNumber?: string;
            /** Format: double */
            isV15Amount?: number | string;
            /** Format: double */
            isV18Amount?: number | string;
            /** Format: double */
            isvAmount?: number | string;
            items?: components["schemas"]["PurchaseInvoiceItemDto"][];
            notes?: null | string;
            status?: string;
            /** Format: double */
            subTotal?: number | string;
            /** Format: uuid */
            supplierId?: string;
            supplierName?: string;
            supplierRTN?: null | string;
            /** Format: double */
            totalAmount?: number | string;
        };
        PurchaseInvoiceItemDto: {
            description?: string;
            isExempt?: boolean;
            /** Format: double */
            isvRate?: number | string;
            /** Format: double */
            lineTotal?: number | string;
            /** Format: int32 */
            quantity?: number | string;
            /** Format: double */
            unitPrice?: number | string;
        };
        RefreshTokenRequest: {
            refreshToken?: null | string;
        };
        RefundApplicationDto: {
            /** Format: double */
            amount?: number | string;
            creditNoteCorrelativeNumber?: string;
            /** Format: uuid */
            creditNoteId?: string;
            /** Format: uuid */
            id?: string;
        };
        RefundApplicationRequest: {
            /** Format: double */
            amount: number | string;
            /** Format: uuid */
            creditNoteId: string;
        };
        RefundDto: {
            /** Format: uuid */
            accountingEntryId?: null | string;
            /** Format: double */
            amount?: number | string;
            applications?: components["schemas"]["RefundApplicationDto"][];
            /** Format: uuid */
            cashRegisterId?: null | string;
            cashRegisterName?: null | string;
            currency?: string;
            externalReference?: null | string;
            /** Format: uuid */
            id?: string;
            method?: string;
            /** Format: uuid */
            paymentId?: string;
            paymentNumber?: string;
            reason?: string;
            /** Format: uuid */
            recordedByUserId?: string;
            recordedByUserName?: string;
            /** Format: date-time */
            refundDate?: string;
            refundNumber?: string;
            status?: string;
        };
        RegisterRequest: {
            email: string;
            firstName: string;
            lastName: string;
            password: string;
            username: string;
        };
        ReservationDto: {
            /** Format: int32 */
            adults?: number | string;
            /** Format: double */
            advancePayment?: number | string;
            /** Format: date */
            checkInDate?: string;
            /** Format: date */
            checkOutDate?: string;
            /** Format: int32 */
            children?: number | string;
            /** Format: uuid */
            guestId?: string;
            guestName?: string;
            /** Format: uuid */
            id?: string;
            notes?: null | string;
            paymentMethod?: null | string;
            /** Format: uuid */
            roomId?: string;
            roomNumber?: string;
            status?: string;
            /** Format: int32 */
            version?: number | string;
        };
        ReservationStateResult: {
            message: string;
            /** Format: int32 */
            version: number | string;
        };
        ReservationVersionResult: {
            /** Format: int32 */
            version: number | string;
        };
        RetireFiscalProfileRequest: {
            reason: string;
        };
        RoleDto: {
            description?: null | string;
            /** Format: uuid */
            id?: string;
            isSystem?: boolean;
            name?: string;
            permissions?: string[];
        };
        RoleOptionDto: {
            /** Format: uuid */
            id?: string;
            name?: string;
        };
        RoomDto: {
            /** Format: int32 */
            capacity?: number | string;
            /** Format: int32 */
            floor?: number | string;
            /** Format: uuid */
            id?: string;
            observations?: null | string;
            /** Format: double */
            pricePerNight?: number | string;
            roomNumber?: string;
            /** Format: uuid */
            roomTypeId?: string;
            roomTypeName?: string;
            status?: string;
        };
        RoomTypeDto: {
            /** Format: int32 */
            capacity?: number | string;
            description?: null | string;
            /** Format: uuid */
            id?: string;
            name?: string;
            /** Format: double */
            pricePerNight?: number | string;
        };
        SupplierDto: {
            address?: null | string;
            contactPerson?: null | string;
            email?: null | string;
            /** Format: uuid */
            id?: string;
            isActive?: boolean;
            name?: string;
            phone?: null | string;
            rtn?: null | string;
        };
        TaxConfigurationDto: {
            applicableTo?: null | string;
            /** Format: uuid */
            id?: string;
            isActive?: boolean;
            name?: string;
            /** Format: double */
            rate?: number | string;
        };
        TaxSummaryDto: {
            /** Format: date-time */
            from: string;
            /** Format: double */
            isvCollected15: number | string;
            /** Format: double */
            isvCollected18: number | string;
            /** Format: double */
            isvOnPurchases: number | string;
            /** Format: double */
            netIsvToPay: number | string;
            /** Format: double */
            salesExempt: number | string;
            /** Format: double */
            salesTaxed15: number | string;
            /** Format: double */
            salesTaxed18: number | string;
            /** Format: date-time */
            to: string;
            /** Format: double */
            touristTaxCollected: number | string;
        };
        TrialBalanceItem: {
            /** Format: uuid */
            accountId?: string;
            accountName?: string;
            accountNumber?: string;
            accountType?: string;
            /** Format: double */
            balance?: number | string;
            /** Format: double */
            credit?: number | string;
            /** Format: double */
            debit?: number | string;
            /** Format: double */
            previousBalance?: number | string;
        };
        UpdateAccountRequest: {
            accountName: null | string;
            accountType: null | string;
            isActive: null | boolean;
        };
        UpdateBusinessSettingsRequest: {
            address?: null | string;
            businessName?: null | string;
            email?: null | string;
            /** Format: date */
            fiscalValidFrom?: null | string;
            /** Format: date */
            fiscalValidUntil?: null | string;
            footer?: null | string;
            headerAlign?: null | string;
            /** Format: double */
            isvRate?: null | number | string;
            logoBase64?: null | string;
            /** Format: int32 */
            marginLeft?: null | number | string;
            phone?: null | string;
            printFontSize?: null | string;
            /** Format: int32 */
            printLineSpacing?: null | number | string;
            /** Format: int32 */
            printLogoHeight?: null | number | string;
            printPrinterName?: null | string;
            /** Format: int32 */
            printWidth?: null | number | string;
            rtn?: null | string;
            separatorChar?: null | string;
            showFiscal?: null | boolean;
            showFooter?: null | boolean;
            showGuest?: null | boolean;
            showHeader?: null | boolean;
            showItems?: null | boolean;
            showLogo?: null | boolean;
            showPayment?: null | boolean;
            showTotals?: null | boolean;
            /** Format: double */
            touristTaxRate?: null | number | string;
        };
        UpdateCategoryRequest: {
            description?: null | string;
            name?: null | string;
        };
        UpdateClassificationRequest: {
            classification: string;
        };
        UpdateCustomerRequest: {
            address: null | string;
            email: null | string;
            exonerationOrderNumber?: null | string;
            /** Format: date */
            exonerationValidFrom?: null | string;
            /** Format: date */
            exonerationValidTo?: null | string;
            isIsvExempt?: null | boolean;
            isTouristTaxExempt?: null | boolean;
            name: null | string;
            phone: null | string;
            rtn: null | string;
            sagRegistryNumber?: null | string;
            sefinExonerationCertificateNumber?: null | string;
            taxpayerType?: null | string;
        };
        UpdateDiscountRequest: {
            applicableTo?: null | string;
            description?: null | string;
            discountType?: null | string;
            isActive?: null | boolean;
            /** Format: int32 */
            minAge?: null | number | string;
            name?: null | string;
            /** Format: int32 */
            priority?: null | number | string;
            requiresDocument?: null | boolean;
            /** Format: double */
            value?: null | number | string;
        };
        UpdateGuestRequest: {
            classification: null | string;
            company: null | string;
            /** Format: date */
            dateOfBirth: null | string;
            documentNumber: null | string;
            documentType: null | string;
            email: null | string;
            exonerationOrderNumber?: null | string;
            /** Format: date */
            exonerationValidFrom?: null | string;
            /** Format: date */
            exonerationValidTo?: null | string;
            firstName: null | string;
            guestRTN: null | string;
            hasVehicle: null | boolean;
            isIsvExempt?: null | boolean;
            isTouristTaxExempt?: null | boolean;
            lastName: null | string;
            nationality: null | string;
            origin: null | string;
            phone: null | string;
            preferences: null | string;
            sagRegistryNumber?: null | string;
            sefinExonerationCertificateNumber?: null | string;
            taxpayerType?: null | string;
            vehiclePlate: null | string;
        };
        UpdateProductRequest: {
            /** Format: uuid */
            categoryId?: null | string;
            /** Format: int32 */
            currentStock?: null | number | string;
            description?: null | string;
            isActive?: null | boolean;
            /** Format: int32 */
            minStockLevel?: null | number | string;
            name?: null | string;
            sku?: null | string;
            /** Format: double */
            unitPrice?: null | number | string;
        };
        UpdateReservationRequest: {
            /** Format: int32 */
            adults: null | number | string;
            /** Format: double */
            advancePayment: null | number | string;
            /** Format: date */
            checkInDate: null | string;
            /** Format: date */
            checkOutDate: null | string;
            /** Format: int32 */
            children: null | number | string;
            /** Format: int32 */
            expectedVersion: number | string;
            notes: null | string;
            paymentMethod: null | string;
            /** Format: uuid */
            roomId: null | string;
        };
        UpdateRoleRequest: {
            description: null | string;
            name: null | string;
            permissions: null | string[];
        };
        UpdateRoomRequest: {
            /** Format: int32 */
            floor: null | number | string;
            observations: null | string;
            roomNumber: null | string;
            /** Format: uuid */
            roomTypeId: null | string;
            status: null | string;
        };
        UpdateRoomTypeRequest: {
            /** Format: int32 */
            capacity: null | number | string;
            description: null | string;
            name: null | string;
            /** Format: double */
            pricePerNight: null | number | string;
        };
        UpdateSupplierRequest: {
            address: null | string;
            contactPerson: null | string;
            email: null | string;
            isActive?: null | boolean;
            name: null | string;
            phone: null | string;
            rtn: null | string;
        };
        UpdateUserRequest: {
            email: null | string;
            firstName: null | string;
            isActive: null | boolean;
            lastName: null | string;
            roles: null | string[];
        };
        UserDto: {
            email?: string;
            firstName?: string;
            /** Format: uuid */
            id?: string;
            isActive?: boolean;
            lastName?: string;
            mustChangePassword?: boolean;
            permissions?: string[];
            roles?: string[];
            username?: string;
        };
    };
    responses: never;
    parameters: never;
    requestBodies: never;
    headers: never;
    pathItems: never;
}
export type $defs = Record<string, never>;
export interface operations {
    Accounts_GetAll: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["AccountingAccountDto"][];
                    "text/json": components["schemas"]["AccountingAccountDto"][];
                    "text/plain": components["schemas"]["AccountingAccountDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Accounts_Create: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateAccountRequest"];
                "application/json": components["schemas"]["CreateAccountRequest"];
                "text/json": components["schemas"]["CreateAccountRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["AccountingAccountDto"];
                    "text/json": components["schemas"]["AccountingAccountDto"];
                    "text/plain": components["schemas"]["AccountingAccountDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Accounts_GetAllFlat: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["AccountingAccountDto"][];
                    "text/json": components["schemas"]["AccountingAccountDto"][];
                    "text/plain": components["schemas"]["AccountingAccountDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Accounts_GetJournalEntries: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["AccountingEntryDto"][];
                    "text/json": components["schemas"]["AccountingEntryDto"][];
                    "text/plain": components["schemas"]["AccountingEntryDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Accounts_CreateJournalEntry: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateAccountingEntryRequest"];
                "application/json": components["schemas"]["CreateAccountingEntryRequest"];
                "text/json": components["schemas"]["CreateAccountingEntryRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["AccountingEntryDto"];
                    "text/json": components["schemas"]["AccountingEntryDto"];
                    "text/plain": components["schemas"]["AccountingEntryDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Accounts_GetJournalPage: {
        parameters: {
            query?: {
                Search?: string;
                Page?: number | string;
                PageSize?: number | string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PagedResultOfAccountingEntryListDto"];
                    "text/json": components["schemas"]["PagedResultOfAccountingEntryListDto"];
                    "text/plain": components["schemas"]["PagedResultOfAccountingEntryListDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Accounts_GetJournalSummary: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["JournalSummaryDto"];
                    "text/json": components["schemas"]["JournalSummaryDto"];
                    "text/plain": components["schemas"]["JournalSummaryDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Accounts_GetJournalEntry: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["AccountingEntryDto"];
                    "text/json": components["schemas"]["AccountingEntryDto"];
                    "text/plain": components["schemas"]["AccountingEntryDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Accounts_GetTrialBalance: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["TrialBalanceItem"][];
                    "text/json": components["schemas"]["TrialBalanceItem"][];
                    "text/plain": components["schemas"]["TrialBalanceItem"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Accounts_GetById: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["AccountingAccountDto"];
                    "text/json": components["schemas"]["AccountingAccountDto"];
                    "text/plain": components["schemas"]["AccountingAccountDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Accounts_Update: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["UpdateAccountRequest"];
                "application/json": components["schemas"]["UpdateAccountRequest"];
                "text/json": components["schemas"]["UpdateAccountRequest"];
            };
        };
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Accounts_Deactivate: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["MessageResponseDto"];
                    "text/json": components["schemas"]["MessageResponseDto"];
                    "text/plain": components["schemas"]["MessageResponseDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Accounts_GetMovements: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["AccountMovementDto"][];
                    "text/json": components["schemas"]["AccountMovementDto"][];
                    "text/plain": components["schemas"]["AccountMovementDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Auth_ChangePassword: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["ChangePasswordRequest"];
                "application/json": components["schemas"]["ChangePasswordRequest"];
                "text/json": components["schemas"]["ChangePasswordRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["MessageResponseDto"];
                    "text/json": components["schemas"]["MessageResponseDto"];
                    "text/plain": components["schemas"]["MessageResponseDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Auth_Login: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["LoginRequest"];
                "application/json": components["schemas"]["LoginRequest"];
                "text/json": components["schemas"]["LoginRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["AuthResponse"];
                    "text/json": components["schemas"]["AuthResponse"];
                    "text/plain": components["schemas"]["AuthResponse"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Auth_Logout: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["MessageResponseDto"];
                    "text/json": components["schemas"]["MessageResponseDto"];
                    "text/plain": components["schemas"]["MessageResponseDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Auth_GetCurrentUser: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CurrentUserDto"];
                    "text/json": components["schemas"]["CurrentUserDto"];
                    "text/plain": components["schemas"]["CurrentUserDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Auth_Refresh: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["RefreshTokenRequest"];
                "application/json": components["schemas"]["RefreshTokenRequest"];
                "text/json": components["schemas"]["RefreshTokenRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["AuthResponse"];
                    "text/json": components["schemas"]["AuthResponse"];
                    "text/plain": components["schemas"]["AuthResponse"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Auth_Register: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["RegisterRequest"];
                "application/json": components["schemas"]["RegisterRequest"];
                "text/json": components["schemas"]["RegisterRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["AuthResponse"];
                    "text/json": components["schemas"]["AuthResponse"];
                    "text/plain": components["schemas"]["AuthResponse"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Backup_GetLogs: {
        parameters: {
            query?: {
                limit?: number | string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["BackupLogDto"][];
                    "text/json": components["schemas"]["BackupLogDto"][];
                    "text/plain": components["schemas"]["BackupLogDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Backup_GetLogsPage: {
        parameters: {
            query?: {
                Status?: string;
                Search?: string;
                Page?: number | string;
                PageSize?: number | string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PagedResultOfBackupLogDto"];
                    "text/json": components["schemas"]["PagedResultOfBackupLogDto"];
                    "text/plain": components["schemas"]["PagedResultOfBackupLogDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Backup_CreateManualBackup: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description Archivo descargable. */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/octet-stream": string;
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    CAI_GetAll: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CAIDto"][];
                    "text/json": components["schemas"]["CAIDto"][];
                    "text/plain": components["schemas"]["CAIDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    CAI_Create: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateCAIRequest"];
                "application/json": components["schemas"]["CreateCAIRequest"];
                "text/json": components["schemas"]["CreateCAIRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CAIDto"];
                    "text/json": components["schemas"]["CAIDto"];
                    "text/plain": components["schemas"]["CAIDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    CAI_GetActive: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CAIDto"];
                    "text/json": components["schemas"]["CAIDto"];
                    "text/plain": components["schemas"]["CAIDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    CAI_GetById: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CAIDto"];
                    "text/json": components["schemas"]["CAIDto"];
                    "text/plain": components["schemas"]["CAIDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    CAI_Delete: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Customers_GetAll: {
        parameters: {
            query?: {
                search?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CustomerDto"][];
                    "text/json": components["schemas"]["CustomerDto"][];
                    "text/plain": components["schemas"]["CustomerDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Customers_Create: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateCustomerRequest"];
                "application/json": components["schemas"]["CreateCustomerRequest"];
                "text/json": components["schemas"]["CreateCustomerRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CustomerDto"];
                    "text/json": components["schemas"]["CustomerDto"];
                    "text/plain": components["schemas"]["CustomerDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Customers_GetPage: {
        parameters: {
            query?: {
                Search?: string;
                Page?: number | string;
                PageSize?: number | string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PagedResultOfCustomerDto"];
                    "text/json": components["schemas"]["PagedResultOfCustomerDto"];
                    "text/plain": components["schemas"]["PagedResultOfCustomerDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Customers_GetById: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CustomerDto"];
                    "text/json": components["schemas"]["CustomerDto"];
                    "text/plain": components["schemas"]["CustomerDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Customers_Update: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["UpdateCustomerRequest"];
                "application/json": components["schemas"]["UpdateCustomerRequest"];
                "text/json": components["schemas"]["UpdateCustomerRequest"];
            };
        };
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Customers_Delete: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Discounts_GetAll: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["DiscountDto"][];
                    "text/json": components["schemas"]["DiscountDto"][];
                    "text/plain": components["schemas"]["DiscountDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Discounts_Create: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateDiscountRequest"];
                "application/json": components["schemas"]["CreateDiscountRequest"];
                "text/json": components["schemas"]["CreateDiscountRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["DiscountDto"];
                    "text/json": components["schemas"]["DiscountDto"];
                    "text/plain": components["schemas"]["DiscountDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Discounts_GetById: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["DiscountDto"];
                    "text/json": components["schemas"]["DiscountDto"];
                    "text/plain": components["schemas"]["DiscountDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Discounts_Update: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["UpdateDiscountRequest"];
                "application/json": components["schemas"]["UpdateDiscountRequest"];
                "text/json": components["schemas"]["UpdateDiscountRequest"];
            };
        };
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Discounts_Delete: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Folios_GetAll: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["FolioDto"][];
                    "text/json": components["schemas"]["FolioDto"][];
                    "text/plain": components["schemas"]["FolioDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Folios_GetByReservation: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                reservationId: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["FolioDto"];
                    "text/json": components["schemas"]["FolioDto"];
                    "text/plain": components["schemas"]["FolioDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Folios_GetPage: {
        parameters: {
            query?: {
                Status?: string;
                Search?: string;
                Page?: number | string;
                PageSize?: number | string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PagedResultOfFolioListDto"];
                    "text/json": components["schemas"]["PagedResultOfFolioListDto"];
                    "text/plain": components["schemas"]["PagedResultOfFolioListDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Folios_AddItem: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                folioId: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["AddFolioItemRequest"];
                "application/json": components["schemas"]["AddFolioItemRequest"];
                "text/json": components["schemas"]["AddFolioItemRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["FolioChargeResult"];
                    "text/json": components["schemas"]["FolioChargeResult"];
                    "text/plain": components["schemas"]["FolioChargeResult"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Conflict */
            409: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Folios_GetById: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["FolioDto"];
                    "text/json": components["schemas"]["FolioDto"];
                    "text/plain": components["schemas"]["FolioDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Guests_GetAll: {
        parameters: {
            query?: {
                search?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["GuestDto"][];
                    "text/json": components["schemas"]["GuestDto"][];
                    "text/plain": components["schemas"]["GuestDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Guests_Create: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateGuestRequest"];
                "application/json": components["schemas"]["CreateGuestRequest"];
                "text/json": components["schemas"]["CreateGuestRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["GuestDto"];
                    "text/json": components["schemas"]["GuestDto"];
                    "text/plain": components["schemas"]["GuestDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Guests_GetPage: {
        parameters: {
            query?: {
                Search?: string;
                Page?: number | string;
                PageSize?: number | string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PagedResultOfGuestDto"];
                    "text/json": components["schemas"]["PagedResultOfGuestDto"];
                    "text/plain": components["schemas"]["PagedResultOfGuestDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Guests_GetById: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["GuestDto"];
                    "text/json": components["schemas"]["GuestDto"];
                    "text/plain": components["schemas"]["GuestDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Guests_Update: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["UpdateGuestRequest"];
                "application/json": components["schemas"]["UpdateGuestRequest"];
                "text/json": components["schemas"]["UpdateGuestRequest"];
            };
        };
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Guests_Delete: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Guests_UpdateClassification: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["UpdateClassificationRequest"];
                "application/json": components["schemas"]["UpdateClassificationRequest"];
                "text/json": components["schemas"]["UpdateClassificationRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["GuestClassificationDto"];
                    "text/json": components["schemas"]["GuestClassificationDto"];
                    "text/plain": components["schemas"]["GuestClassificationDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Guests_GetStats: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["GuestStatsDto"];
                    "text/json": components["schemas"]["GuestStatsDto"];
                    "text/plain": components["schemas"]["GuestStatsDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Inventory_GetCategories: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CategoryDto"][];
                    "text/json": components["schemas"]["CategoryDto"][];
                    "text/plain": components["schemas"]["CategoryDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Inventory_CreateCategory: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateCategoryRequest"];
                "application/json": components["schemas"]["CreateCategoryRequest"];
                "text/json": components["schemas"]["CreateCategoryRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CategoryDto"];
                    "text/json": components["schemas"]["CategoryDto"];
                    "text/plain": components["schemas"]["CategoryDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Inventory_UpdateCategory: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["UpdateCategoryRequest"];
                "application/json": components["schemas"]["UpdateCategoryRequest"];
                "text/json": components["schemas"]["UpdateCategoryRequest"];
            };
        };
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Inventory_DeleteCategory: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Inventory_GetMovements: {
        parameters: {
            query?: {
                productId?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["InventoryMovementDto"][];
                    "text/json": components["schemas"]["InventoryMovementDto"][];
                    "text/plain": components["schemas"]["InventoryMovementDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Inventory_CreateMovement: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateInventoryMovementRequest"];
                "application/json": components["schemas"]["CreateInventoryMovementRequest"];
                "text/json": components["schemas"]["CreateInventoryMovementRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["InventoryMovementDto"];
                    "text/json": components["schemas"]["InventoryMovementDto"];
                    "text/plain": components["schemas"]["InventoryMovementDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Inventory_GetMovementsPage: {
        parameters: {
            query?: {
                ProductId?: string;
                MovementType?: string;
                Search?: string;
                Page?: number | string;
                PageSize?: number | string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PagedResultOfInventoryMovementDto"];
                    "text/json": components["schemas"]["PagedResultOfInventoryMovementDto"];
                    "text/plain": components["schemas"]["PagedResultOfInventoryMovementDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Inventory_GetProducts: {
        parameters: {
            query?: {
                search?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["ProductDto"][];
                    "text/json": components["schemas"]["ProductDto"][];
                    "text/plain": components["schemas"]["ProductDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Inventory_CreateProduct: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateProductRequest"];
                "application/json": components["schemas"]["CreateProductRequest"];
                "text/json": components["schemas"]["CreateProductRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["ProductDto"];
                    "text/json": components["schemas"]["ProductDto"];
                    "text/plain": components["schemas"]["ProductDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Inventory_GetProductsPage: {
        parameters: {
            query?: {
                CategoryId?: string;
                IsActive?: boolean;
                Search?: string;
                Page?: number | string;
                PageSize?: number | string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PagedResultOfProductDto"];
                    "text/json": components["schemas"]["PagedResultOfProductDto"];
                    "text/plain": components["schemas"]["PagedResultOfProductDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Inventory_UpdateProduct: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["UpdateProductRequest"];
                "application/json": components["schemas"]["UpdateProductRequest"];
                "text/json": components["schemas"]["UpdateProductRequest"];
            };
        };
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Inventory_DeleteProduct: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Inventory_GetSummary: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["InventorySummaryDto"];
                    "text/json": components["schemas"]["InventorySummaryDto"];
                    "text/plain": components["schemas"]["InventorySummaryDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Invoices_GetAll: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["InvoiceDto"][];
                    "text/json": components["schemas"]["InvoiceDto"][];
                    "text/plain": components["schemas"]["InvoiceDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Invoices_Create: {
        parameters: {
            query?: never;
            header?: {
                "Idempotency-Key"?: string;
            };
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateInvoiceRequest"];
                "application/json": components["schemas"]["CreateInvoiceRequest"];
                "text/json": components["schemas"]["CreateInvoiceRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["InvoiceDto"];
                    "text/json": components["schemas"]["InvoiceDto"];
                    "text/plain": components["schemas"]["InvoiceDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Invoices_GetByDateRange: {
        parameters: {
            query?: {
                start?: string;
                end?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["InvoiceDto"][];
                    "text/json": components["schemas"]["InvoiceDto"][];
                    "text/plain": components["schemas"]["InvoiceDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Invoices_GetPage: {
        parameters: {
            query?: {
                Dni?: string;
                Start?: string;
                End?: string;
                CaiId?: string;
                DocumentAuthorizationId?: string;
                OriginalInvoiceId?: string;
                DocumentType?: string;
                Page?: number | string;
                PageSize?: number | string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PagedResultOfInvoiceDto"];
                    "text/json": components["schemas"]["PagedResultOfInvoiceDto"];
                    "text/plain": components["schemas"]["PagedResultOfInvoiceDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Invoices_Search: {
        parameters: {
            query?: {
                dni?: string;
                start?: string;
                end?: string;
                caiId?: string;
                documentAuthorizationId?: string;
                originalInvoiceId?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["InvoiceDto"][];
                    "text/json": components["schemas"]["InvoiceDto"][];
                    "text/plain": components["schemas"]["InvoiceDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Invoices_GetById: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["InvoiceDto"];
                    "text/json": components["schemas"]["InvoiceDto"];
                    "text/plain": components["schemas"]["InvoiceDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Invoices_Update: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateInvoiceRequest"];
                "application/json": components["schemas"]["CreateInvoiceRequest"];
                "text/json": components["schemas"]["CreateInvoiceRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["InvoiceDto"];
                    "text/json": components["schemas"]["InvoiceDto"];
                    "text/plain": components["schemas"]["InvoiceDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Invoices_Cancel: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Invoices_CreateCreditNote: {
        parameters: {
            query?: never;
            header?: {
                "Idempotency-Key"?: string;
            };
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateCreditNoteRequest"];
                "application/json": components["schemas"]["CreateCreditNoteRequest"];
                "text/json": components["schemas"]["CreateCreditNoteRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["InvoiceDto"];
                    "text/json": components["schemas"]["InvoiceDto"];
                    "text/plain": components["schemas"]["InvoiceDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Invoices_CreateDebitNote: {
        parameters: {
            query?: never;
            header?: {
                "Idempotency-Key"?: string;
            };
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateDebitNoteRequest"];
                "application/json": components["schemas"]["CreateDebitNoteRequest"];
                "text/json": components["schemas"]["CreateDebitNoteRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["InvoiceDto"];
                    "text/json": components["schemas"]["InvoiceDto"];
                    "text/plain": components["schemas"]["InvoiceDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Print_PrintInvoice: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PrintedInvoiceDto"];
                    "text/json": components["schemas"]["PrintedInvoiceDto"];
                    "text/plain": components["schemas"]["PrintedInvoiceDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Print_GetPreview: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PrintPreviewDto"];
                    "text/json": components["schemas"]["PrintPreviewDto"];
                    "text/plain": components["schemas"]["PrintPreviewDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Print_GetPrinters: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": string[];
                    "text/json": string[];
                    "text/plain": string[];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Print_TestPrinter: {
        parameters: {
            query?: {
                name?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PrinterConnectionDto"];
                    "text/json": components["schemas"]["PrinterConnectionDto"];
                    "text/plain": components["schemas"]["PrinterConnectionDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Print_GetTestPreview: {
        parameters: {
            query?: {
                width?: number | string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PrintPreviewDto"];
                    "text/json": components["schemas"]["PrintPreviewDto"];
                    "text/plain": components["schemas"]["PrintPreviewDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Print_PrintTest: {
        parameters: {
            query?: {
                width?: number | string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["MessageResponseDto"];
                    "text/json": components["schemas"]["MessageResponseDto"];
                    "text/plain": components["schemas"]["MessageResponseDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Print_GetTestRuler: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["MessageResponseDto"];
                    "text/json": components["schemas"]["MessageResponseDto"];
                    "text/plain": components["schemas"]["MessageResponseDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Reservations_GetAll: {
        parameters: {
            query?: {
                status?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["ReservationDto"][];
                    "text/json": components["schemas"]["ReservationDto"][];
                    "text/plain": components["schemas"]["ReservationDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Reservations_Create: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateReservationRequest"];
                "application/json": components["schemas"]["CreateReservationRequest"];
                "text/json": components["schemas"]["CreateReservationRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["ReservationDto"];
                    "text/json": components["schemas"]["ReservationDto"];
                    "text/plain": components["schemas"]["ReservationDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Conflict */
            409: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Reservations_GetCalendar: {
        parameters: {
            query?: {
                start?: string;
                end?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["ReservationDto"][];
                    "text/json": components["schemas"]["ReservationDto"][];
                    "text/plain": components["schemas"]["ReservationDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Reservations_CheckIn: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CheckInRequest"];
                "application/json": components["schemas"]["CheckInRequest"];
                "text/json": components["schemas"]["CheckInRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CheckInResult"];
                    "text/json": components["schemas"]["CheckInResult"];
                    "text/plain": components["schemas"]["CheckInResult"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Conflict */
            409: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Reservations_CheckOut: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CheckOutRequest"];
                "application/json": components["schemas"]["CheckOutRequest"];
                "text/json": components["schemas"]["CheckOutRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CheckOutResult"];
                    "text/json": components["schemas"]["CheckOutResult"];
                    "text/plain": components["schemas"]["CheckOutResult"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Conflict */
            409: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Reservations_GetByDateRange: {
        parameters: {
            query?: {
                start?: string;
                end?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["ReservationDto"][];
                    "text/json": components["schemas"]["ReservationDto"][];
                    "text/plain": components["schemas"]["ReservationDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Reservations_GetPage: {
        parameters: {
            query?: {
                Status?: string;
                Search?: string;
                Page?: number | string;
                PageSize?: number | string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PagedResultOfReservationDto"];
                    "text/json": components["schemas"]["PagedResultOfReservationDto"];
                    "text/plain": components["schemas"]["PagedResultOfReservationDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Reservations_GetById: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["ReservationDto"];
                    "text/json": components["schemas"]["ReservationDto"];
                    "text/plain": components["schemas"]["ReservationDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Reservations_Update: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["UpdateReservationRequest"];
                "application/json": components["schemas"]["UpdateReservationRequest"];
                "text/json": components["schemas"]["UpdateReservationRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["ReservationVersionResult"];
                    "text/json": components["schemas"]["ReservationVersionResult"];
                    "text/plain": components["schemas"]["ReservationVersionResult"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Conflict */
            409: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Reservations_Delete: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Reservations_Cancel: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CancelReservationRequest"];
                "application/json": components["schemas"]["CancelReservationRequest"];
                "text/json": components["schemas"]["CancelReservationRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["ReservationStateResult"];
                    "text/json": components["schemas"]["ReservationStateResult"];
                    "text/plain": components["schemas"]["ReservationStateResult"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Conflict */
            409: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Reservations_Confirm: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["ConfirmReservationRequest"];
                "application/json": components["schemas"]["ConfirmReservationRequest"];
                "text/json": components["schemas"]["ConfirmReservationRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["ReservationStateResult"];
                    "text/json": components["schemas"]["ReservationStateResult"];
                    "text/plain": components["schemas"]["ReservationStateResult"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Conflict */
            409: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Roles_GetAll: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["RoleDto"][];
                    "text/json": components["schemas"]["RoleDto"][];
                    "text/plain": components["schemas"]["RoleDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Roles_Create: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateRoleRequest"];
                "application/json": components["schemas"]["CreateRoleRequest"];
                "text/json": components["schemas"]["CreateRoleRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["RoleDto"];
                    "text/json": components["schemas"]["RoleDto"];
                    "text/plain": components["schemas"]["RoleDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Roles_GetPermissions: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PermissionDto"][];
                    "text/json": components["schemas"]["PermissionDto"][];
                    "text/plain": components["schemas"]["PermissionDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Roles_GetById: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["RoleDto"];
                    "text/json": components["schemas"]["RoleDto"];
                    "text/plain": components["schemas"]["RoleDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Roles_Update: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["UpdateRoleRequest"];
                "application/json": components["schemas"]["UpdateRoleRequest"];
                "text/json": components["schemas"]["UpdateRoleRequest"];
            };
        };
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Roles_Delete: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Rooms_GetAll: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["RoomDto"][];
                    "text/json": components["schemas"]["RoomDto"][];
                    "text/plain": components["schemas"]["RoomDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Rooms_Create: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateRoomRequest"];
                "application/json": components["schemas"]["CreateRoomRequest"];
                "text/json": components["schemas"]["CreateRoomRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["RoomDto"];
                    "text/json": components["schemas"]["RoomDto"];
                    "text/plain": components["schemas"]["RoomDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Rooms_GetAvailable: {
        parameters: {
            query?: {
                checkIn?: string;
                checkOut?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["RoomDto"][];
                    "text/json": components["schemas"]["RoomDto"][];
                    "text/plain": components["schemas"]["RoomDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Rooms_GetByStatus: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                status: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["RoomDto"][];
                    "text/json": components["schemas"]["RoomDto"][];
                    "text/plain": components["schemas"]["RoomDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Rooms_GetById: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["RoomDto"];
                    "text/json": components["schemas"]["RoomDto"];
                    "text/plain": components["schemas"]["RoomDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Rooms_Update: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["UpdateRoomRequest"];
                "application/json": components["schemas"]["UpdateRoomRequest"];
                "text/json": components["schemas"]["UpdateRoomRequest"];
            };
        };
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Rooms_Delete: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Suppliers_GetAll: {
        parameters: {
            query?: {
                search?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["SupplierDto"][];
                    "text/json": components["schemas"]["SupplierDto"][];
                    "text/plain": components["schemas"]["SupplierDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Suppliers_Create: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateSupplierRequest"];
                "application/json": components["schemas"]["CreateSupplierRequest"];
                "text/json": components["schemas"]["CreateSupplierRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["SupplierDto"];
                    "text/json": components["schemas"]["SupplierDto"];
                    "text/plain": components["schemas"]["SupplierDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Suppliers_GetById: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["SupplierDto"];
                    "text/json": components["schemas"]["SupplierDto"];
                    "text/plain": components["schemas"]["SupplierDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Suppliers_Update: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["UpdateSupplierRequest"];
                "application/json": components["schemas"]["UpdateSupplierRequest"];
                "text/json": components["schemas"]["UpdateSupplierRequest"];
            };
        };
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Suppliers_Delete: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Users_GetAll: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["UserDto"][];
                    "text/json": components["schemas"]["UserDto"][];
                    "text/plain": components["schemas"]["UserDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Users_Create: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateUserRequest"];
                "application/json": components["schemas"]["CreateUserRequest"];
                "text/json": components["schemas"]["CreateUserRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["UserDto"];
                    "text/json": components["schemas"]["UserDto"];
                    "text/plain": components["schemas"]["UserDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Users_GetPage: {
        parameters: {
            query?: {
                RoleId?: string;
                Search?: string;
                Page?: number | string;
                PageSize?: number | string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PagedResultOfUserDto"];
                    "text/json": components["schemas"]["PagedResultOfUserDto"];
                    "text/plain": components["schemas"]["PagedResultOfUserDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Users_GetRoleOptions: {
        parameters: {
            query?: {
                AssignableOnly?: boolean;
                Search?: string;
                Page?: number | string;
                PageSize?: number | string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PagedResultOfRoleOptionDto"];
                    "text/json": components["schemas"]["PagedResultOfRoleOptionDto"];
                    "text/plain": components["schemas"]["PagedResultOfRoleOptionDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Users_GetById: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["UserDto"];
                    "text/json": components["schemas"]["UserDto"];
                    "text/plain": components["schemas"]["UserDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Users_Update: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["UpdateUserRequest"];
                "application/json": components["schemas"]["UpdateUserRequest"];
                "text/json": components["schemas"]["UpdateUserRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["UserDto"];
                    "text/json": components["schemas"]["UserDto"];
                    "text/plain": components["schemas"]["UserDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Users_Delete: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["MessageResponseDto"];
                    "text/json": components["schemas"]["MessageResponseDto"];
                    "text/plain": components["schemas"]["MessageResponseDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Users_ResetPassword: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["AdminResetPasswordRequest"];
                "application/json": components["schemas"]["AdminResetPasswordRequest"];
                "text/json": components["schemas"]["AdminResetPasswordRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["MessageResponseDto"];
                    "text/json": components["schemas"]["MessageResponseDto"];
                    "text/plain": components["schemas"]["MessageResponseDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Users_AssignRole: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                userId: string;
                roleId: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["MessageResponseDto"];
                    "text/json": components["schemas"]["MessageResponseDto"];
                    "text/plain": components["schemas"]["MessageResponseDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    AuditLogs_GetLogs: {
        parameters: {
            query?: {
                action?: string;
                entityName?: string;
                startDate?: string;
                endDate?: string;
                search?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["AuditLogDto"][];
                    "text/json": components["schemas"]["AuditLogDto"][];
                    "text/plain": components["schemas"]["AuditLogDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    AuditLogs_GetActions: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": string[];
                    "text/json": string[];
                    "text/plain": string[];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    AuditLogs_ExportIntegrity: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["AuditChainExport"];
                    "text/json": components["schemas"]["AuditChainExport"];
                    "text/plain": components["schemas"]["AuditChainExport"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    AuditLogs_GetPage: {
        parameters: {
            query?: {
                Search?: string;
                Action?: string;
                Page?: number | string;
                PageSize?: number | string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PagedResultOfAuditLogDto"];
                    "text/json": components["schemas"]["PagedResultOfAuditLogDto"];
                    "text/plain": components["schemas"]["PagedResultOfAuditLogDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    AuditLogs_VerifyIntegrity: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["AuditIntegrityResult"];
                    "text/json": components["schemas"]["AuditIntegrityResult"];
                    "text/plain": components["schemas"]["AuditIntegrityResult"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    CardSettlements_GetAll: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CardSettlementDto"][];
                    "text/json": components["schemas"]["CardSettlementDto"][];
                    "text/plain": components["schemas"]["CardSettlementDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    CardSettlements_Create: {
        parameters: {
            query?: never;
            header?: {
                "Idempotency-Key"?: string;
            };
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateCardSettlementRequest"];
                "application/json": components["schemas"]["CreateCardSettlementRequest"];
                "text/json": components["schemas"]["CreateCardSettlementRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CardSettlementDto"];
                    "text/json": components["schemas"]["CardSettlementDto"];
                    "text/plain": components["schemas"]["CardSettlementDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Conflict */
            409: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    CardSettlements_GetEligiblePayments: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["EligibleCardPaymentDto"][];
                    "text/json": components["schemas"]["EligibleCardPaymentDto"][];
                    "text/plain": components["schemas"]["EligibleCardPaymentDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    CardSettlements_GetEligiblePage: {
        parameters: {
            query?: {
                Search?: string;
                Page?: number | string;
                PageSize?: number | string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PagedResultOfEligibleCardPaymentDto"];
                    "text/json": components["schemas"]["PagedResultOfEligibleCardPaymentDto"];
                    "text/plain": components["schemas"]["PagedResultOfEligibleCardPaymentDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    CardSettlements_GetEligibleSummary: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["EligibleCardSummaryDto"];
                    "text/json": components["schemas"]["EligibleCardSummaryDto"];
                    "text/plain": components["schemas"]["EligibleCardSummaryDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    CardSettlements_GetPage: {
        parameters: {
            query?: {
                Search?: string;
                Page?: number | string;
                PageSize?: number | string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PagedResultOfCardSettlementDto"];
                    "text/json": components["schemas"]["PagedResultOfCardSettlementDto"];
                    "text/plain": components["schemas"]["PagedResultOfCardSettlementDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    CardSettlements_GetById: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CardSettlementDto"];
                    "text/json": components["schemas"]["CardSettlementDto"];
                    "text/plain": components["schemas"]["CardSettlementDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    CashRegisters_GetAll: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CashRegisterDto"][];
                    "text/json": components["schemas"]["CashRegisterDto"][];
                    "text/plain": components["schemas"]["CashRegisterDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    CashRegisters_Create: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateCashRegisterRequest"];
                "application/json": components["schemas"]["CreateCashRegisterRequest"];
                "text/json": components["schemas"]["CreateCashRegisterRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CashRegisterDto"];
                    "text/json": components["schemas"]["CashRegisterDto"];
                    "text/plain": components["schemas"]["CashRegisterDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    CashRegisters_GetPage: {
        parameters: {
            query?: {
                OpenOnly?: boolean;
                Search?: string;
                Page?: number | string;
                PageSize?: number | string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PagedResultOfCashRegisterDto"];
                    "text/json": components["schemas"]["PagedResultOfCashRegisterDto"];
                    "text/plain": components["schemas"]["PagedResultOfCashRegisterDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    CashRegisters_GetById: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CashRegisterDto"];
                    "text/json": components["schemas"]["CashRegisterDto"];
                    "text/plain": components["schemas"]["CashRegisterDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    CashRegisters_Close: {
        parameters: {
            query?: never;
            header?: {
                "Idempotency-Key"?: string;
            };
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CloseCashRegisterRequest"];
                "application/json": components["schemas"]["CloseCashRegisterRequest"];
                "text/json": components["schemas"]["CloseCashRegisterRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CashRegisterOperationDto"];
                    "text/json": components["schemas"]["CashRegisterOperationDto"];
                    "text/plain": components["schemas"]["CashRegisterOperationDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Conflict */
            409: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    CashRegisters_GetMovements: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CashMovementDto"][];
                    "text/json": components["schemas"]["CashMovementDto"][];
                    "text/plain": components["schemas"]["CashMovementDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    CashRegisters_GetMovementsPage: {
        parameters: {
            query?: {
                MovementType?: string;
                Search?: string;
                Page?: number | string;
                PageSize?: number | string;
            };
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PagedResultOfCashMovementDto"];
                    "text/json": components["schemas"]["PagedResultOfCashMovementDto"];
                    "text/plain": components["schemas"]["PagedResultOfCashMovementDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    CashRegisters_Open: {
        parameters: {
            query?: never;
            header?: {
                "Idempotency-Key"?: string;
            };
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["OpenCashRegisterRequest"];
                "application/json": components["schemas"]["OpenCashRegisterRequest"];
                "text/json": components["schemas"]["OpenCashRegisterRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CashRegisterOperationDto"];
                    "text/json": components["schemas"]["CashRegisterOperationDto"];
                    "text/plain": components["schemas"]["CashRegisterOperationDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Conflict */
            409: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Dashboard_GetSummary: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["DashboardSummaryDto"];
                    "text/json": components["schemas"]["DashboardSummaryDto"];
                    "text/plain": components["schemas"]["DashboardSummaryDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    DataExport_ExportGuestsXlsx: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description Archivo descargable. */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet": string;
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    DataExport_ExportInvoicesXlsx: {
        parameters: {
            query?: {
                from?: string;
                to?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description Archivo descargable. */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet": string;
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    DataExport_ExportReservationsXlsx: {
        parameters: {
            query?: {
                from?: string;
                to?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description Archivo descargable. */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet": string;
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    DataExport_ExportRoomsXlsx: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description Archivo descargable. */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet": string;
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    DocumentAuthorizations_GetAll: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["DocumentAuthorizationDto"][];
                    "text/json": components["schemas"]["DocumentAuthorizationDto"][];
                    "text/plain": components["schemas"]["DocumentAuthorizationDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    DocumentAuthorizations_Create: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateDocumentAuthorizationRequest"];
                "application/json": components["schemas"]["CreateDocumentAuthorizationRequest"];
                "text/json": components["schemas"]["CreateDocumentAuthorizationRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["DocumentAuthorizationDto"];
                    "text/json": components["schemas"]["DocumentAuthorizationDto"];
                    "text/plain": components["schemas"]["DocumentAuthorizationDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    DocumentAuthorizations_GetActive: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                documentType: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["DocumentAuthorizationDto"];
                    "text/json": components["schemas"]["DocumentAuthorizationDto"];
                    "text/plain": components["schemas"]["DocumentAuthorizationDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    DocumentAuthorizations_GetOptionsPage: {
        parameters: {
            query?: {
                Source?: string;
                DocumentType?: string;
                ActiveOnly?: boolean;
                Search?: string;
                Page?: number | string;
                PageSize?: number | string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PagedResultOfAuthorizationOptionDto"];
                    "text/json": components["schemas"]["PagedResultOfAuthorizationOptionDto"];
                    "text/plain": components["schemas"]["PagedResultOfAuthorizationOptionDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    DocumentAuthorizations_CreateWithFile: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "multipart/form-data": {
                    CAINumber?: string;
                    DocumentType?: string;
                    /** Format: date */
                    DueDate?: string;
                    FinalRange?: string;
                    InitialRange?: string;
                    /** Format: date */
                    IssueDate?: string;
                } & {
                    file?: components["schemas"]["IFormFile"];
                };
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["DocumentAuthorizationDto"];
                    "text/json": components["schemas"]["DocumentAuthorizationDto"];
                    "text/plain": components["schemas"]["DocumentAuthorizationDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    DocumentAuthorizations_GetById: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["DocumentAuthorizationDto"];
                    "text/json": components["schemas"]["DocumentAuthorizationDto"];
                    "text/plain": components["schemas"]["DocumentAuthorizationDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    DocumentAuthorizations_Delete: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    DocumentAuthorizations_GetFile: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description Archivo descargable. */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/pdf": string;
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Payments_GetAll: {
        parameters: {
            query?: {
                invoiceId?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PaymentDto"][];
                    "text/json": components["schemas"]["PaymentDto"][];
                    "text/plain": components["schemas"]["PaymentDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Payments_Create: {
        parameters: {
            query?: never;
            header?: {
                "Idempotency-Key"?: string;
            };
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreatePaymentRequest"];
                "application/json": components["schemas"]["CreatePaymentRequest"];
                "text/json": components["schemas"]["CreatePaymentRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PaymentDto"];
                    "text/json": components["schemas"]["PaymentDto"];
                    "text/plain": components["schemas"]["PaymentDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Payments_GetById: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PaymentDto"];
                    "text/json": components["schemas"]["PaymentDto"];
                    "text/plain": components["schemas"]["PaymentDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Refunds_GetAll: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                paymentId: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["RefundDto"][];
                    "text/json": components["schemas"]["RefundDto"][];
                    "text/plain": components["schemas"]["RefundDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Refunds_Create: {
        parameters: {
            query?: never;
            header?: {
                "Idempotency-Key"?: string;
            };
            path: {
                paymentId: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateRefundRequest"];
                "application/json": components["schemas"]["CreateRefundRequest"];
                "text/json": components["schemas"]["CreateRefundRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["RefundDto"];
                    "text/json": components["schemas"]["RefundDto"];
                    "text/plain": components["schemas"]["RefundDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Refunds_GetById: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                paymentId: string;
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["RefundDto"];
                    "text/json": components["schemas"]["RefundDto"];
                    "text/plain": components["schemas"]["RefundDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    PurchaseInvoices_GetAll: {
        parameters: {
            query?: {
                supplierId?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PurchaseInvoiceDto"][];
                    "text/json": components["schemas"]["PurchaseInvoiceDto"][];
                    "text/plain": components["schemas"]["PurchaseInvoiceDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    PurchaseInvoices_Create: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreatePurchaseInvoiceRequest"];
                "application/json": components["schemas"]["CreatePurchaseInvoiceRequest"];
                "text/json": components["schemas"]["CreatePurchaseInvoiceRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PurchaseInvoiceDto"];
                    "text/json": components["schemas"]["PurchaseInvoiceDto"];
                    "text/plain": components["schemas"]["PurchaseInvoiceDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    PurchaseInvoices_GetByDateRange: {
        parameters: {
            query?: {
                start?: string;
                end?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PurchaseInvoiceDto"][];
                    "text/json": components["schemas"]["PurchaseInvoiceDto"][];
                    "text/plain": components["schemas"]["PurchaseInvoiceDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    PurchaseInvoices_GetById: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["PurchaseInvoiceDto"];
                    "text/json": components["schemas"]["PurchaseInvoiceDto"];
                    "text/plain": components["schemas"]["PurchaseInvoiceDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    PurchaseInvoices_Delete: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    PurchaseInvoices_MarkAsPaid: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Reports_GetBalanceSheet: {
        parameters: {
            query?: {
                asOfDate?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["BalanceSheetDto"];
                    "text/json": components["schemas"]["BalanceSheetDto"];
                    "text/plain": components["schemas"]["BalanceSheetDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Reports_GetIncomeStatement: {
        parameters: {
            query?: {
                from?: string;
                to?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["IncomeStatementDto"];
                    "text/json": components["schemas"]["IncomeStatementDto"];
                    "text/plain": components["schemas"]["IncomeStatementDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Reports_GetPurchasesBookXlsx: {
        parameters: {
            query?: {
                from?: string;
                to?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description Archivo descargable. */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet": string;
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Reports_GetSalesBookXlsx: {
        parameters: {
            query?: {
                from?: string;
                to?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description Archivo descargable. */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet": string;
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Reports_GetTaxSummary: {
        parameters: {
            query?: {
                from?: string;
                to?: string;
            };
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["TaxSummaryDto"];
                    "text/json": components["schemas"]["TaxSummaryDto"];
                    "text/plain": components["schemas"]["TaxSummaryDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    RoomTypes_GetAll: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["RoomTypeDto"][];
                    "text/json": components["schemas"]["RoomTypeDto"][];
                    "text/plain": components["schemas"]["RoomTypeDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    RoomTypes_Create: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateRoomTypeRequest"];
                "application/json": components["schemas"]["CreateRoomTypeRequest"];
                "text/json": components["schemas"]["CreateRoomTypeRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["RoomTypeDto"];
                    "text/json": components["schemas"]["RoomTypeDto"];
                    "text/plain": components["schemas"]["RoomTypeDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    RoomTypes_GetById: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["RoomTypeDto"];
                    "text/json": components["schemas"]["RoomTypeDto"];
                    "text/plain": components["schemas"]["RoomTypeDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    RoomTypes_Update: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["UpdateRoomTypeRequest"];
                "application/json": components["schemas"]["UpdateRoomTypeRequest"];
                "text/json": components["schemas"]["UpdateRoomTypeRequest"];
            };
        };
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    RoomTypes_Delete: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Settings_Get: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["BusinessSettingsDto"];
                    "text/json": components["schemas"]["BusinessSettingsDto"];
                    "text/plain": components["schemas"]["BusinessSettingsDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Settings_Update: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["UpdateBusinessSettingsRequest"];
                "application/json": components["schemas"]["UpdateBusinessSettingsRequest"];
                "text/json": components["schemas"]["UpdateBusinessSettingsRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["BusinessSettingsDto"];
                    "text/json": components["schemas"]["BusinessSettingsDto"];
                    "text/plain": components["schemas"]["BusinessSettingsDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Settings_ApproveFiscalProfile: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["ApproveFiscalProfileRequest"];
                "application/json": components["schemas"]["ApproveFiscalProfileRequest"];
                "text/json": components["schemas"]["ApproveFiscalProfileRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["BusinessSettingsDto"];
                    "text/json": components["schemas"]["BusinessSettingsDto"];
                    "text/plain": components["schemas"]["BusinessSettingsDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Settings_GetFiscalProfileHistory: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["FiscalProfileRevisionDto"][];
                    "text/json": components["schemas"]["FiscalProfileRevisionDto"][];
                    "text/plain": components["schemas"]["FiscalProfileRevisionDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Settings_RetireFiscalProfile: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["RetireFiscalProfileRequest"];
                "application/json": components["schemas"]["RetireFiscalProfileRequest"];
                "text/json": components["schemas"]["RetireFiscalProfileRequest"];
            };
        };
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["BusinessSettingsDto"];
                    "text/json": components["schemas"]["BusinessSettingsDto"];
                    "text/plain": components["schemas"]["BusinessSettingsDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    TaxConfigurations_GetAll: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["TaxConfigurationDto"][];
                    "text/json": components["schemas"]["TaxConfigurationDto"][];
                    "text/plain": components["schemas"]["TaxConfigurationDto"][];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    TaxConfigurations_Create: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateTaxConfigurationRequest"];
                "application/json": components["schemas"]["CreateTaxConfigurationRequest"];
                "text/json": components["schemas"]["CreateTaxConfigurationRequest"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["TaxConfigurationDto"];
                    "text/json": components["schemas"]["TaxConfigurationDto"];
                    "text/plain": components["schemas"]["TaxConfigurationDto"];
                };
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    TaxConfigurations_Update: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/*+json": components["schemas"]["CreateTaxConfigurationRequest"];
                "application/json": components["schemas"]["CreateTaxConfigurationRequest"];
                "text/json": components["schemas"]["CreateTaxConfigurationRequest"];
            };
        };
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    TaxConfigurations_Delete: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Solicitud inválida; los errores de validación se indican por campo. */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Se requiere una sesión válida. */
            401: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description El operador carece de permisos o debe cambiar la contraseña. */
            403: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
            /** @description Error interno normalizado; incluye traceId para seguimiento. */
            500: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": {
                        code: string;
                        detail: string;
                        errors?: null | {
                            [key: string]: string[];
                        };
                        instance: string;
                        /** Format: int32 */
                        status: number | string;
                        title: string;
                        traceId: string;
                        type: string;
                    };
                };
            };
        };
    };
    Health_Get: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["HealthResponseDto"];
                };
            };
        };
    };
}
