using BankService.Backend.Persistance;
using Microsoft.EntityFrameworkCore;

namespace BankService.Backend.Tests.IntegrationTests
{
    public class PostgresDbContextFactory
    {
        private const string PostgresConnectionString = "User ID=postgres;Password=pass;Server=localhost;Port=5433;Database=bankservice;Integrated Security=true;Pooling=true;";

        public BankServiceDbContext CreateDbContext()
        {
            return new PostgresDbContext(new DbContextOptionsBuilder().UseNpgsql(PostgresConnectionString).Options);
        }
    }
}
