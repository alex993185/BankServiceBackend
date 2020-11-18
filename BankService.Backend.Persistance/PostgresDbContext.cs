using Microsoft.EntityFrameworkCore;

namespace BankServiceBackend.Persistance
{
    public class PostgresDbContext : BankServiceDbContext
    {
        public PostgresDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}