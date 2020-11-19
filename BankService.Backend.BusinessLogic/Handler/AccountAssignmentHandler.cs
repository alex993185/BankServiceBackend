using System;
using System.Threading.Tasks;
using BankService.Backend.BusinessLogic.Exceptions;
using BankService.Backend.Persistance.Exceptions;
using BankService.Backend.Persistance.Repositories;

namespace BankService.Backend.BusinessLogic.Handler
{
    public class AccountAssignmentHandler : IAccountAssignmentHandler
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;

        public AccountAssignmentHandler(IAccountRepository accountRepository, IUserRepository userRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task AssignAsync(long accountNumber, long customerNumber, string hashedPin)
        {
            try
            {
                var account = await _accountRepository.GetAsync(accountNumber);
                if (account.HashedPin != hashedPin)
                {
                    throw new PersistingFailedException("Wrong PIN!");
                }

                await _accountRepository.Assign(accountNumber, customerNumber);
            }
            catch (UserFriendlyException e)
            {
                throw new AssignmentException($"Assigning account to user failed. ({e.ReadableMessage})");
            }
        }
    }
}
