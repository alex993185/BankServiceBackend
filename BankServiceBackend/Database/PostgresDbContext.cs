using System;
using BankServiceBackend.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankServiceBackend.Database
{
    public class PostgresDbContext : DbContext
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
}