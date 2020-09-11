using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigProject.Auth.Models
{
    public class RequestResult
    {
        public RequestResult()
        {
            IsSuccess = false;
            Message = string.Empty;
        }

        public RequestResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public RequestResult(string message)
        {
            Message = message;
        }

        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; }
    }
}
