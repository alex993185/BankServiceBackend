using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankServiceBackend.Entities;
using BankServiceBackend.Repositories;

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
        public async Task<ActionResult<IEnumerable<Account>>> GetAll()
        {
            return new ActionResult<IEnumerable<Account>>(await _accountRepository.GetAllAsync());
        }

        // GET: api/Accounts/1
        [HttpGet("{accountNumber}")]
        public async Task<ActionResult<Account>> Get(long accountNumber)
        {
            var account = await _accountRepository.Get(accountNumber);
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
        public async Task<IActionResult> Update(long accountNumber, Account account)
        {
            if (accountNumber != account.AccountNumber)
            {
                return BadRequest();
            }

            var updateSuccessful = await _accountRepository.Update(accountNumber, account);
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
        public async Task<ActionResult<Account>> Save(Account account)
        {
            await _accountRepository.Save(account);
            return CreatedAtAction("Get", new { accountNumber = account.AccountNumber }, account);
        }

        // DELETE: api/Accounts/1
        [HttpDelete("{id}")]
        public async Task<ActionResult<Account>> Delete(long accountNumber)
        {
            var account = await _accountRepository.Get(accountNumber);
            if (account == null)
            {
                return NotFound("The account does not exist!");
            }

            await _accountRepository.Delete(account);
            return account;
        }
    }
}