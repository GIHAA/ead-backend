namespace TechFixBackend.Exceptions
{
    public class AccountLockedException : Exception
    {
        public AccountLockedException() : base("The account is locked.")
        {
        }

        public AccountLockedException(string message) : base(message)
        {
        }

        public AccountLockedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
