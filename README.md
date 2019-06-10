# Moneybox Money Withdrawal

## Notes

* I have written full unit tests for the Account class but have not written tests for the other classes due to time restrictions
* For the TransferMoney and WithdrawMoney classes I have maintained the behaviour of throwing an exception if the action can't be performed. An alternative would be to return an ActionResult object with success and message fields.
* In the spirit of having rich domain objects I have put the validation logic for the account directly in the account class. An alternative would be to encapsulate this in a separate validation service which could be injected into the Account class itself or directly into the TransferMoney and WithdrawMoney services.
* There is a tight coupling between the User and Account classes because the Account class delegates the task of notifying the user to the User class. This could be broken by having an IUser interface or injecting the INotificationService directly in the Account class.
