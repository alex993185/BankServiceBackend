using System.Threading.Tasks;

namespace BankServiceBackend.BusinessLogic.Handler
{
    public interface ITransactionHandler
    {
        Task DepositAsync(long accountNumber, double amountInEuro, string hashedPin);
        Task<bool> Withdraw(long accountNumber, double amountInEuro, string hashedPin);
    }
}