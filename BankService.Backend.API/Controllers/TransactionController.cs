using System;
using System.Threading.Tasks;
using BankService.Backend.BusinessLogic.Exceptions;
using BankService.Backend.BusinessLogic.Handler;
using Microsoft.AspNetCore.Mvc;

namespace BankService.Backend.API.Controllers
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

        [HttpPost]
        [Route("deposit")]
        public async Task<ActionResult> Deposit([FromQuery] long accountNumber, [FromQuery] double amountInEuro, [FromQuery] string hashedPin)
        {
            try
            {
                await _transactionHandler.DepositAsync(accountNumber, amountInEuro, hashedPin);
                return Ok();
            }
            catch (DepositFailedException e)
            {
                return BadRequest(e.ReadableMessage);
            }
        }

        [HttpPost]
        [Route("withdraw")]
        public async Task<ActionResult> Withdraw([FromQuery] long accountNumber, [FromQuery] double amountInEuro, [FromQuery] string hashedPin)
        { 
            try
            {
                if (await _transactionHandler.Withdraw(accountNumber, amountInEuro, hashedPin))
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch (WithdrawFailedException e)
            {
                return BadRequest(e.ReadableMessage);
            }
        }
    }
}