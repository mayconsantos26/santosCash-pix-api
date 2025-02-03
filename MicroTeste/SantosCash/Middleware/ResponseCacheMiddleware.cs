using Microsoft.Extensions.Caching.Distributed;

public class ResponseCacheMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDistributedCache _cache;

    public ResponseCacheMiddleware(RequestDelegate next, IDistributedCache cache)
    {
        _next = next;
        _cache = cache;
    }

    public async Task Invoke(HttpContext context)
    {
        var cacheKey = $"Response_{context.Request.Path}";
        var cachedResponse = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedResponse))
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(cachedResponse);
            return;
        }

        var originalBodyStream = context.Response.Body;
        using var newBodyStream = new MemoryStream();
        context.Response.Body = newBodyStream;

        await _next(context);

        newBodyStream.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(newBodyStream).ReadToEndAsync();
        newBodyStream.Seek(0, SeekOrigin.Begin);

        await newBodyStream.CopyToAsync(originalBodyStream);

        await _cache.SetStringAsync(cacheKey, responseBody, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });
    }
}
