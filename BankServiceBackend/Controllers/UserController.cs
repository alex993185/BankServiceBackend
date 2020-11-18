using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankServiceBackend.Entities;
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
                return new ActionResult<IEnumerable<UserDTO>>(users.Select(GetTransferObject));
            }
            catch (UserFriendlyException e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Users/1
        [HttpGet("{customerNumber}")]
        public async Task<ActionResult<UserDTO>> GetAsync(long customerNumber)
        {
            try
            {
                var user = await _userRepository.GetAsync(customerNumber);
                return GetTransferObject(user);
            }
            catch (UserFriendlyException e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Users/1
        [HttpPut("{customerNumber}")]
        public async Task<ActionResult<UserDTO>> UpdateAsync(long customerNumber, UserDTO user)
        {
            try
            {
                var userEntity = await _userRepository.UpdateAsync(customerNumber, GetEntity(user));
                return GetTransferObject(userEntity);
            }
            catch (UserFriendlyException e)
            {
                return BadRequest(e);
            }
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateAsync(UserDTO user)
        {
            try
            {
                var userEntity = await _userRepository.SaveAsync(GetEntity(user));
                return GetTransferObject(userEntity);
            }
            catch (UserFriendlyException e)
            {
                return BadRequest(e);
            }
        }

        // DELETE: api/Users/1
        [HttpDelete("{customerNumber}")]
        public async Task<ActionResult<UserDTO>> DeleteAsync(long customerNumber)
        {
            try
            {
                var user = await _userRepository.RemoveAsync(customerNumber);
                return GetTransferObject(user);
            }
            catch (UserFriendlyException e)
            {
                return BadRequest(e);
            }
        }

        private UserDTO GetTransferObject(User user)
        {
            return new UserDTO { CustomerNumber = user.CustomerNumber, Birthday = user.Birthday, FirstName = user.FirstName, Name = user.Name, Gender = user.Gender };
        }
        
        private User GetEntity(UserDTO user)
        {
            return new User { Birthday = user.Birthday, FirstName = user.FirstName, Name = user.Name, Gender = user.Gender };
        }
    }
}
