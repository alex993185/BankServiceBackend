using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankServiceBackend.Persistance.Entities;
using BankServiceBackend.Persistance.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BankServiceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAllAsync()
        {
            return new ActionResult<IEnumerable<Account>>(await _accountRepository.GetAllAsync());
        }

        // GET: api/Accounts/1
        [HttpGet("{accountNumber}")]
        public async Task<ActionResult<Account>> GetAsync(long accountNumber)
        {
            var account = await _accountRepository.GetAsync(accountNumber);
            if (account == null)
            {
                return NotFound("The account does not exist!");
            }

            return account;
        }

        // PUT: api/Accounts/1
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{accountNumber}")]
        public async Task<IActionResult> UpdateAsync(long accountNumber, Account account)
        {
            if (accountNumber != account.AccountNumber)
            {
                return BadRequest();
            }

            var updateSuccessful = await _accountRepository.UpdateAsync(accountNumber, account);
            if (!updateSuccessful)
            {
                return NotFound("The account does not exist!");
            }

            return NoContent();
        }

        // POST: api/Accounts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Account>> SaveAsync(Account account)
        {
            await _accountRepository.SaveAsync(account);
            return CreatedAtAction("GetAsync", new { accountNumber = account.AccountNumber }, account);
        }

        // DELETE: api/Accounts/1
        [HttpDelete("{accountNumber}")]
        public async Task<ActionResult<Account>> DeleteAsync(long accountNumber)
        {
            var account = await _accountRepository.GetAsync(accountNumber);
            if (account == null)
            {
                return NotFound("The account does not exist!");
            }

            await _accountRepository.DeleteAsync(account);
            return account;
        }
    }
}