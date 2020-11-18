using Microsoft.EntityFrameworkCore;

namespace BankService.Backend.Persistance
{
    public class InMemoryDbContext : BankServiceDbContext
    {
        public InMemoryDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}