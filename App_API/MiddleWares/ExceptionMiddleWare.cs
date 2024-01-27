using App_API.Exceptions;
using System.Runtime.InteropServices;

namespace App_API.MiddleWares
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleWare(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (MyException excepion)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = excepion.StatusCode;
                var error = new
                {
                    Status = excepion.StatusCode,
                    Message = excepion.Message
                };

                await context.Response.WriteAsJsonAsync(error);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var error = new
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = "There is something wrong!"
                };

                await context.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
