using System.Net;
using System.Text.Json;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.DBLog;
using static UNITYPOS_API.Middlewares.Exceptions.ExceptionHelpers;


namespace UNITYPOS_API.Middlewares.Exceptions
{
    public class ErrorHandlerMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context, IUnitOfWork uow)
        {
            try
            {
                await _next(context);
            }
            catch (AppExceptions ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest, uow);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound, uow);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError, uow);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode statusCode, IUnitOfWork uow)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(new
            {
                message = ex.Message,
                errorCode = context.Response.StatusCode
            });

            await context.Response.WriteAsync(result);

            try
            {
                var errorLog = new ErrorLog
                {
                    ErrorInComponent = context.Request.Path,
                    ErrorEvent = ex.InnerException?.Message,
                    Error = ex.Message,
                    ErrorTime = DateTime.Now.ToUniversalTime(),
                    // LoggedUserName = context.User!=null && context.User.Identity != null ? context.User.Identity.Name : "",
                    LoggedUserName = context.User?.Identity?.Name ?? "Anonymous"

                };

                await uow.GenericRepository<ErrorLog>().InsertAsync(errorLog);
            }
            catch (Exception logEx)
            {
                _logger.LogError(logEx, "Failed to log error to database.");
            }
        }


    }
}
