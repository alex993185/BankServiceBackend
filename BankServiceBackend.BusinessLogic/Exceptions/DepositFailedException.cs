using BankServiceBackend.Persistance.Exceptions;

namespace BankServiceBackend.BusinessLogic.Exceptions
{
    public class DepositFailedException : UserFriendlyException
    {
        public DepositFailedException(string message) : base(message)
        {
        }
    }
}
