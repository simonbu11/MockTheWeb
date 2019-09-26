using System;

namespace MockTheWeb
{
    public class VerificationException : Exception
    {
        public VerificationException(string message)
            : base(message)
        {
        }
    }
}