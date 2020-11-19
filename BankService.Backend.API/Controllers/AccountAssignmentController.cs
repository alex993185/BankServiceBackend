using System;
using System.Threading.Tasks;
using BankService.Backend.BusinessLogic.Handler;
using BankService.Backend.Persistance.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BankService.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountAssignmentController : ControllerBase
    {
        private readonly IAccountAssignmentHandler _accountAssignmentHandler;

        public AccountAssignmentController(IAccountAssignmentHandler accountAssignmentHandler)
        {
            _accountAssignmentHandler = accountAssignmentHandler ?? throw new ArgumentNullException(nameof(accountAssignmentHandler));
        }

        [HttpGet]
        [Route("assign")]
        public async Task<ActionResult> Assign([FromQuery] long accountNumber, [FromQuery] long customerNumber, [FromQuery] string hashedPin)
        {
            try
            {
                await _accountAssignmentHandler.AssignAsync(accountNumber, customerNumber, hashedPin);
                return Ok();
            }
            catch (UserFriendlyException e)
            {
                return BadRequest(e.ReadableMessage);
            }
        }
    }
}
