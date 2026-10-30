using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Accounting;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services.Interfaces;

namespace hotel_erp.Api.Services;

public sealed class AccountingCatalogApplicationService(
    IAccountingRepository _repo, IMapper _mapper,
    OperationalMutationGuard access, CatalogMutationCommitter _mutations)
{
    public async Task<CatalogMutationResult> CreateAsync(SecurityActor actor, CreateAccountRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageAccounting, cancellationToken);
        await _mutations.LockAsync("Accounting", cancellationToken);

        if (await _repo.AccountNumberExistsAsync(request.AccountNumber))
            return CatalogMutationResult.BadRequest("El número de cuenta ya existe");

        if (!Enum.TryParse<AccountType>(request.AccountType, out var accountType))
            return CatalogMutationResult.BadRequest("Tipo de cuenta inválido. Use: Activo, Pasivo, Patrimonio, Ingreso, Gasto");

        var account = new AccountingAccount
        {
            AccountNumber = request.AccountNumber,
            AccountName = request.AccountName,
            AccountType = accountType,
            ParentAccountId = request.ParentAccountId,
            IsActive = true
        };

        await _repo.AddAccountAsync(account);
        await _mutations.PersistAsync(actor, "CreateAccountingAccount", nameof(AccountingAccount), () => account.Id, cancellationToken);
        var dto = _mapper.Map<AccountingAccountDto>(account);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.Created(account.Id, dto);
    }

    public async Task<CatalogMutationResult> UpdateAsync(SecurityActor actor, Guid id, UpdateAccountRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageAccounting, cancellationToken);
        await _mutations.LockAsync("Accounting", cancellationToken);

        var account = await _repo.GetAccountByIdAsync(id);
        if (account == null) return CatalogMutationResult.NotFound();

        if (request.AccountName != null)
            account.AccountName = request.AccountName;

        if (request.AccountType != null)
        {
            if (!Enum.TryParse<AccountType>(request.AccountType, out var accountType))
                return CatalogMutationResult.BadRequest("Tipo de cuenta inválido");
            account.AccountType = accountType;
        }

        if (request.IsActive.HasValue)
            account.IsActive = request.IsActive.Value;

        await _repo.UpdateAccountAsync(account);
        await _mutations.PersistAsync(actor, "UpdateAccountingAccount", nameof(AccountingAccount), () => id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.NoContent();
    }

    public async Task<CatalogMutationResult> DeactivateAsync(SecurityActor actor, Guid id, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageAccounting, cancellationToken);
        await _mutations.LockAsync("Accounting", cancellationToken);

        var account = await _repo.GetAccountByIdAsync(id);
        if (account == null) return CatalogMutationResult.NotFound();

        var movements = await _repo.GetAccountMovementsAsync(id);
        if (movements.Any())
        {
            account.IsActive = false;
            await _repo.UpdateAccountAsync(account);
            await _mutations.PersistAsync(actor, "DeactivateAccountingAccount", nameof(AccountingAccount), () => id, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return CatalogMutationResult.Ok(new { message = "Cuenta desactivada (tiene movimientos)" });
        }

        await _repo.DeleteAccountAsync(account);
        await _mutations.PersistAsync(actor, "DeactivateAccountingAccount", nameof(AccountingAccount), () => id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.Ok(new { message = "Cuenta eliminada" });
    }

    public async Task<CatalogMutationResult> CreateJournalEntryAsync(SecurityActor actor, CreateAccountingEntryRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageAccounting, cancellationToken);
        await _mutations.LockAsync("Accounting", cancellationToken);

        if (request.Items == null || request.Items.Count < 2)
            return CatalogMutationResult.BadRequest(new { message = "Un asiento contable requiere al menos 2 cuentas (partida doble)" });

        var totalDebit = request.Items.Sum(i => i.Debit);
        var totalCredit = request.Items.Sum(i => i.Credit);

        if (totalDebit <= 0 || totalCredit <= 0)
            return CatalogMutationResult.BadRequest(new { message = "Los importes del asiento deben ser mayores a cero" });

        if (Math.Round(totalDebit, 2) != Math.Round(totalCredit, 2))
            return CatalogMutationResult.BadRequest(new { message = $"El asiento está descuadrado. Débitos: L {totalDebit:N2} vs Créditos: L {totalCredit:N2}" });

        var accounts = await _repo.GetAllAccountsAsync();
        var accountDict = accounts.ToDictionary(a => a.Id);

        var entryType = Enum.TryParse<EntryType>(request.EntryType, true, out var parsedType)
            ? parsedType
            : EntryType.Diario;

        var entry = new AccountingEntry
        {
            TransactionDate = request.TransactionDate,
            Description = request.Description.Trim(),
            EntryType = entryType,
            ReferenceId = null,
        };

        foreach (var item in request.Items)
        {
            if (!accountDict.ContainsKey(item.AccountId))
                return CatalogMutationResult.BadRequest(new { message = $"La cuenta contable {item.AccountId} no existe" });

            if (item.Debit > 0 && item.Credit > 0)
                return CatalogMutationResult.BadRequest(new { message = "Una línea contable no puede tener débito y crédito simultáneamente" });

            entry.EntryItems.Add(new EntryItem
            {
                AccountId = item.AccountId,
                Debit = item.Debit,
                Credit = item.Credit,
                Description = item.Description?.Trim()
            });
        }

        await _repo.AddEntryAsync(entry);
        await _mutations.PersistAsync(actor, "CreateManualAccountingEntry", nameof(AccountingEntry), () => entry.Id, cancellationToken);

        var createdEntry = await _repo.GetEntryByIdAsync(entry.Id);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.Created(entry.Id, _mapper.Map<AccountingEntryDto>(createdEntry ?? entry));
    }
}
