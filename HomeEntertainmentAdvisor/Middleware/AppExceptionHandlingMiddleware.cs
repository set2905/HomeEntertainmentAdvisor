using System.Net;

namespace HomeEntertainmentAdvisor.Middleware
{
    public class AppExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger _logger;
        public AppExceptionHandlingMiddleware(ILogger<AppExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync($"{ex.Message}\n{ex.StackTrace}");
                // await context.Response.WriteAsync("Internal Server Error. Please, contact us at setoff2905@yandex.ru.");
                _logger.Log(LogLevel.Error, $"Message: {Environment.NewLine + ex.Message} {Environment.NewLine}Trace: {Environment.NewLine + ex.StackTrace ?? string.Empty}");
            }

        }
    }
}
