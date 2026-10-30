using hotel_erp.Api.Infrastructure;
using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Accounting;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services;
using hotel_erp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hotel_erp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = PermissionNames.ManageAccounting)]
    public class AccountsController : ControllerBase
    {
        private readonly AccountingCatalogApplicationService _application;
        private readonly IAccountingRepository _repo;
        private readonly IMapper _mapper;

        private readonly HotelListQueryService _lists;

        public AccountsController(IAccountingRepository repo, IMapper mapper, HotelListQueryService lists, AccountingCatalogApplicationService application)
        {
            _repo = repo;
            _mapper = mapper;
            _lists = lists;
            _application = application;
        }

        [HttpGet]
        public async Task<ActionResult<List<AccountingAccountDto>>> GetAll()
        {
            var accounts = await _repo.GetAllAccountsAsync();
            var balances = await _repo.GetAllAccountBalancesAsync();
            var dtos = _mapper.Map<List<AccountingAccountDto>>(accounts);
            foreach (var dto in dtos)
                dto.Balance = balances.GetValueOrDefault(dto.Id, 0);
            var tree = BuildTree(dtos, null);
            return Ok(tree);
        }

        [HttpGet("flat")]
        public async Task<ActionResult<List<AccountingAccountDto>>> GetAllFlat()
        {
            var accounts = await _repo.GetAllAccountsAsync();
            var balances = await _repo.GetAllAccountBalancesAsync();
            var dtos = _mapper.Map<List<AccountingAccountDto>>(accounts);
            foreach (var dto in dtos)
                dto.Balance = balances.GetValueOrDefault(dto.Id, 0);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountingAccountDto>> GetById(Guid id)
        {
            var account = await _repo.GetAccountByIdAsync(id);
            if (account == null) return NotFound();
            var dto = _mapper.Map<AccountingAccountDto>(account);
            dto.Balance = await _repo.GetAccountBalanceAsync(id);
            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AccountingAccountDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<AccountingAccountDto>> Create([FromBody] CreateAccountRequest request)
        {
            return this.CatalogResult(await _application.CreateAsync(SecurityActor.From(User), request, HttpContext.RequestAborted), nameof(GetById));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateAccountRequest request)
        {
            return this.CatalogResult(await _application.UpdateAsync(SecurityActor.From(User), id, request, HttpContext.RequestAborted));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MessageResponseDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> Deactivate(Guid id)
        {
            return this.CatalogResult(await _application.DeactivateAsync(SecurityActor.From(User), id, HttpContext.RequestAborted));
        }

        [HttpGet("journal-entries")]
        public async Task<ActionResult<List<AccountingEntryDto>>> GetJournalEntries()
        {
            var entries = await _repo.GetAllEntriesAsync();
            return Ok(_mapper.Map<List<AccountingEntryDto>>(entries));
        }

        [HttpGet("journal-entries/paged")]
        public async Task<ActionResult<PagedResult<AccountingEntryListDto>>> GetJournalPage([FromQuery] SearchPageQuery query)
            => Ok(await _lists.JournalEntriesAsync(query, HttpContext.RequestAborted));

        [HttpGet("journal-entries/summary")]
        public async Task<ActionResult<JournalSummaryDto>> GetJournalSummary()
            => Ok(await _lists.JournalSummaryAsync(HttpContext.RequestAborted));

        [HttpGet("journal-entries/{id}")]
        [ProducesResponseType(typeof(AccountingEntryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AccountingEntryDto>> GetJournalEntry(Guid id)
        {
            var entry = await _repo.GetEntryByIdAsync(id);
            if (entry == null) return NotFound();
            return Ok(_mapper.Map<AccountingEntryDto>(entry));
        }

        [HttpPost("journal-entries")]
        [ProducesResponseType(typeof(AccountingEntryDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<AccountingEntryDto>> CreateJournalEntry([FromBody] CreateAccountingEntryRequest request)
        {
            return this.CatalogResult(await _application.CreateJournalEntryAsync(SecurityActor.From(User), request, HttpContext.RequestAborted), nameof(GetJournalEntry));
        }


        [HttpGet("trial-balance")]
        public async Task<ActionResult<List<TrialBalanceItem>>> GetTrialBalance()
        {
            var accounts = await _repo.GetAllAccountsAsync();
            var accountTotals = await _repo.GetAccountTotalsAsync();
            var items = new List<TrialBalanceItem>();

            foreach (var account in accounts.Where(a => a.IsActive))
            {
                var totals = accountTotals.TryGetValue(account.Id, out var t) ? t : (Debit: 0m, Credit: 0m);
                var totalDebit = totals.Debit;
                var totalCredit = totals.Credit;
                var balance = account.AccountType switch
                {
                    AccountType.Activo or AccountType.Gasto => totalDebit - totalCredit,
                    _ => totalCredit - totalDebit,
                };

                items.Add(new TrialBalanceItem
                {
                    AccountId = account.Id,
                    AccountNumber = account.AccountNumber,
                    AccountName = account.AccountName,
                    AccountType = account.AccountType.ToString(),
                    PreviousBalance = 0,
                    Debit = totalDebit,
                    Credit = totalCredit,
                    Balance = balance
                });
            }

            return Ok(items.OrderBy(i => i.AccountNumber).ToList());
        }

        [HttpGet("{id}/movements")]
        public async Task<ActionResult<List<AccountMovementDto>>> GetMovements(Guid id)
        {
            var account = await _repo.GetAccountByIdAsync(id);
            if (account == null) return NotFound();

            var movements = await _repo.GetAccountMovementsAsync(id);
            var runningBalance = 0m;

            var dtos = movements.Select(m =>
            {
                var diff = account.AccountType switch
                {
                    AccountType.Activo or AccountType.Gasto => m.Debit - m.Credit,
                    _ => m.Credit - m.Debit,
                };
                runningBalance += diff;
                return new AccountMovementDto
                {
                    Date = m.Entry.TransactionDate.ToDateTime(TimeOnly.MinValue),
                    Description = m.Description ?? m.Entry.Description,
                    Debit = m.Debit,
                    Credit = m.Credit,
                    Balance = runningBalance,
                    ReferenceType = m.Entry.EntryType.ToString()
                };
            }).ToList();

            return Ok(dtos);
        }

        private static List<AccountingAccountDto> BuildTree(List<AccountingAccountDto> flat, Guid? parentId)
        {
            return flat
                .Where(a => a.ParentAccountId == parentId)
                .Select(a =>
                {
                    a.Children = BuildTree(flat, a.Id);
                    return a;
                })
                .ToList();
        }
    }
}
