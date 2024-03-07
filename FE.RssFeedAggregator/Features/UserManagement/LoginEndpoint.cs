using System.Security.Claims;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using MockStuff.Db;

namespace FE.RssFeedAggregator.Features;

public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
{
    private readonly AppDbContext context;

    public LoginEndpoint(AppDbContext context)
    {
        this.context = context;
    }

    public override void Configure()
    {
        AllowAnonymous();
        Post("identity/login");
    }

    public override async Task<LoginResponse> ExecuteAsync(LoginRequest req, CancellationToken ct)
    {
        var user = await context.AppUsers.FirstOrDefaultAsync(x => x.ApiKey == req.ApiKey);
        if (user is null)
            ThrowError("Couldn't log you in", 404);

        var jwt = JwtBearer.CreateToken(options =>
        {
            options.SigningKey = "MySuperSecretJwtSecretDontTellAnyone";
            options.User.Claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            options.User.Claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.Name));
            options.ExpireAt = DateTime.UtcNow.AddDays(7);
        });

        return new LoginResponse(jwt, user.Name, user.ApiKey);
    }
}

public record LoginRequest(string ApiKey);

public record LoginResponse(string JwtToken, string Name, string ApiKey);