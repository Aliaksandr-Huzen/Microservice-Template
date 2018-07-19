using System;

namespace Microservice.Core.Service.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string code, string message)
            : base(message)
        {
            Code = code;
        }

        public string Code { get; }
    }
}