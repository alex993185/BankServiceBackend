using BankServiceBackend.Controllers;
using BankServiceBackend.Entities;
using BankServiceBackend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankServiceBackend.Tests.IntegrationTests
{
    [TestFixture]
    public class AccountsControllerTests
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void Ctor_ArgumentNull_ThrowsException()
        {
            Assert.That(() => new AccountsController(null), Throws.ArgumentNullException);
        }

        [Test]
        public async Task GetAll_Accounts_AreReturned()
        {
            // Setup
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var account = new Account { AccountNumber = 1, Credit = 100, Dispo = 0, Name = "TestAccount" };
            accountRepositoryMock.Setup(ar => ar.GetAllAsync()).Returns(Task.FromResult<IEnumerable<Account>>(new List<Account> { account } ));
            var sut = new AccountsController(accountRepositoryMock.Object);

            // Act
            var accounts = (await sut.GetAll()).Value;

            // Verify
            Assert.That(accounts.Contains(account), Is.True, $"Expected {account} is returned!");
        }

        [Test]
        public async Task Save_Accounts_AreReturned()
        {
            // Setup
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var account = new Account { AccountNumber = 1, Credit = 100, Dispo = 0, Name = "TestAccount" };
            accountRepositoryMock.Setup(ar => ar.Get(It.IsAny<long>())).Returns(Task.FromResult(account));
            var sut = new AccountsController(accountRepositoryMock.Object);

            // Act
            var reroute = (await sut.Save(account)).Result;

            // Verify
            Assert.Multiple(() =>
            {
                accountRepositoryMock.Verify(ar => ar.Save(account), "Expected {account} is saved!");
                Assert.That(reroute, Is.Not.Null);
            });
        }
    }
}
