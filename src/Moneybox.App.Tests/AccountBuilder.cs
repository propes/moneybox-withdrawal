using System;
using Moneybox.App.Domain.Services;
using Moq;

namespace Moneybox.App.Tests
{
    public class AccountBuilder
    {
        private User user = new User(new Mock<INotificationService>().Object);

        private decimal balance = 0m;
        private decimal withdrawn = 0m;
        private decimal paidIn = 0m;

        public Account Build()
        {
            return new Account(Guid.NewGuid(), this.user, this.balance, this.withdrawn, this.paidIn);
        }

        public AccountBuilder WithBalance(decimal balance)
        {
            this.balance = balance;

            return this;
        }

        public AccountBuilder WithPaidIn(decimal paidIn)
        {
            this.paidIn = paidIn;

            return this;
        }

        public AccountBuilder WithUser(User user)
        {
            this.user = user;

            return this;
        }
    }
}