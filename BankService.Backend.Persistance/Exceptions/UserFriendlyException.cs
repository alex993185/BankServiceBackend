using System;

namespace BankService.Backend.Persistance.Exceptions
{
    public class UserFriendlyException : Exception
    {
        public UserFriendlyException(string message)
        {
            ReadableMessage = message;
        }

        public string ReadableMessage { get; }
    }
}
