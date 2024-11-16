using System;
using Movies.Api.Auth;

namespace Movies.Api.Middleware;

public class AuthMiddleware
{
  private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      var userIdClaim = context.User.Claims.SingleOrDefault(x => x.Type == "userId");

      Guid? userId = Guid.TryParse(userIdClaim?.Value, out var parsedId)
            ? parsedId
            : null;

      if (userId.HasValue) {
        var authContext = context.RequestServices.GetService(typeof(AuthContext)) as AuthContext;
        authContext?.SetUserId(userId.Value);
      }

      await _next(context);
    }
}
