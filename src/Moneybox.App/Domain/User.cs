using System;
using Moneybox.App.Domain.Services;

namespace Moneybox.App
{
    public class User
    {
        private readonly INotificationService notificationService;

        public User(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public void NotifyFundsLow()
        {
            this.notificationService.NotifyFundsLow(Email);
        }

        public void NotifyApproachingPayInLimit()
        {
            this.notificationService.NotifyApproachingPayInLimit(Email);
        }
    }
}
