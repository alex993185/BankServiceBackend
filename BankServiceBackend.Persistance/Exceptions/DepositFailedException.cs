namespace BankServiceBackend.Persistance.Exceptions
{
    public class DepositFailedException : UserFriendlyException
    {
        public DepositFailedException(string message) : base(message)
        {
        }
    }
}
