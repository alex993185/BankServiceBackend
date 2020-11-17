﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BankServiceBackend.Persistance.Entities;

namespace BankServiceBackend.Persistance.Repositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account> GetAsync(long accountNumber);
        Task<Account> UpdateAsync(long accountNumber, Account account);
        Task<Account> SaveAsync(Account account);
        Task<Account> DeleteAsync(long accountNumber);
    }
}