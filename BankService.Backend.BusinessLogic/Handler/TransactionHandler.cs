using System;
using System.Threading.Tasks;
using BankService.Backend.BusinessLogic.Exceptions;
using BankService.Backend.Persistance.Repositories;

namespace BankService.Backend.BusinessLogic.Handler
{
    public class TransactionHandler : ITransactionHandler
    {
        private readonly IAccountRepository _accountRepository;

        public TransactionHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task DepositAsync(long accountNumber, double amountInEuro, string hashedPin)
        {
            try
            {
                var persistedAccount = await _accountRepository.GetAsync(accountNumber);
                persistedAccount.Credit += amountInEuro;
                await _accountRepository.UpdateAsync(accountNumber, hashedPin, persistedAccount);
            }
            catch (Exception)
            {
                throw new DepositFailedException("Deposit failed!");
            }
        }

        public async Task<bool> Withdraw(long accountNumber, double amountInEuro, string hashedPin)
        {
            try
            {
                var persistedAccount = await _accountRepository.GetAsync(accountNumber);
                if (persistedAccount.Credit + persistedAccount.Dispo >= amountInEuro)
                {
                    persistedAccount.Credit -= amountInEuro;
                    await _accountRepository.UpdateAsync(accountNumber, hashedPin, persistedAccount);
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw new WithdrawFailedException("Deposit failed!");
            }
        }
    }
}
