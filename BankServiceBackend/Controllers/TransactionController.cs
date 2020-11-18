﻿using System;
using System.Threading.Tasks;
using BankServiceBackend.BusinessLogic;
using BankServiceBackend.BusinessLogic.Exceptions;
using BankServiceBackend.BusinessLogic.Handler;
using BankServiceBackend.Persistance.Exceptions;
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

        [HttpGet]
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