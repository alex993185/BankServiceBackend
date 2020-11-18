using Microsoft.EntityFrameworkCore;

namespace BankServiceBackend.Persistance
{
    public class InMemoryDbContext : BankServiceDbContext
    {
        public InMemoryDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}