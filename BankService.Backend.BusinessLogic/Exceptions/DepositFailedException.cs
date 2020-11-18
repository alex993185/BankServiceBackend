using BankService.Backend.Persistance.Exceptions;

namespace BankService.Backend.BusinessLogic.Exceptions
{
    public class DepositFailedException : UserFriendlyException
    {
        public DepositFailedException(string message) : base(message)
        {
        }
    }
}
