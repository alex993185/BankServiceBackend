namespace BankServiceBackend.Persistance.Exceptions
{
    public class FetchingFailedException : UserFriendlyException
    {
        public FetchingFailedException(string message) : base(message)
        {
        }
    }
}
