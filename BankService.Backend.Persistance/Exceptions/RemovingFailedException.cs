namespace BankServiceBackend.Persistance.Exceptions
{
    public class RemovingFailedException : UserFriendlyException
    {
        public RemovingFailedException(string message) : base(message)
        {
        }
    }
}