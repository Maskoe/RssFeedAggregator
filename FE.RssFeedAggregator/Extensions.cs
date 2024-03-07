using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

public static class Extensions
{
    public static TokenUser ToTokenUser(this ClaimsPrincipal principal)
    {
        var tokenUser = new TokenUser();

        tokenUser.Id = Guid.Parse(principal.FindFirstValue(JwtRegisteredClaimNames.Sub));
        tokenUser.Name = principal.FindFirstValue(JwtRegisteredClaimNames.Name);

        return tokenUser;
    }

    public class TokenUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}