using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WorkoutTracker.Api.Exceptions;

namespace WorkoutTracker.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            // Default details for an unhandled exception
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "An unexpected internal server error occurred.";

            switch (context.Exception)
            {
                case EntityNotFoundException ex:
                    statusCode = HttpStatusCode.NotFound;
                    message = ex.Message;
                    break;

                case UnauthorizedActionException ex:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = ex.Message;
                    break;

                case Exception ex
                    when ex is DbUpdateConcurrencyException or EntityAlreadyExistsException:
                    statusCode = HttpStatusCode.Conflict;
                    message = ex.Message;
                    break;

                case CreateUserAccountException ex:
                    statusCode = HttpStatusCode.BadRequest;
                    message = ex.Message;
                    break;

                default:
                    _logger.LogError("An unhandled exception has occurred: {Message}", context.Exception.Message);
                    break;
            }


            // Create the JSON response
            var errorResponse = new
            {
                Title = statusCode.ToString(),
                Status = (int)statusCode,
                Detail = message
            };

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = (int)statusCode
            };

            // Mark the exception as handled
            context.ExceptionHandled = true;
        }
    }
}
