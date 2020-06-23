using System;

namespace Sample.Domain.Exceptions
{
    public class ExpiredUserException : Exception
    {
        public override string Message { get; }
        
        public ExpiredUserException(string message)
        {
            Message = message;
        }
    }
}