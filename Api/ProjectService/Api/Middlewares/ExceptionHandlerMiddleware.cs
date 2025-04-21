
using CRM.Data.Common.Exceptions;
using JetBrains.Annotations;
using System.Net;

    namespace Api.Middlewares
    {
        [UsedImplicitly]
        internal class ExceptionHandlerMiddleware
        {
            private readonly RequestDelegate _next;

            public ExceptionHandlerMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext context, ILogger<ExceptionHandlerMiddleware> logger)
            {
                try
                {
                    await _next(context);

                }
                catch (Exception ex)
                {
                    var handled = await HandleExceptionAsync(context, ex, logger);

                    if (!handled)
                        throw;
                }
            }

            private async Task<bool> HandleExceptionAsync(HttpContext context, Exception exception, ILogger<ExceptionHandlerMiddleware> logger)
            {
                var code = HttpStatusCode.InternalServerError;
                string result = null;

                switch (exception)
                {
                    case NotAuthenticatedException notAuthenticatedException:
                        code = HttpStatusCode.Unauthorized;
                        result = notAuthenticatedException.Message;
                        break;
                    case NotFoundException notFoundException:
                        code = HttpStatusCode.NotFound;
                        result = notFoundException.Message;
                        break;
                    case NoAccessException noAccessException:
                        code = HttpStatusCode.Forbidden;
                        result = noAccessException.Message;
                        break;
                    case ConflictException conflictException:
                        code = HttpStatusCode.Conflict;
                        result = conflictException.Message;
                        break;
                    case BadRequestException badRequestException:
                        code = HttpStatusCode.BadRequest;
                        result = badRequestException.Message;
                        break;
                    case UnprocessableEntityException unprocessableEntityException:
                        code = HttpStatusCode.UnprocessableEntity;
                        result = unprocessableEntityException.Message;
                        break;
                }

                context.Response.StatusCode = (int)code;

                if (result == null)
                {
                    return false;
                }


                await context.Response.WriteAsync(result);
                logger.LogWarning("Error message: '{ErrorMessage}'", result);

                return true;
            }
        }
    }
