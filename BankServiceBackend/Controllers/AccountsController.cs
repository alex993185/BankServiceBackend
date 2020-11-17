using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankServiceBackend.Entities;
using BankServiceBackend.Persistance.Entities;
using BankServiceBackend.Persistance.Exceptions;
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
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAllAsync()
        {
            try
            {
                var accounts = await _accountRepository.GetAllAsync();
                return new ActionResult<IEnumerable<AccountDTO>>(accounts.Select(GetTransferObject));
            }
            catch (PersistenceException e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Accounts/1
        [HttpGet("{accountNumber}")]
        public async Task<ActionResult<AccountDTO>> GetAsync(long accountNumber)
        {
            try
            {
                var account = await _accountRepository.GetAsync(accountNumber);
                return GetTransferObject(account);
            }
            catch (PersistenceException e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Accounts/1
        [HttpPut("{accountNumber}")]
        public async Task<ActionResult<AccountDTO>> UpdateAsync(long accountNumber, AccountDTO account)
        {
            try
            {
                var accountEntity = await _accountRepository.UpdateAsync(accountNumber, GetEntity(account));
                return GetTransferObject(accountEntity);
            }
            catch (PersistenceException e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: api/Accounts
        [HttpPost]
        public async Task<ActionResult<AccountDTO>> SaveAsync(AccountDTO account)
        {
            try
            {
                var accountEntity = await _accountRepository.SaveAsync(GetEntity(account));
                return GetTransferObject(accountEntity);
            }
            catch (PersistenceException e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Accounts/1
        [HttpDelete("{accountNumber}")]
        public async Task<ActionResult<AccountDTO>> DeleteAsync(long accountNumber)
        {
            try
            {
                var account = await _accountRepository.GetAsync(accountNumber);
                await _accountRepository.DeleteAsync(accountNumber);
                return GetTransferObject(account);
            }
            catch (PersistenceException e)
            {
                return BadRequest(e.Message);
            }
        }

        private AccountDTO GetTransferObject(Account account)
        {
            return new AccountDTO { AccountNumber = account.AccountNumber, Name = account.Name, Credit = account.Credit, Dispo = account.Dispo };
        }

        private Account GetEntity(AccountDTO account)
        {
            return new Account { AccountNumber = account.AccountNumber, Name = account.Name, Credit = account.Credit, Dispo = account.Dispo };
        }

    }
}