using BRIX.GameService.Contracts.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BRIX.GameService.Services.Utility
{
    public class ProblemException(params (string Code, string Message)[] messages) : Exception
    {
        public (string Code, string Message)[] Messages = messages;
    }

    public class ProblemExceptionHandler(
        ILogger<ProblemExceptionHandler> logger,
        IProblemDetailsService problemDetailsService) 
        : IExceptionHandler
    {
        private readonly ILogger<ProblemExceptionHandler> _logger = logger;
        private readonly IProblemDetailsService _problemDetailsService = problemDetailsService;

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogError(exception.Message);
                ProblemDetails problem;

                if (exception is ProblemException problemException)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    problem = new ProblemDetails()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = problemException.Message,
                        Type = "Bad Request",
                    };
                    problem.Extensions.TryAdd(
                        "problemDetalization", 
                        new ProblemDetalization(problemException.Messages)
                    );
                }
                else
                {
                    problem = new ProblemDetails()
                    {
                        Status = StatusCodes.Status500InternalServerError,
                        Title = "Internal Server Error",
                        Detail = "No details",
                        Type = "Internal Server Error"
                    };
                }

                await _problemDetailsService.TryWriteAsync(
                    new ProblemDetailsContext
                    {
                        HttpContext = httpContext,
                        ProblemDetails = problem
                    }
                );

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
