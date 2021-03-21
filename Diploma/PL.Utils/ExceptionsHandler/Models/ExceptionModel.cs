using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Common.Infrastructure.Exceptions;

namespace PL.Utils.ExceptionsHandler.Models
{
    public class ExceptionModel
    {
        [JsonPropertyName("statusCode")]
        public HttpStatusCode StatusCode { get; private set; }

        [JsonPropertyName("message")]
        public string Message { get; private set; }

        public ExceptionModel(string message, HttpStatusCode statusCode)
        {
            Message = message;
            StatusCode = statusCode;
        }

        public static ExceptionModel FromException(Exception ex)
        {
            return new ExceptionModel(ex.Message, HttpStatusCode.InternalServerError);
        }

        public static ExceptionModel FromException(DiplomaApiExpection ex)
        {
            return new ExceptionModel(ex.Message, ex.ResultStatusCode);
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
