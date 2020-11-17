using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankServiceBackend.Entities;
using Microsoft.AspNetCore.Mvc;
using BankServiceBackend.Persistance.Entities;
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
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            var users = await _userRepository.GetAllAsync();
            return new ActionResult<IEnumerable<UserDTO>>(users.Select(GetTransferObject));
        }

        // GET: api/Users/1
        [HttpGet("{customerNumber}")]
        public async Task<ActionResult<UserDTO>> Get(long customerNumber)
        {
            var user = await _userRepository.GetAsync(customerNumber);
            if (user == null)
            {
                return NotFound($"The customer number {customerNumber} does not exist!");
            }

            return GetTransferObject(user);
        }

        // PUT: api/Users/1
        [HttpPut("{customerNumber}")]
        public async Task<IActionResult> Update(long customerNumber, UserDTO user)
        {
            if (_userRepository.GetAsync(user.CustomerNumber) == null)
            {
                return NotFound("The user does not exist. Updating failed!");
            }

            try
            {
                var userEntity = await _userRepository.UpdateAsync(customerNumber, GetEntity(user));
                return CreatedAtAction("Get", new { customerNumber = userEntity.CustomerNumber }, GetTransferObject(userEntity));
            }
            catch (Exception)
            {
                return Conflict("Updating user failed!");
            }
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> Create(UserDTO user)
        {
            var userEntity = await _userRepository.SaveAsync(GetEntity(user));
            return CreatedAtAction("Get", new { customerNumber = userEntity.CustomerNumber }, GetTransferObject(userEntity));
        }

        // DELETE: api/Users/1
        [HttpDelete("{customerNumber}")]
        public async Task<ActionResult<UserDTO>> Delete(long customerNumber)
        {
            var user = await _userRepository.RemoveAsync(customerNumber);
            if (user == null)
            {
                return BadRequest($"Customer number {customerNumber} does not exist!");
            }

            return GetTransferObject(user);
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
