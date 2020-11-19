using System;
using System.Threading.Tasks;
using BankService.Backend.BusinessLogic.Exceptions;
using BankService.Backend.Persistance.Exceptions;
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
                var account = await _accountRepository.GetAsync(accountNumber);
                if (account.HashedPin != hashedPin)
                {
                    throw new DepositFailedException("Wrong PIN!");
                }

                await _accountRepository.DepositAsync(accountNumber, amountInEuro);
            }
            catch (UserFriendlyException e)
            {
                throw new DepositFailedException($"Deposit failed ({e.ReadableMessage})");
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
                var account = await _accountRepository.GetAsync(accountNumber);
                if (account.HashedPin != hashedPin)
                {
                    throw new WithdrawFailedException("Wrong PIN!");
                }

                if (account.Credit + account.Dispo >= amountInEuro)
                {
                    await _accountRepository.WithdrawAsync(accountNumber, amountInEuro);
                    return true;
                }

                return false;
            }
            catch (UserFriendlyException e)
            {
                throw new WithdrawFailedException($"Withdraw failed ({e.ReadableMessage})");
            }
            catch (Exception)
            {
                throw new WithdrawFailedException("Withdraw failed!");
            }
        }
    }
}