namespace PruebaTecnicaBackAPI.Middleware
{
    public class ApiKeyMiddleware(RequestDelegate next, IConfiguration config)
    {
        private const string ApiKeyHeader = "X-API-KEY";

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(ApiKeyHeader, out var receivedKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "API Key requerida." });
                return;
            }

            var validKey = config["ApiKey"];
            if (receivedKey != validKey)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "API Key inválida." });
                return;
            }

            await next(context);
        }
    }
}
