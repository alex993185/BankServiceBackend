using System;
using System.Threading.Tasks;
using BankServiceBackend.Persistance.Repositories;

namespace BankServiceBackend.BusinessLogic.Handler
{
    public class TransactionHandler : ITransactionHandler
    {
        private readonly IAccountRepository _accountRepository;

        public TransactionHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task Deposit(long accountNumber, double amountInEuro, string hashedPin)
        {
            var persistedAccount = await _accountRepository.GetAsync(accountNumber);
            persistedAccount.Credit += amountInEuro;
            await _accountRepository.UpdateAsync(accountNumber, hashedPin, persistedAccount);
        }

        public async Task<bool> Withdraw(long accountNumber, double amountInEuro, string hashedPin)
        {
            var persistedAccount = await _accountRepository.GetAsync(accountNumber);
            if (persistedAccount.Credit + persistedAccount.Dispo > amountInEuro)
            {
                persistedAccount.Credit -= amountInEuro;
                await _accountRepository.UpdateAsync(accountNumber, hashedPin, persistedAccount);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
