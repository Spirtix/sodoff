using Microsoft.AspNetCore.Mvc.Filters;
using sodoff.Util;
using System.Text;

namespace sodoff.Attributes;
public class EncryptResponse : Attribute, IAsyncResultFilter {
    const string key = "56BB211B-CF06-48E1-9C1D-E40B5173D759";

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next) {
        var originalBodyStream = context.HttpContext.Response.Body;

        using (var memoryStream = new MemoryStream()) {
            context.HttpContext.Response.Body = memoryStream;

            await next();

            // Read body and encrypt
            memoryStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();
            var result = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<string>{TripleDES.EncryptUnicode(responseBody, key)}</string>";
            context.HttpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(result);
            var newBody = new MemoryStream(Encoding.UTF8.GetBytes(result));
            // Override body with encrypted data
            await newBody.CopyToAsync(originalBodyStream);
        }
    }
}
