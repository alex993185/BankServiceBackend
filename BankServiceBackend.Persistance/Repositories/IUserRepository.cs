using System.Collections.Generic;
using System.Threading.Tasks;
using BankServiceBackend.Persistance.Entities;

namespace BankServiceBackend.Persistance.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetAsync(long customerNumber);
        Task<User> UpdateAsync(long customerNumber, User user);
        Task<User> SaveAsync(User user);
        Task<User> RemoveAsync(long customerNumber);
    }
}
