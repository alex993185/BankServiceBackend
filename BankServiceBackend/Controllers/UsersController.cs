using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankServiceBackend.Database;
using BankServiceBackend.Entities;

namespace BankServiceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly PostgresDbContext _context;

        public UsersController(PostgresDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/1
        [HttpGet("{customerNumber}")]
        public async Task<ActionResult<User>> Get(long customerNumber)
        {
            var user = await _context.Users.FindAsync(customerNumber);
            if (user == null)
            {
                return NotFound("The user does not exist!");
            }

            return user;
        }

        // PUT: api/Users/1
        [HttpPut("{customerNumber}")]
        public async Task<IActionResult> Update(long customerNumber, User user)
        {
            if (customerNumber != user.CustomerNumber)
            {
                return BadRequest();
            }

            if (!UserExists(user.CustomerNumber))
            {
                return NotFound("The user does not exist. Updating failed!");
            }

            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Updating user failed!");
            }

            return CreatedAtAction("Get", new { customerNumber = user.CustomerNumber }, user);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { customerNumber = user.CustomerNumber }, user);
        }

        // DELETE: api/Users/1
        [HttpDelete("{customerNumber}")]
        public async Task<ActionResult<User>> Delete(long customerNumber)
        {
            var user = await _context.Users.FindAsync(customerNumber);
            if (user == null)
            {
                return NotFound("The user does not exist. Deleting failed!");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(long customerNumber)
        {
            return _context.Users.Any(e => e.CustomerNumber == customerNumber);
        }
    }
}
