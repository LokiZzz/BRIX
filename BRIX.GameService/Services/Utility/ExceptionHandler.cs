using Microsoft.AspNetCore.Diagnostics;

namespace BRIX.GameService.Services.Utility
{
    public class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _logger = logger;

        public ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogError(exception.Message);

                return ValueTask.FromResult(true);
            }
            catch
            {
                return ValueTask.FromResult(false);
            }
        }
    }
}
