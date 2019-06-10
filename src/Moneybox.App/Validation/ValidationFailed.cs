namespace Moneybox.App.Validation
{
    public class ValidationFailed : ValidationResult
    {
        public ValidationFailed(string reason) : base(false, reason)
        {
            
        }
    }
}