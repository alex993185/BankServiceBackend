using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BankServiceBackend.BusinessLogic.Handler;
using BankServiceBackend.Persistance.Entities;
using BankServiceBackend.Persistance.Exceptions;
using BankServiceBackend.Persistance.Repositories;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace BankServiceBackend.Tests.IntegrationTests
{
    [TestFixture]
    public class TransactionHandlerTests
    {
        [Test]
        public void Ctor_ArgumentNull_ExceptionIsThrown()
        {
            Assert.That(() => new TransactionHandler(null), Throws.ArgumentNullException);
        }

        [Test]
        public void DepositAsync_UnknownAccount_DepositFailedExceptionIsThrown()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(ar => ar.GetAsync(It.IsAny<long>())).Throws(new FetchingFailedException(""));
            var sut = new TransactionHandler(accountRepositoryMock.Object);

            Assert.ThrowsAsync<DepositFailedException>(async () => await sut.DepositAsync(long.MaxValue, 10, "Hash"));
        }

        [Test]
        public void DepositAsync_WrongPinHash_DepositFailedExceptionIsThrown()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var account = new Account {AccountNumber = 1, HashedPin = "Hash"};
            accountRepositoryMock.Setup(ar => ar.GetAsync(1)).Returns(Task.FromResult(account));
            accountRepositoryMock.Setup(ar => ar.UpdateAsync(1, It.IsAny<string>(), It.IsAny<Account>())).Throws(new PersistingFailedException("Wrong PIN!"));
            var sut = new TransactionHandler(accountRepositoryMock.Object);

            Assert.ThrowsAsync<DepositFailedException>(async () => await sut.DepositAsync(1, 10, "WrongHash"));
        }

        [Test]
        public void DepositAsync_CorrectPinHash_NothingIsThrown()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var account = new Account { AccountNumber = 1, HashedPin = "Hash" };
            accountRepositoryMock.Setup(ar => ar.GetAsync(1)).Returns(Task.FromResult(account));
            accountRepositoryMock.Setup(ar => ar.UpdateAsync(1, It.IsAny<string>(), It.IsAny<Account>())).Returns(Task.FromResult(new Account { AccountNumber = 1, Credit = 10 }));
            var sut = new TransactionHandler(accountRepositoryMock.Object);

            Assert.That(async () => await sut.DepositAsync(1, 10, "Hash"), Throws.Nothing, "Expected deposit is successful!");
        }

        [Test]
        public void WithdrawAsync_UnknownAccount_WithdrawFailedExceptionIsThrown()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(ar => ar.GetAsync(It.IsAny<long>())).Throws(new FetchingFailedException(""));
            var sut = new TransactionHandler(accountRepositoryMock.Object);

            Assert.ThrowsAsync<WithdrawFailedException>(async () => await sut.Withdraw(long.MaxValue, 10, "Hash"));
        }

        [Test]
        public void WithdrawAsync_WrongPinHash_WithdrawFailedExceptionIsThrown()
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var account = new Account { AccountNumber = 1, HashedPin = "Hash", Credit = 1000 };
            accountRepositoryMock.Setup(ar => ar.GetAsync(1)).Returns(Task.FromResult(account));
            accountRepositoryMock.Setup(ar => ar.UpdateAsync(1, It.IsAny<string>(), It.IsAny<Account>())).Throws(new PersistingFailedException("Wrong PIN!"));
            var sut = new TransactionHandler(accountRepositoryMock.Object);

            Assert.ThrowsAsync<WithdrawFailedException>(async () => await sut.Withdraw(1, 10, "WrongHash"));
        }

        [TestCase(0, 0, false)]
        [TestCase(100, 0, true)]
        [TestCase(0, 100, true)]
        [TestCase(50, 50, true)]
        [TestCase(0, 99.999, false)]
        [TestCase(99.999, 0, false)]
        public void WithdrawAsync_CreditAndDispo_AreConsidered(double credit, double dispo, bool isWithdrawalSuccessful)
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var account = new Account { AccountNumber = 1, HashedPin = "Hash", Credit = credit, Dispo = dispo };
            accountRepositoryMock.Setup(ar => ar.GetAsync(1)).Returns(Task.FromResult(account));
            accountRepositoryMock.Setup(ar => ar.UpdateAsync(1, It.IsAny<string>(), It.IsAny<Account>())).Returns(Task.FromResult(new Account { AccountNumber = 1, Credit = 0 }));
            var sut = new TransactionHandler(accountRepositoryMock.Object);

            Assert.That(async () => await sut.Withdraw(1, 100, "Hash"), Is.EqualTo(isWithdrawalSuccessful));
        }
    }
}
