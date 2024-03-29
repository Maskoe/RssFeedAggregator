namespace FE.RssFeedAggregator.Features.UserManagement;

public class RegisterEndpoint : Endpoint<RegisterRequest, RegisterResponse>
{
    private readonly AppDbContext appDbContext;

    public RegisterEndpoint(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    public override void Configure()
    {
        AllowAnonymous();
        Post("identity/register");
    }

    public override async Task<RegisterResponse> ExecuteAsync(RegisterRequest req, CancellationToken ct)
    {
        var newUser = new AppUser { Name = req.Name, Role = req.Role };
        appDbContext.AppUsers.Add(newUser);

        await appDbContext.SaveChangesAsync();

        var userDto = new UserDto(newUser.Id, newUser.CreatedAt, newUser.UpdatedAt, newUser.Name, newUser.ApiKey);

        return new RegisterResponse(userDto);
    }
}

public record RegisterRequest(string Name, string Role);

public record RegisterResponse(UserDto User);

public record UserDto(Guid Id, DateTime CreatedAt, DateTime UpdatedAt, string Name, string ApiKey);