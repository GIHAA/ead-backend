﻿/*
 * File: AccountLockedException.cs
 * Project: Healthy Bites.Exceptions
 * Description: Custom exception class representing an account lock scenario. This exception is thrown when a user's 
 *              account is locked. It provides constructors to pass a default message, a custom message, or a message 
 *              with an inner exception.
 */


namespace HealthyBites.Exceptions
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
