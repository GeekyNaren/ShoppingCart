namespace ShoppingCart.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //try
            //{
            //    await _next(context);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "An unhandled exception occurred.");
            //    context.Response.StatusCode = 500;
            //    context.Response.ContentType = "application/json";
            //    var response = new { message = "An unexpected error occurred. Please try again later." };
            //    await context.Response.WriteAsJsonAsync(response);
            //}
            _logger.LogInformation("Request: {Method} {Path}", context.Request.Method, context.Request.Path);
            await _next(context); // pass control to next middleware
            _logger.LogInformation("Response: {StatusCode}", context.Response.StatusCode);
        }
    }
}
