using BankService.Backend.Persistance.Exceptions;

namespace BankService.Backend.BusinessLogic.Exceptions
{
    public class WithdrawFailedException : UserFriendlyException
    {
        public WithdrawFailedException(string message) : base(message)
        {
        }
    }
}
