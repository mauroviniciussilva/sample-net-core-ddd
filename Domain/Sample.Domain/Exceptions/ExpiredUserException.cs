using System;

namespace Sample.Domain.Exceptions
{
    public class ExpiredUserException : Exception
    {
        public ExceptionItemInfo ExceptionItemInfo { get; set; }
        public ExpiredUserException(ExceptionItemInfo exceptionItemInfo) : base(exceptionItemInfo.Message) => ExceptionItemInfo = exceptionItemInfo;
        public ExpiredUserException(string model, string reference, string message) : this(new ExceptionItemInfo(model, reference, message))
        {

        }
    }
}