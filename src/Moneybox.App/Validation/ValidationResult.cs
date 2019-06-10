namespace Moneybox.App.Validation
{
    public class ValidationResult
    {
        public bool Success { get; private set; }
        public string Reason { get; private set; }

        public ValidationResult(bool success, string reason)
        {
            this.Success = success;
            this.Reason = reason;
        }
    }
}