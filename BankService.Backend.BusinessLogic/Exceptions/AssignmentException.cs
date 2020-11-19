using BankService.Backend.Persistance.Exceptions;

namespace BankService.Backend.BusinessLogic.Exceptions
{
    public class AssignmentException : UserFriendlyException
    {
        public AssignmentException(string message) : base(message)
        {
        }
    }
}