using System.Threading.Tasks;

namespace BankService.Backend.BusinessLogic.Handler
{
    public interface IAccountAssignmentHandler
    {
        Task AssignAsync(long accountNumber, long customerNumber, string hashedPin);
    }
}