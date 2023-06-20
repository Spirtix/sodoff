using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using sodoff.Schema;
using sodoff.Util;
using System.Security.Cryptography;
using System.Text;

namespace sodoff.Attributes;
public class SignResponse : Attribute, IAsyncResultFilter {
    const string key = "11A0CC5A-C4DF-4A0E-931C-09A44C9966AE";
    public async System.Threading.Tasks.Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next) {
        var originalBodyStream = context.HttpContext.Response.Body;

        try {
            using (var memoryStream = new MemoryStream()) {
                context.HttpContext.Response.Body = memoryStream;

                await next();

                memoryStream.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

                var hash = Md5.GetMd5Hash(key + responseBody);

                context.HttpContext.Response.Headers.Add("Signature", hash);

                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(originalBodyStream);
            }
        } finally {
            context.HttpContext.Response.Body = originalBodyStream;
        }
    }
}