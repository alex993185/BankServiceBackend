namespace BankServiceBackend.Persistance.Exceptions
{
    public class FetchingFailedException : PersistenceException
    {
        public FetchingFailedException(string message) : base(message)
        {
        }
    }
}
