﻿using BankService.Backend.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankService.Backend.Persistance
{
    public abstract class BankServiceDbContext : DbContext
    {
        protected BankServiceDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
}