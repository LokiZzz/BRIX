using BRIX.GameService.Contracts.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BRIX.GameService.Services.Utility
{
    /// <summary>
    /// Исключение-проблема, которое будет перехвачено глобальной обработкой исключений.
    /// Из него будет собран насыщенный унифицированный ответ с ошибками.
    /// </summary>
    public class ProblemException((string Code, string Message)[] problems) : Exception
    {
        public ProblemException(string code, string message) : this([(code, message)]) { }

        public (string Code, string Message)[] Problems { get; init; } = problems;
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
                        "detalization", 
                        new ProblemDetalization(problemException.Problems)
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
