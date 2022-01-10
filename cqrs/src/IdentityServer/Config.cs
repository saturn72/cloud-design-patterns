using IdentityServer4.Models;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<ApiScope> ApiScopes =>
             new List<ApiScope>
             {
            new ApiScope("api1", "My API")
             };
        public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "command-api-swagger",
                ClientName = "cqrs command api for swagger",
                RequireConsent = false,
                RequireClientSecret = false,
                RequirePkce = true,
                RedirectUris = new[] { "https://command.saturn72.com:443/oauth2-redirect.html" },
                AllowedCorsOrigins = new[] { "https://command.saturn72.com:443" },
                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                PostLogoutRedirectUris = new[] { "https://notused"},
                AllowedScopes = { "openid","profile", "api" }
            }
        };
    }
}