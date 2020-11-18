using BankServiceBackend.Persistance.Exceptions;

namespace BankServiceBackend.BusinessLogic.Exceptions
{
    public class WithdrawFailedException : UserFriendlyException
    {
        public WithdrawFailedException(string message) : base(message)
        {
        }
    }
}
