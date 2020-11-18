using System;
using System.Threading.Tasks;
using BankServiceBackend.BusinessLogic.Handler;
using Microsoft.AspNetCore.Mvc;

namespace BankServiceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionHandler _transactionHandler;

        public TransactionController(ITransactionHandler transactionHandler)
        {
            _transactionHandler = transactionHandler ?? throw new ArgumentNullException(nameof(transactionHandler));
        }

        [HttpGet]
        [Route("deposit")]
        public async Task<ActionResult> Deposit([FromQuery] long accountNumber, [FromQuery] double amountInEuro, [FromQuery] string hashedPin)
        {
            if (await _transactionHandler.Deposit(accountNumber, amountInEuro, hashedPin))
            {
                return Ok();
            }
            else
            {
                return BadRequest("Deposit failed!");
            }
        }

        [HttpGet]
        [Route("withdraw")]
        public async Task<ActionResult> Withdraw([FromQuery] long accountNumber, [FromQuery] double amountInEuro, [FromQuery] string hashedPin)
        {
            if (await _transactionHandler.Withdraw(accountNumber, amountInEuro, hashedPin))
            {
                return Ok();
            }
            else
            {
                return BadRequest("Withdrawal failed!");
            }
        }
    }
}
