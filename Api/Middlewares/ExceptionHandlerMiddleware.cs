using System.Net;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace Api.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var responseModel = new ErrorResponse();
                var statusCode = HttpStatusCode.InternalServerError;

                switch (error)
                {
                    case UnauthorizedAccessException:
                        statusCode = HttpStatusCode.Unauthorized;
                        responseModel.Message = "Unauthorized access. Please authenticate.";
                        break;

                    case SecurityTokenExpiredException:
                        statusCode = HttpStatusCode.Unauthorized;
                        responseModel.Message = "Token has expired.";
                        break;

                    case SecurityTokenException:
                        statusCode = HttpStatusCode.Unauthorized;
                        responseModel.Message = "Invalid token.";
                        break;

                    //case ValidationException ve:
                    //    statusCode = HttpStatusCode.BadRequest;
                    //    responseModel.Message = "Validation failed.";
                    //    responseModel.Errors = ve.Errors;
                    //    break;

                    case KeyNotFoundException:
                        statusCode = HttpStatusCode.NotFound;
                        responseModel.Message = "Requested resource was not found.";
                        break;

                    case NotImplementedException:
                        statusCode = HttpStatusCode.NotImplemented;
                        responseModel.Message = "This feature is not implemented.";
                        break;

                    case ArgumentException ae:
                        statusCode = HttpStatusCode.BadRequest;
                        responseModel.Message = $"Invalid input: {ae.Message}";
                        break;

                    default:
                        responseModel.Message = "An unexpected error occurred.";
                        break;
                }

                response.StatusCode = (int)statusCode;
                responseModel.StatusCode = (int)statusCode;
                responseModel.DetailsUrl = GetDocsUrl(statusCode);

                LogException(error, statusCode);

                var json = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await response.WriteAsync(json);
            }
        }

        private void LogException(Exception ex, HttpStatusCode status)
        {
            if ((int)status >= 500)
                _logger.LogError(ex, "Unhandled server error: {Message}", ex.Message);
            else
                _logger.LogWarning(ex, "Client-side error: {Message}", ex.Message);
        }

        private string GetDocsUrl(HttpStatusCode statusCode)
        {
            return $"https://datatracker.ietf.org/doc/html/rfc9110#section-{(int)statusCode}";
        }
    }

    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = "An error occurred.";
        public string? DetailsUrl { get; set; }
        public IEnumerable<ValidationFailure>? Errors { get; set; }
    }
}
