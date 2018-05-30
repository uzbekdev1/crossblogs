using System;
using System.Linq;
using System.Threading.Tasks;
using crossblog.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace crossblog.Dto
{
    public class ApiResponse
    {
        public int StatusCode { get; }

        public string ErrorMessage { get; }

        public bool IsSuccess { get; set; }

        public object Result { get; set; }

        public ApiResponse() : this(StatusCodes.Status204NoContent)
        {
        }

        public ApiResponse(int statusCode, string message = "")
        {
            StatusCode = statusCode;
            ErrorMessage = string.IsNullOrWhiteSpace(message) ? GetDefaultMessageForStatusCode(statusCode) : message;
        }

        public static ApiResponse BadRequest(ModelStateDictionary modelState)
        {

            return new ApiResponse(StatusCodes.Status400BadRequest, modelState.GetErrors());
        }

        public static ApiResponse BadRequest(string message = "")
        {
            return new ApiResponse(StatusCodes.Status400BadRequest, message);
        }

        public static ApiResponse Ok(object result = null)
        {
            return new ApiResponse(StatusCodes.Status200OK)
            {
                Result = result,
                IsSuccess = true
            };
        }

        public static ApiResponse NotFound(string message = "")
        {
            return new ApiResponse(StatusCodes.Status404NotFound, message);
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            var msg = "";

            switch (statusCode)
            {
                case StatusCodes.Status200OK:
                    msg = statusCode + ": OK";
                    break;

                case StatusCodes.Status400BadRequest:
                    msg = statusCode + ": Bad request";
                    break;

                case StatusCodes.Status404NotFound:
                    msg = statusCode + ": Resource not found";
                    break;

                case StatusCodes.Status500InternalServerError:
                    msg = statusCode + ": An unhandled error occurred";
                    break;
            }

            return msg;
        }

    }
}
