using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebJITDebugger.Helpers;

namespace WebJITDebugger.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            string actionName = context.ActionDescriptor.DisplayName ?? "<NULL>";
            string exceptionStack = context.Exception.StackTrace ?? "<NULL>";
            string exceptionMessage = context.Exception.Message;

            var response = new
            {
                Type = context.Exception.GetType().Name,
                ActionName = actionName,
                Message = exceptionMessage,
                StackTrace = exceptionStack,
            };

            context.Result = new JsonResult(response)
            {
                StatusCode = 400,
            };
            context.ExceptionHandled = true;
        }
    }
}
