using MockStuff.Db;

namespace MockStuff.Handlers;

public class CreateUserHandler
{
    public static async Task<IResult> Execute(
        HttpContext httpContext,
        AppDbContext appDbContext,
        CreateUserRequest request,
        CancellationToken ct)
    {
        var newUser = new AppUser { Name = request.Name };
        appDbContext.AppUsers.Add(newUser);

        var res = await appDbContext.SaveChangesAsync();
        if (res == 0)
            return TypedResults.Problem("Idk whats going on here");

        var userDto = new UserDto(newUser.Id, newUser.CreatedAt, newUser.UpdatedAt, newUser.Name, newUser.ApiKey);

        return TypedResults.Ok(userDto);
    }
}

public record CreateUserRequest(string Name);

public record UserDto(Guid Id, DateTime CreatedAt, DateTime UpdatedAt, string Name, string ApiKey);