using Moneybox.App.DataAccess;
using Moneybox.App.Validation;
using System;

namespace Moneybox.App.Features
{
    public class TransferMoney
    {
        private IAccountRepository accountRepository;

        public TransferMoney(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public void Execute(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            var from = this.accountRepository.GetAccountById(fromAccountId);
            var to = this.accountRepository.GetAccountById(toAccountId);

            // Both the withdrawal and the pay in action need to be validated
            // before any action can be taken against either account.
            var withdrawalValidation = from.CanWithdrawAmount(amount);
            if (!withdrawalValidation.Success)
            {
                throw new InvalidOperationException(withdrawalValidation.Reason);
            }

            var payInValidation = to.CanPayInAmount(amount);
            if (!payInValidation.Success)
            {
                throw new InvalidOperationException(payInValidation.Reason);
            }

            from.WithdrawAmount(amount);
            to.PayInAmount(amount);

            this.accountRepository.Update(from);
            this.accountRepository.Update(to);
        }
    }
}
