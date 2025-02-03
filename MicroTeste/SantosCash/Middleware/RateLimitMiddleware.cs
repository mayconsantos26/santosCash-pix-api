using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Threading.Tasks;

public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDistributedCache _cache;

    public RateLimitMiddleware(RequestDelegate next, IDistributedCache cache)
    {
        _next = next;
        _cache = cache;
    }

    public async Task Invoke(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString();
        var cacheKey = $"RateLimit_{ip}";
        var requestCount = await _cache.GetStringAsync(cacheKey);

        int count = string.IsNullOrEmpty(requestCount) ? 0 : int.Parse(requestCount);
        if (count >= 5)
        {
            context.Response.StatusCode = 429;
            await context.Response.WriteAsync("Muitas requisições. Tente novamente mais tarde.");
            return;
        }

        count++;
        await _cache.SetStringAsync(cacheKey, count.ToString(), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
        });

        await _next(context);
    }
}
