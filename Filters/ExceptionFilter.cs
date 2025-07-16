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
                case EntityNotFoundException entityNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = entityNotFoundException.Message;
                    break;

                case UnauthorizedAccessException unauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = unauthorizedAccessException.Message;
                    break;

                case DbUpdateConcurrencyException dbUpdateConcurrencyException:
                    statusCode = HttpStatusCode.Conflict;
                    message = dbUpdateConcurrencyException.Message;
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
