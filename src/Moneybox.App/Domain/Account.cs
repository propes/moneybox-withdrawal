using System;
using Moneybox.App.Validation;

namespace Moneybox.App
{
    public class Account
    {
        // These should come from config but I'm assuming for this exercise it's
        // ok to leave these as private constants.
        public const decimal PayInLimit = 4000m;
        public const decimal MinimumAllowedBalance = 0m;
        public const decimal LowBalanceThreshold = 500m;
        public const decimal PayInLimitDifferenceThreshold = 500m;

        // I have made this class immutable so that it is protected from outside changes.
        // In doing so I'm assuming that the account repository can create account objects
        // using the constructor.
        public Account(Guid id, User user, decimal balance, decimal withdrawn, decimal paidIn)
        {
            this.Id = id;
            this.User = user;
            this.Balance = balance;
            this.Withdrawn = withdrawn;
            this.PaidIn = paidIn;
        }

        public Guid Id { get; private set; }

        public User User { get; private set; }

        public decimal Balance { get; private set; }

        public decimal Withdrawn { get; private set; }

        public decimal PaidIn { get; private set; }

        public ValidationResult CanWithdrawAmount(decimal amount)
        {
            if (amount < 0m)
            {
                return new ValidationFailed("Amount must be greater than or equal to zero");
            }

            if (Balance - amount < MinimumAllowedBalance)
            {
                return new ValidationFailed("Insufficient funds to make transfer");
            }

            return new ValidationSuccess();
        }

        public ValidationResult CanPayInAmount(decimal amount)
        {
            if (amount < 0m)
            {
                return new ValidationFailed("Amount must be greater than or equal to zero");
            }

            if (PaidIn + amount > PayInLimit)
            {
                return new ValidationFailed("Account pay in limit reached");
            }

            return new ValidationSuccess();
        }

        public void WithdrawAmount(decimal amount)
        {
            var validation = CanWithdrawAmount(amount);
            if (!validation.Success)
            {
                throw new InvalidOperationException(validation.Reason);
            }

            Balance -= amount;
            Withdrawn -= amount;

            if (Balance < LowBalanceThreshold)
            {
                User.NotifyFundsLow();
            }
        }

        public void PayInAmount(decimal amount)
        {
            var validation = CanPayInAmount(amount);
            if (!validation.Success)
            {
                throw new InvalidOperationException(validation.Reason);
            }

            Balance += amount;
            PaidIn += amount;

            if (PayInLimit - PaidIn < PayInLimitDifferenceThreshold)
            {
                User.NotifyApproachingPayInLimit();
            }
        }
    }
}
