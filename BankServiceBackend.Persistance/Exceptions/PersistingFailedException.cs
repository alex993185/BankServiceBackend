namespace BankServiceBackend.Persistance.Exceptions
{
    public class PersistingFailedException : UserFriendlyException
    {
        public PersistingFailedException(string readableMessage) : base(readableMessage)
        {
        }
    }
}
