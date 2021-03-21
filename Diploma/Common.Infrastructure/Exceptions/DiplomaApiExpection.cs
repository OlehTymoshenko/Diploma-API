using System;
using System.Net;

namespace Common.Infrastructure.Exceptions
{
    public class DiplomaApiExpection : Exception
    {
        public HttpStatusCode ResultStatusCode { get; private set; }

        public DiplomaApiExpection(string message, HttpStatusCode statusCode) : base(message)
        {
            ResultStatusCode = statusCode;
        }

        public DiplomaApiExpection(string message, HttpStatusCode statusCode, Exception innerException) : base(message, innerException)
        {
            ResultStatusCode = statusCode;
        }

    }
}
