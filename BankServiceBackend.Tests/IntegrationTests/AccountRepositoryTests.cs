﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankServiceBackend.Persistance.Entities;
using BankServiceBackend.Persistance.Exceptions;
using BankServiceBackend.Persistance.Repositories;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace BankServiceBackend.Tests.IntegrationTests
{
    [TestFixture]
    public class AccountRepositoryTests : PersistanceTests
    {
        [Test]
        public void Ctor_ArgumentNull_ExceptionIsThrown()
        {
            Assert.That(() => new AccountRepository(null), Throws.ArgumentNullException);
        }

        [Test]
        public async Task GetAllAsync_PersistedAccounts_AreReturned()
        {
            // Arrange
            var sut = new AccountRepository(_dbContext);
            var account1 = await sut.SaveAsync(GetDummyAccount());
            var account2 = await sut.SaveAsync(GetDummyAccount());

            // Act
            var accounts = await sut.GetAllAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(accounts.Contains(account1), Is.True, $"Expected {account1} is returned!");
                Assert.That(accounts.Contains(account2), Is.True, $"Expected {account2} is returned!");
            });
        }

        [Test]
        public void GetAsync_UnknownAccount_ExceptionIsThrown()
        {
            var sut = new AccountRepository(_dbContext);
            Assert.ThrowsAsync<FetchingFailedException>(async () => await sut.GetAsync(long.MaxValue), "Expected fictive account number is unknown!");
        }

        [Test]
        public async Task GetAsync_KnownAccount_AccountIsReturned()
        {
            // Arrange
            var sut = new AccountRepository(_dbContext);
            var account = await sut.SaveAsync(GetDummyAccount());

            // Act
            var rcvdAccount = await sut.GetAsync(account.AccountNumber);

            var comparisonResult = new CompareLogic().Compare(account, rcvdAccount);
            Assert.That(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [Test]
        public async Task SaveAsync_Account_IsPersisted()
        {
            // Arrange
            var account = GetDummyAccount();

            var sut = new AccountRepository(_dbContext);

            // Act
            var persistedAccount = await sut.SaveAsync(account);

            // Assert
            Assert.That(persistedAccount.AccountNumber, Is.Not.Null, $"Expected persisted {account} has an account number!");
        }

        [Test]
        public async Task SaveAsync_AccountProperties_AreCorrectlyPersisted()
        {
            // Arrange
            var account = GetDummyAccount();
            var sut = new AccountRepository(_dbContext);

            // Act
            var persistedAccount = await sut.SaveAsync(account);

            // Assert
            var comparisonResult = new CompareLogic().Compare(account, persistedAccount);
            Assert.That(comparisonResult.AreEqual, Is.True, comparisonResult.DifferencesString);
        }

        [Test]
        public async Task UpdateAsync_KnownAccount_AccountEntityIsUpdated()
        {
            // Arrange
            var sut = new AccountRepository(_dbContext);
            var account = await sut.SaveAsync(GetDummyAccount());

            // Act
            account.Name = "UpdatedAccount";
            var updatedAccount = await sut.UpdateAsync(account.AccountNumber, account);

            // Verify
            var comparisonResult = new CompareLogic().Compare(account, updatedAccount);
            Assert.That(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [Test]
        public void UpdateAsync_UnknownAccount_ThrowsException()
        {
            var sut = new AccountRepository(_dbContext);

            Assert.ThrowsAsync<PersistingFailedException>(async () => await sut.UpdateAsync(long.MaxValue, new Account()));
        }

        [Test]
        public async Task UpdateAsync_AccountNumber_CanNotBeChanged()
        {
            // Arrange
            var sut = new AccountRepository(_dbContext);
            var account = await sut.SaveAsync(GetDummyAccount());
            var accountNumber = account.AccountNumber;

            // Act
            account.AccountNumber = accountNumber + 1;
            var updatedAccount = await sut.UpdateAsync(accountNumber, account);

            // Verify
            Assert.That(updatedAccount.AccountNumber, Is.EqualTo(account.AccountNumber), "Account number must not be changed!");
        }

        [Test]
        public async Task UpdateAsync_UnknownAccountNumber_ExceptionIsThrown()
        {
            // Arrange
            var sut = new AccountRepository(_dbContext);
            var account = await sut.SaveAsync(GetDummyAccount());
            account = await sut.SaveAsync(account);

            // Act & Assert
            var accountNumber = account.AccountNumber + 1;
            Assert.ThrowsAsync<PersistingFailedException>(async () => await sut.UpdateAsync(accountNumber, account));
        }

        private Account GetDummyAccount()
        {
            return new Account
            {
                Name = "TestAccount",
                Users = new List<User> { new User { Name = "TestUser" } },
                HashedPin = "Hash",
                Credit = 10,
                Dispo = 1000
            };
        }
    }
}