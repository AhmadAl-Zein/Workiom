using System.Text;

namespace Workiom.Common
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // Log the request url
            var requestUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
            _logger.LogInformation($"Request URL: {requestUrl}");

            // Log Client IP
            var clientIP = context.Connection.RemoteIpAddress?.ToString();
            _logger.LogInformation($"Client IP: {clientIP}");

            // Log the request body
            var requestBodyStream = new MemoryStream();
            await context.Request.Body.CopyToAsync(requestBodyStream);
            _logger.LogInformation($"Request Body: {Encoding.UTF8.GetString(requestBodyStream.ToArray())}");

            // Replace the request stream with the new memory stream so that we can read the request body again later
            requestBodyStream.Seek(0, SeekOrigin.Begin);
            context.Request.Body = requestBodyStream;

            // Log the response body
            var originalResponseBodyStream = context.Response.Body;
            var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context);

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
            _logger.LogInformation($"Response Body: {responseBody}");

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalResponseBodyStream);
        }
    }
}
