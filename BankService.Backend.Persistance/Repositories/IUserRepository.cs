using System.Collections.Generic;
using System.Threading.Tasks;
using BankService.Backend.Persistance.Entities;

namespace BankService.Backend.Persistance.Repositories
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
