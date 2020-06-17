using System;

namespace Sample.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public ExceptionItemInfo ExceptionItemInfo { get; set; }
        public DomainException(ExceptionItemInfo exceptionItemInfo) : base(exceptionItemInfo.Message) => this.ExceptionItemInfo = exceptionItemInfo;
        public DomainException(string model, string reference, string message, params object[] arguments) : this(new ExceptionItemInfo(model, reference, message, arguments))
        {

        }
    }
}
