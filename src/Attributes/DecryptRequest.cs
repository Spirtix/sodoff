using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using sodoff.Util;

namespace sodoff.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class DecryptRequest : Attribute, IAsyncActionFilter {
    const string key = "56BB211B-CF06-48E1-9C1D-E40B5173D759";

    private readonly string field;

    public DecryptRequest(string field) {
        this.field = field;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
        if (context.HttpContext.Request.HasFormContentType) {
            var form = context.HttpContext.Request.Form;
            if (form.ContainsKey(field)) {
                // Get field from submitted form and decrypt
                var fieldValue = form[field].ToString();
                string result = TripleDES.DecryptUnicode(fieldValue, key);
                var dict = form.ToDictionary(x => x.Key, x => x.Value);
                dict[field] = result;
                var updatedForm = new FormCollection(dict);
                // Update the form in the request
                context.HttpContext.Request.Form = updatedForm;
            }
        }

        await next();
    }
}