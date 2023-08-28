using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

using sodoff.Model;
using sodoff.Controllers.Common;

namespace sodoff.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class VikingSession : Attribute, IAsyncActionFilter {
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
        DBContext ctx = ((AchievementController)context.Controller).ctx;

        foreach (var a in context.ActionArguments.Keys) {
            Console.WriteLine(a);
        }

        if (!context.HttpContext.Request.Form.ContainsKey("apiToken")) {
            context.Result = new UnauthorizedObjectResult("Unauthorized") { StatusCode = 403 };
            return;
        }

        Session? session = ctx.Sessions.FirstOrDefault(x => x.ApiToken == context.HttpContext.Request.Form["apiToken"].ToString());
        if (session?.VikingId is null) {
            context.Result = new UnauthorizedObjectResult("Unauthorized") { StatusCode = 403 };
            return;
        }

        // NOTE: we can't refer to session.Viking here, because it may cause to ignore modifications from the threads we are waiting for
        //       we can use session.Viking only after vikingMutex.WaitOne()

        Mutex vikingMutex = new Mutex(false, "SoDOffViking:" + session.VikingId);
        try {
            vikingMutex.WaitOne();
            context.ActionArguments["viking"] = session.Viking;
            await next();
        } finally {
            vikingMutex.ReleaseMutex();
        }
    }
}
