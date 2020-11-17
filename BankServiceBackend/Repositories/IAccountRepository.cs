using System.Collections.Generic;
using System.Threading.Tasks;
using BankServiceBackend.Entities;

namespace BankServiceBackend.Repositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account> Get(long accountNumber);
        Task<bool> Update(long accountNumber, Account account);
        Task Save(Account account);
        Task Delete(Account account);
    }
}