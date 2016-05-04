using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AzureBot
{
    public class NotAuthenticatedException : Exception
    {
        public NotAuthenticatedException()
            : base("Cannot access resources as you are not authenticated!")
        { }

        public NotAuthenticatedException(string message)
            : base(message)
        { }

        public NotAuthenticatedException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public NotAuthenticatedException(string format, Exception innerException, params object[] args)
            : base(String.Format(format, args), innerException)
        { }

        public NotAuthenticatedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

    }
}