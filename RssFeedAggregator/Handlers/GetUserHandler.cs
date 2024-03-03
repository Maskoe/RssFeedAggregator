using MockStuff.Db;

namespace MockStuff.Handlers;

public class GetUserHandler
{
    public static async Task<IResult> Execute(HttpContext httpContext, CancellationToken ct)
    {
        var userDto = httpContext.CurrentUser();
        return TypedResults.Ok(userDto);
    }
}