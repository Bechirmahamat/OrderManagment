namespace Api.Middlewares
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 403)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"error\": \"Access denied. You do not have permission to access this resource.\"}");
            }
        }
    }
}
