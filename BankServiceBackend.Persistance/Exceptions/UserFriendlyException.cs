using System;

namespace BankServiceBackend.Persistance.Exceptions
{
    public abstract class UserFriendlyException : Exception
    {
        public UserFriendlyException(string message)
        {
            ReadableMessage = message;
        }

        public string ReadableMessage { get; }
    }
}
