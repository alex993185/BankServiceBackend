using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankService.Backend.Persistance;
using BankService.Backend.Persistance.Entities;
using BankService.Backend.Persistance.Enums;
using BankService.Backend.Persistance.Exceptions;
using BankService.Backend.Persistance.Repositories;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace BankService.Backend.Tests.IntegrationTests
{
    /**
     * <summary>
     * This test class runs user repository tests against a real Postgres.
     * </summary>
     */
    [TestFixture]
    public class UserRepositoryTests
    {
        private BankServiceDbContext _dbContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            _dbContext = new PostgresDbContextFactory().CreateDbContext();
        }

       [Test]
        public void Ctor_ArgumentNull_ThrowsException()
        {
            Assert.That(() => new UserRepository(null), Throws.ArgumentNullException);
        }

        [Test]
        public async Task GetAllAsync_PersistedUsers_AreReturned()
        {
            // Arrange
            var sut = new UserRepository(_dbContext);
            var user1 = await sut.SaveAsync(new User { Name = "TestUser1" });
            var user2 = await sut.SaveAsync(new User { Name = "TestUser2" });

            // Act
            var users = await sut.GetAllAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(users.Any(u => u.CustomerNumber == user1.CustomerNumber), Is.True, $"Expected {user1} is returned!");
                Assert.That(users.Any(u => u.CustomerNumber == user2.CustomerNumber), Is.True, $"Expected {user2} is returned!");
            });
        }

        [Test]
        public void GetAsync_UnknownUser_ExceptionIsThrown()
        {
            var sut = new UserRepository(_dbContext);
            Assert.ThrowsAsync<FetchingFailedException>(async () => await sut.GetAsync(long.MaxValue), "Expected fictive customer number is unknown!");
        }

        [Test]
        public async Task GetAsync_KnownUser_UserIsReturned()
        { 
            // Arrange
            var sut = new UserRepository(_dbContext);
            var user = await sut.SaveAsync(new User());

            // Act
            var rcvdUser = await sut.GetAsync(user.CustomerNumber);

            var comparisonResult = new CompareLogic().Compare(user, rcvdUser);
            Assert.That(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [Test]
        public async Task SaveAsync_User_IsPersisted()
        {
            // Arrange
            var user = new User
            {
                FirstName = "FirstNameOfTestUser",
                Name = "TestUser",
                Accounts = new List<Account> { new Account { Name = "TestAccount", HashedPin = "Hash" } },
                Birthday = new DateTime(2020, 11, 15, 20, 04, 0),
                Gender = Gender.Male
            };

            var sut = new UserRepository(_dbContext);

            // Act
            var persistedUser = await sut.SaveAsync(user);

            // Assert
            Assert.That(persistedUser.CustomerNumber, Is.Not.Null, $"Expected persisted {user} has a customer number!");
        }

        [Test]
        public async Task SaveAsync_UserProperties_AreCorrectlyPersisted()
        {
            // Arrange
            var user = new User
            {
                FirstName = "FirstNameOfTestUser",
                Name = "TestUser",
                Accounts = new List<Account> { new Account {Name = "TestAccount", HashedPin = "Hash"} },
                Birthday = new DateTime(2020, 11, 15, 20, 04, 0),
                Gender = Gender.Male
            };

            var sut = new UserRepository(_dbContext);

            // Act
            var persistedUser = await sut.SaveAsync(user);

            // Assert
            var comparisonResult = new CompareLogic().Compare(user, persistedUser);
            Assert.That(comparisonResult.AreEqual, Is.True, comparisonResult.DifferencesString);
        }

        [Test]
        public async Task UpdateAsync_KnownUser_UserEntityIsUpdated()
        {
            // Arrange
            var sut = new UserRepository(_dbContext);
            var user = await sut.SaveAsync(new User());

            // Act
            user.Name = "UpdatedName";
            var updatedUser = await sut.UpdateAsync(user.CustomerNumber, user);

            // Verify
            var comparisonResult = new CompareLogic().Compare(user, updatedUser);
            Assert.That(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [Test]
        public void UpdateAsync_UnknownUser_ThrowsException()
        {
            var sut = new UserRepository(_dbContext);

            Assert.ThrowsAsync<PersistingFailedException>(async () => await sut.UpdateAsync(long.MaxValue, new User()));
        }

        [Test]
        public async Task UpdateAsync_CustomerNumber_CanNotBeChanged()
        {
            // Arrange
            var sut = new UserRepository(_dbContext);
            var user = await sut.SaveAsync(new User());
            var customerNumber = user.CustomerNumber;

            // Act
            user.CustomerNumber = customerNumber + 1;
            var updatedUser = await sut.UpdateAsync(customerNumber, user);

            // Verify
            Assert.That(updatedUser.CustomerNumber, Is.EqualTo(customerNumber), "Customer number must not be changed!");
        }

        [Test]
        public async Task UpdateAsync_UnknownCustomerNumber_ExceptionIsThrown()
        {
            // Arrange
            var sut = new UserRepository(_dbContext);
            var user = await sut.SaveAsync(new User());
            var customerNumber = user.CustomerNumber + 1;

            // Act & Assert
            Assert.ThrowsAsync<PersistingFailedException>(async () => await sut.UpdateAsync(customerNumber, user));
        }
    }
}