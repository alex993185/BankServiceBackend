using BankServiceBackend.Persistance;
using Microsoft.EntityFrameworkCore;

namespace BankServiceBackend.Tests.IntegrationTests
{
    public class PersistanceTests
    {
        private const string PostgresConnectionString = "User ID=postgres;Password=pass;Server=localhost;Port=5433;Database=bankservice;Integrated Security=true;Pooling=true;";
        protected readonly PostgresDbContext _dbContext = new PostgresDbContext(new DbContextOptionsBuilder().UseNpgsql(PostgresConnectionString).Options);

    }
}
