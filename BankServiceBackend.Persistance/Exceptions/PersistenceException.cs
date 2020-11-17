using System;

namespace BankServiceBackend.Persistance.Exceptions
{
    public abstract class PersistenceException : Exception
    {
        public PersistenceException(string message)
        {
            ReadableMessage = message;
        }

        public string ReadableMessage { get; }
    }
}
