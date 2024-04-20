using FlightAPI.Exceptions;
using FlightAPI.Models;
using Newtonsoft.Json;
using System;
using System.Net;

namespace FlightAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        public readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = new ApiResponse()
            {
                StatusCode = GetStatusCode(exception),
                Errors = [exception.Message],
                IsSuccess = false
            };

            var result = JsonConvert.SerializeObject(response);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;

            return context.Response.WriteAsync(result);
        }

        private static HttpStatusCode GetStatusCode(Exception exception)
        {
            switch (exception)
            {
                case UserNotFoundException:
                case FlightNotFoundException:
                    return HttpStatusCode.NotFound;
                case AuthenticationException:
                    return HttpStatusCode.Unauthorized;
                case FailedToCreateUserException:
                case FailedToGenerateTokenException:
                case NullFlightDataException:
                case InvalidFlightIdException:
                case InvalidPlaneIdException:
                case InvalidFlightDataException:
                    return HttpStatusCode.BadRequest;
                default:
                    return HttpStatusCode.InternalServerError;
            }
        }
    }
}
