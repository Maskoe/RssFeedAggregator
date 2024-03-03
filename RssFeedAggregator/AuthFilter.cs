using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MockStuff.Db;
using MockStuff.Handlers;

public class AuthFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var authHeader = context.HttpContext.Request.Headers.Authorization;
        var apiKey = authHeader.ToString()?.Split(" ").ElementAtOrDefault(1);

        var dbContext = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();

        var userFromDb = await dbContext.AppUsers.FirstOrDefaultAsync(x => x.ApiKey == apiKey);
        if (userFromDb is null)
            return Results.Unauthorized();

        var userDto = new UserDto(userFromDb.Id, userFromDb.CreatedAt, userFromDb.UpdatedAt, userFromDb.Name, userFromDb.ApiKey);

        context.HttpContext.Request.Headers["user"] = JsonSerializer.Serialize(userDto);

        return await next(context);
    }
}