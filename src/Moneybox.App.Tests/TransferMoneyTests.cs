using System;
using Moneybox.App.DataAccess;
using Moneybox.App.Features;
using Moq;
using Xunit;

namespace Moneybox.App.Tests
{
    public class TransferMoneyTests
    {
        [Fact]
        public void Execute_GivenInvalidAmount_ThrowException()
        {
            var fromAccount = new AccountBuilder().WithBalance(0).Build();
            var toAccount = new AccountBuilder().Build();
            var amount = 0.01m;

            var mockRepository = new Mock<IAccountRepository>();
            mockRepository
                .Setup(repo => repo.GetAccountById(fromAccount.Id))
                .Returns(fromAccount);

            mockRepository
                .Setup(repo => repo.GetAccountById(toAccount.Id))
                .Returns(toAccount);

            var sut = new TransferMoney(mockRepository.Object);

            Assert.Throws<InvalidOperationException>(() => sut.Execute(fromAccount.Id, toAccount.Id, amount));
        }

        [Fact]
        public void Executive_GivenValidAmount_UpdateAccounts()
        {
            var fromAccount = new AccountBuilder().WithBalance(0.01m).Build();
            var toAccount = new AccountBuilder().WithBalance(0m).Build();
            var amount = 0.01m;

            var mockRepository = new Mock<IAccountRepository>();
            mockRepository
                .Setup(repo => repo.GetAccountById(fromAccount.Id))
                .Returns(fromAccount);

            mockRepository
                .Setup(repo => repo.GetAccountById(toAccount.Id))
                .Returns(toAccount);

            var sut = new TransferMoney(mockRepository.Object);

            sut.Execute(fromAccount.Id, toAccount.Id, amount);

            mockRepository.Verify(repo => repo.Update(It.Is<Account>(account =>
                account.Id == fromAccount.Id &&
                account.Balance == 0m)));

            mockRepository.Verify(repo => repo.Update(It.Is<Account>(account =>
                account.Id == toAccount.Id &&
                account.Balance == 0.01m)));
        }
    }
}