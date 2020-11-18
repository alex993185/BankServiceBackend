using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankService.Backend.API.Entities;
using BankService.Backend.API.Extensions;
using BankService.Backend.Persistance.Exceptions;
using BankService.Backend.Persistance.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BankService.Backend.API.Controllers
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
                return new ActionResult<IEnumerable<AccountDTO>>(accounts.Select(a => a.GetTransferObject()));
            }
            catch (UserFriendlyException e)
            {
                return BadRequest(e.ReadableMessage);
            }
        }

        // GET: api/Accounts/1
        [HttpGet("{accountNumber}")]
        public async Task<ActionResult<AccountDTO>> GetAsync(long accountNumber)
        {
            try
            {
                var account = await _accountRepository.GetAsync(accountNumber);
                return account.GetTransferObject();
            }
            catch (UserFriendlyException e)
            {
                return BadRequest(e.ReadableMessage);
            }
        }
  
        // PUT: api/Accounts/1
        [HttpPut]
        public async Task<ActionResult<AccountDTO>> UpdateAsync([FromQuery] long accountNumber, [FromQuery] string hashedPin, AccountDTO account)
        {
            try
            {
                var accountEntity = await _accountRepository.UpdateAsync(accountNumber, hashedPin, account.GetEntity());
                return accountEntity.GetTransferObject();
            }
            catch (UserFriendlyException e)
            {
                return BadRequest(e.ReadableMessage);
            }
        }

        // POST: api/Accounts
        [HttpPost]
        public async Task<ActionResult<AccountDTO>> SaveAsync([FromQuery] string hashedPin, AccountDTO account)
        {
            try
            {
                var accountEntity = await _accountRepository.SaveAsync(hashedPin, account.GetEntity());
                return accountEntity.GetTransferObject();
            }
            catch (UserFriendlyException e)
            {
                return BadRequest(e.ReadableMessage);
            }
        }

        // DELETE: api/Accounts/1
        [HttpDelete]
        public async Task<ActionResult<AccountDTO>> DeleteAsync([FromQuery] long accountNumber, [FromQuery] string hashedPin)
        {
            try
            {
                var account = await _accountRepository.GetAsync(accountNumber);
                await _accountRepository.DeleteAsync(accountNumber, hashedPin);
                return account.GetTransferObject();
            }
            catch (UserFriendlyException e)
            {
                return BadRequest(e.ReadableMessage);
            }
        }
    }
}