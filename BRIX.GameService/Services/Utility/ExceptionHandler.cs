using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BRIX.GameService.Services.Utility
{
    public class ProblemException(string detail) : Exception
    {
        public string Detail { get; set; } = detail;
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
                    problem = new()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = problemException.Message,
                        Detail = problemException.Detail,
                        Type = "Bad Request"
                    };
                }
                else
                {
                    problem = new()
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
