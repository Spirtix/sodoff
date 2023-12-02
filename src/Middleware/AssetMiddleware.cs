using Microsoft.Extensions.Options;
using sodoff.Configuration;

namespace sodoff.Middleware;
public class AssetMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IOptions<AssetServerConfig> config;

    public AssetMiddleware(RequestDelegate next, IOptions<AssetServerConfig> config)
    {
        _next = next;
        this.config = config;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Connection.LocalPort == config.Value.Port)
            await GetAssetAsync(context);
        else
            await _next(context);
    }

    private async Task GetAssetAsync(HttpContext context)
    {
        string path = context.Request.Path;

        if (path is null || !string.IsNullOrEmpty(config.Value.URLPrefix) && !path.StartsWith("/" + config.Value.URLPrefix) || config.Value.Mode == AssetServerMode.None) {
            context.Response.StatusCode = 400;
            return;
        }

        string assetPath = path.Remove(0, config.Value.URLPrefix.Length + 1);

        string localPath = GetLocalPath("assets/" + assetPath);

        if (localPath == string.Empty) {
            if (config.Value.Mode == AssetServerMode.Partial)
                await GetRemoteAsset(context, assetPath);
            else
                context.Response.StatusCode = 404;
        }
        else {
            context.Response.Headers["Content-Type"] = "application/octet-stream";
            await context.Response.SendFileAsync(Path.GetFullPath(localPath));
        }
    }

    private async Task GetRemoteAsset(HttpContext context, string path)
    {
        HttpClient client = new HttpClient();
        try {
            var response = await client.GetAsync(config.Value.ProviderURL + path);
            string? contentType = response.Content.Headers.ContentType?.MediaType;

            if (contentType is null) {
                context.Response.StatusCode = 404;
            }
            else if (contentType.StartsWith("application/octet-stream") || contentType.StartsWith("image/jpeg")) {
                context.Response.Headers["Content-Type"] = contentType;
                await response.Content.CopyToAsync(context.Response.Body);
            }
            else {
                context.Response.ContentType = contentType;
                await context.Response.WriteAsync(await response.Content.ReadAsStringAsync());
            }
        }
        catch (Exception) {
            context.Response.StatusCode = 404;
        }
    }

    private string GetLocalPath(string path)
    {
        if (File.Exists(path)) return path;

        string[] qualityTiers = { "/High/", "/Mid/", "/Low/" };

        if (config.Value.SubstituteMissingLocalAssets)
        {
            foreach (var tier in qualityTiers)
            {
                if (path.Contains(tier))
                {
                    foreach (var otherTier in qualityTiers)
                    {
                        if (otherTier != tier)
                        {
                            string otherPath = path.Replace(tier, otherTier);
                            if (File.Exists(otherPath)) return otherPath;
                        }
                    }
                }
            }
        }

        return string.Empty;
    }
}
