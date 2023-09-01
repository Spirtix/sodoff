using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace sodoff.Utils;

public class LogRequestOnError : IAsyncExceptionFilter {
    public async Task  OnExceptionAsync(ExceptionContext context) {
        Console.WriteLine(string.Format("Exception caused by: {0}", context.HttpContext.Request.Path));
        foreach (var field in context.HttpContext.Request.Form)
            Console.WriteLine(string.Format("  {0}", field).Replace("\r", "\n"));
    }
}
