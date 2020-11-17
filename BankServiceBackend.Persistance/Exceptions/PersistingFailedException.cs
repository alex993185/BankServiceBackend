namespace BankServiceBackend.Persistance.Exceptions
{
    public class PersistingFailedException : PersistenceException
    {
        public PersistingFailedException(string readableMessage) : base(readableMessage)
        {
        }
    }
}
