using System.Diagnostics;

namespace TodoApp.Api.Middleware
{
#pragma warning disable CA1873
    public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<RequestLoggingMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            var request = context.Request;

            _logger.LogInformation("Started handling: {Method} {Path}", request.Method, request.Path);

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();

                var statusCode = context.Response.StatusCode;
                var ellapsedMs = stopwatch.ElapsedMilliseconds;


                // For now there is no behaviour any than just different type of logging, but after scaling there might appear actions based on type of statusCode
                if (statusCode >= 400 && statusCode < 500)
                {
                    _logger.LogWarning("Request handled: {Method} {Path} -> Status code: {Status} (Client error) in {Elapsed}ms.",
                        request.Method, request.Path, statusCode, ellapsedMs);
                }
                else if (statusCode >= 500)
                {
                    _logger.LogError("Request handled: {Method} {Path} -> Status code: {Status} (Server error) in {Elapsed}ms.",
                        request.Method, request.Path, statusCode, ellapsedMs);
                }
                else
                {
                    _logger.LogInformation("Request handled: {Method} {Path} -> Status code: {Status} (Succesfull) in {Elapsed}ms.",
                        request.Method, request.Path, statusCode, ellapsedMs);
                }
            }
        }
    }
#pragma warning restore CA1873
}
