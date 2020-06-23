using System;
using System.Collections.Generic;

namespace Sample.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public IEnumerable<string> Messages { get; set; }

        public DomainException(IEnumerable<string> messages)
        {
            Messages = messages;
        }
    }
}
