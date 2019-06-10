using System;
using Moneybox.App.Domain.Services;
using Moq;
using Xunit;

namespace Moneybox.App.Tests
{
    public class AccountTests
    {
        [Fact]
        public void CanWithdrawAmount_GivenAmountLessThanZero_ReturnFailedResult()
        {
            var sut = new AccountBuilder().Build();

            var result = sut.CanWithdrawAmount(-0.01m);

            Assert.False(result.Success);
        }

        [Fact]
        public void CanWithdrawAmount_GivenMinimumAllowedBalanceIsExceeded_ReturnFailedResult()
        {
            var sut = new AccountBuilder().WithBalance(0m).Build();

            var result = sut.CanWithdrawAmount(0.01m);

            Assert.False(result.Success);
        }

        [Fact]
        public void CanWithdrawAmount_GivenValidAmount_ReturnSuccessfulResult()
        {
            var sut = new AccountBuilder().WithBalance(0.01m).Build();

            var result = sut.CanWithdrawAmount(0.01m);

            Assert.True(result.Success);
        }

        [Fact]
        public void CanPayInAmount_GivenAmountLessThanZero_ReturnFailedResult()
        {
            var sut = new AccountBuilder().Build();

            var result = sut.CanPayInAmount(-0.01m);

            Assert.False(result.Success);
        }

        [Fact]
        public void CanPayInAmount_GivenPayInLimitIsExceeded_ReturnFailedResult()
        {
            var sut = new AccountBuilder().WithPaidIn(Account.PayInLimit).Build();

            var result = sut.CanPayInAmount(0.01m);

            Assert.False(result.Success);
        }

        [Fact]
        public void CanPayInAmount_GivenValidAmount_ReturnSuccessfulResult()
        {
            var sut = new AccountBuilder().WithPaidIn(Account.PayInLimit - 0.01m).Build();

            var result = sut.CanPayInAmount(0.01m);

            Assert.True(result.Success);
        }

        [Fact]
        public void WithdrawAmount_GivenInvalidAmount_ThrowException()
        {
            var sut = new AccountBuilder().WithBalance(0m).Build();

            Assert.Throws<InvalidOperationException>(() => sut.WithdrawAmount(0.01m));
        }

        [Fact]
        public void WithdrawAmount_GivenValidAmount_UpdateBalanceAndWithdrawnAmounts()
        {
            var sut = new AccountBuilder().WithBalance(0.02m).Build();

            sut.WithdrawAmount(0.01m);

            Assert.Equal(0.01m, sut.Balance);
            Assert.Equal(-0.01m, sut.Withdrawn);
        }

        [Fact]
        public void WithdrawAmount_GivenBalanceIsBelowLowBalanceThreshold_NotifyUser()
        {
            var notificationServiceMock = new Mock<INotificationService>();
            var user = new User(notificationServiceMock.Object);
            user.Email = "test@example.com";

            var sut = new AccountBuilder()
                .WithBalance(0.02m)
                .WithUser(user)
                .Build();

            sut.WithdrawAmount(0.01m);

            notificationServiceMock.Verify(service => service.NotifyFundsLow(user.Email));
        }

        [Fact]
        public void WithdrawAmount_GivenBalanceIsAboveLowBalanceThreshold_DoNotNotifyUser()
        {
            var notificationServiceMock = new Mock<INotificationService>();
            var user = new User(notificationServiceMock.Object);

            var sut = new AccountBuilder()
                .WithBalance(Account.LowBalanceThreshold + 0.01m)
                .WithUser(user)
                .Build();

            sut.WithdrawAmount(0.01m);

            notificationServiceMock.Verify(service => service.NotifyFundsLow(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void PayInAmount_GivenInvalidAmount_ThrowException()
        {
            var sut = new AccountBuilder().WithPaidIn(Account.PayInLimit).Build();

            Assert.Throws<InvalidOperationException>(() => sut.PayInAmount(0.01m));
        }

        [Fact]
        public void PayInAmount_GivenValidAmount_UpdateBalanceAndPaidInAmount()
        {
            var sut = new AccountBuilder().WithBalance(0).Build();

            sut.PayInAmount(0.01m);

            Assert.Equal(0.01m, sut.Balance);
            Assert.Equal(0.01m, sut.PaidIn);
        }

        [Fact]
        public void PayInAmount_GivenPayInLimitDifferenceThresholdExceeded_NotifyUser()
        {
            var notificationServiceMock = new Mock<INotificationService>();
            var user = new User(notificationServiceMock.Object);
            user.Email = "test@example.com";

            var sut = new AccountBuilder()
                .WithUser(user)
                .WithPaidIn(Account.PayInLimit - Account.PayInLimitDifferenceThreshold)
                .Build();

            sut.PayInAmount(0.01m);

            notificationServiceMock.Verify(service => service.NotifyApproachingPayInLimit(user.Email));
        }

        [Fact]
        public void PayInAmount_GivenPayInLimitDifferenceThresholdNotExceeded_DoNotNotifyUser()
        {
            var notificationServiceMock = new Mock<INotificationService>();
            var user = new User(notificationServiceMock.Object);

            var sut = new AccountBuilder()
                .WithUser(user)
                .WithPaidIn(Account.PayInLimit - Account.PayInLimitDifferenceThreshold - 0.01m)
                .Build();

            sut.PayInAmount(0.01m);

            notificationServiceMock.Verify(service => service.NotifyApproachingPayInLimit(It.IsAny<string>()), Times.Never);
        }
    }
}