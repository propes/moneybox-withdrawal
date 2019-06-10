using Moneybox.App.DataAccess;
using System;

namespace Moneybox.App.Features
{
    public class WithdrawMoney
    {
        private IAccountRepository accountRepository;

        public WithdrawMoney(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public void Execute(Guid fromAccountId, decimal amount)
        {
            var from = this.accountRepository.GetAccountById(fromAccountId);

            // Will throw an exception if the action is invalid.
            from.WithdrawAmount(amount);

            this.accountRepository.Update(from);
        }
    }
}
