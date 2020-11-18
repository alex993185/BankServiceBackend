using System.Collections.Generic;
using System.Threading.Tasks;
using BankService.Backend.Persistance.Entities;

namespace BankService.Backend.Persistance.Repositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account> GetAsync(long accountNumber);
        Task<Account> UpdateAsync(long accountNumber, string hashedPin, Account account);
        Task<Account> SaveAsync(string hashedPin, Account account);
        Task<Account> DeleteAsync(long accountNumber, string hashedPin);
    }
}