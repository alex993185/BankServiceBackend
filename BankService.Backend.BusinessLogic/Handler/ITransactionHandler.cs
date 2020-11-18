using System.Threading.Tasks;

namespace BankService.Backend.BusinessLogic.Handler
{
    public interface ITransactionHandler
    {
        Task DepositAsync(long accountNumber, double amountInEuro, string hashedPin);
        Task<bool> Withdraw(long accountNumber, double amountInEuro, string hashedPin);
    }
}