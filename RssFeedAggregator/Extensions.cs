using System.Text.Json;
using MockStuff.Handlers;

public static class Extensions
{
    public static UserDto CurrentUser(this HttpContext httpContext)
    {
        var userJson = httpContext.Request.Headers["user"];
        var userDto = JsonSerializer.Deserialize<UserDto>(userJson);
        return userDto;
    }

    public static RouteHandlerBuilder WithAuth(this RouteHandlerBuilder builder)
    {
        builder.AddEndpointFilter<AuthFilter>();
        return builder;
    }
}