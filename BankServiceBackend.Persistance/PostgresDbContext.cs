using BankServiceBackend.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankServiceBackend.Persistance
{
    public class PostgresDbContext : DbContext
    {
        public PostgresDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransferTransaction> TransferTransactions { get; set; }
    }
}