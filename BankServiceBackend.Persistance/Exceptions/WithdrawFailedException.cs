namespace BankServiceBackend.Persistance.Exceptions
{
    public class WithdrawFailedException : UserFriendlyException
    {
        public WithdrawFailedException(string message) : base(message)
        {
        }
    }
}
