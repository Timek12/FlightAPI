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
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception has occurred while executing the request.");
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
                case UserNotFoundException
                or FlightNotFoundException:
                    return HttpStatusCode.NotFound;
                case AuthenticationException
                or InvalidRefreshTokenException:
                    return HttpStatusCode.Unauthorized;
                case InvalidAccessTokenException:
                    return HttpStatusCode.Forbidden;
                case FailedToCreateUserException
                or FailedToGenerateTokenException
                or NullFlightDataException
                or InvalidFlightIdException
                or InvalidPlaneIdException
                or InvalidFlightDataException
                or UserAlreadyExistsException:
                    return HttpStatusCode.BadRequest;
                default:
                    return HttpStatusCode.InternalServerError;
            }
        }
    }
}
