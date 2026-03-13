using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Blog_final_project.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userName = context.User.Identity?.IsAuthenticated == true
            ? context.User.Identity.Name
            : "Anonymous";

        var method = context.Request.Method;
        var path = context.Request.Path;

        _logger.LogInformation("User: {User} | {Method} {Path}", userName, method, path);

        await _next(context);
    }
}