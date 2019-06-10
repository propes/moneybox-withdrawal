using System;

namespace Moneybox.App
{
    public class Account
    {
        // These should come from config but I'm assuming for this exercise it's
        // ok to leave these as private constants.
        private const decimal PayInLimit = 4000m;
        private const decimal LowBalanceThreshold = 500m;
        private const decimal ApproachingPayInLimitThreshold = 500m;

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

        public bool CanWithdrawAmount(decimal amount)
        {
            return amount <= Balance;
        }

        public void WithdrawAmount(decimal amount)
        {
            var fromBalance = Balance - amount;
            if (fromBalance < 0m)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }
            else if (fromBalance < LowBalanceThreshold)
            {
                User.NotifyFundsLow();
            }

            Balance -= amount;
            Withdrawn -= amount;
        }

        public bool CanPayInAmount(decimal amount)
        {
            return PaidIn + amount <= PayInLimit;
        }

        public void PayInAmount(decimal amount)
        {
            var paidIn = PaidIn + amount;
            if (paidIn > PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }

            if (PayInLimit - paidIn < ApproachingPayInLimitThreshold)
            {
                User.NotifyApproachingPayInLimit();
            }

            Balance += amount;
            PaidIn += amount;
        }
    }
}
