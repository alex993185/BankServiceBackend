using Microsoft.EntityFrameworkCore;

namespace BankService.Backend.Persistance
{
    public class PostgresDbContext : BankServiceDbContext
    {
        public PostgresDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}