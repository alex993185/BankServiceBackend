namespace BankServiceBackend.Persistance.Exceptions
{
    public class RemovingFailedException : PersistenceException
    {
        public RemovingFailedException(string message) : base(message)
        {
        }
    }
}