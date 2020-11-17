using System;
using System.Threading.Tasks;
using BankServiceBackend.Database;
using BankServiceBackend.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BankServiceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly PostgresDbContext _context;

        public TransactionController(PostgresDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // POST: api/Transaction
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("deposit")]
        public async Task<ActionResult> Deposit(Account account, double amountInEuro)
        {
            var persistedAccount = await _context.Accounts.FindAsync(account.AccountNumber);
            if (persistedAccount == null)
            {
                return NotFound("The account does not exist!");
            }

            if (persistedAccount.HashedPin != account.HashedPin)
            {
                return BadRequest("Wrong PIN!");
            }

            persistedAccount.Credit -= amountInEuro;
            return Ok();
        }

        [HttpPost]
        [Route("withdraw")]
        public async Task<ActionResult> Withdraw(Account account, double amountInEuro)
        {
            var persistedAccount = await _context.Accounts.FindAsync(account.AccountNumber);
            if (persistedAccount == null)
            {
                return NotFound("The account does not exist!");
            }

            if (persistedAccount.HashedPin != account.HashedPin)
            {
                return BadRequest("Wrong PIN!");
            }

            if (persistedAccount.Credit - amountInEuro < persistedAccount.Dispo)
            {
                return BadRequest("Dispo limit exceeded. Transaction cancelled!");
            }

            persistedAccount.Credit -= amountInEuro;
            return Ok();
        }
    }
}
