using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankServiceBackend.Entities;
using BankServiceBackend.Extensions;
using Microsoft.AspNetCore.Mvc;
using BankServiceBackend.Persistance.Entities;
using BankServiceBackend.Persistance.Exceptions;
using BankServiceBackend.Persistance.Repositories;

namespace BankServiceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllAsync()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                return new ActionResult<IEnumerable<UserDTO>>(users.Select(u => u.GetTransferObject()));
            }
            catch (UserFriendlyException e)
            {
                return BadRequest(e.ReadableMessage);
            }
        }

        // GET: api/Users/1
        [HttpGet("{customerNumber}")]
        public async Task<ActionResult<UserDTO>> GetAsync(long customerNumber)
        {
            try
            {
                var user = await _userRepository.GetAsync(customerNumber);
                return user.GetTransferObject();
            }
            catch (UserFriendlyException e)
            {
                return BadRequest(e.ReadableMessage);
            }
        }

        // PUT: api/Users/1
        [HttpPut("{customerNumber}")]
        public async Task<ActionResult<UserDTO>> UpdateAsync(long customerNumber, UserDTO user)
        {
            try
            {
                var userEntity = await _userRepository.UpdateAsync(customerNumber, user.GetEntity());
                return userEntity.GetTransferObject();
            }
            catch (UserFriendlyException e)
            {
                return BadRequest(e.ReadableMessage);
            }
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateAsync(UserDTO user)
        {
            try
            {
                var userEntity = await _userRepository.SaveAsync(user.GetEntity());
                return userEntity.GetTransferObject();
            }
            catch (UserFriendlyException e)
            {
                return BadRequest(e.ReadableMessage);
            }
        }

        // DELETE: api/Users/1
        [HttpDelete("{customerNumber}")]
        public async Task<ActionResult<UserDTO>> DeleteAsync(long customerNumber)
        {
            try
            {
                var user = await _userRepository.RemoveAsync(customerNumber);
                return user.GetTransferObject();
            }
            catch (UserFriendlyException e)
            {
                return BadRequest(e.ReadableMessage);
            }
        }
    }
}