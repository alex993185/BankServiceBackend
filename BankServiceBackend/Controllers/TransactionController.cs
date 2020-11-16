using System.Threading.Tasks;
using BankServiceBackend.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BankServiceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    { 
        // POST: api/Transaction
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("deposit")]
        public async Task<ActionResult> Deposit(Account account, double amount)
        {
            return Ok();
        }

        [HttpPost]
        [Route("withdraw")]
        public async Task<ActionResult> Withdraw(Account account, double amount)
        {
            return Ok();
        }
    }
}
