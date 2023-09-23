using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Api;

public static class BffConfigExtension
{
    public static void ConfigureBff(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment()) IdentityModelEventSource.ShowPII = true;

        builder.Services.AddBff(options => options.ManagementBasePath = "/bff");

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = "cookie";
                options.DefaultChallengeScheme = "oidc";
                options.DefaultSignOutScheme = "oidc";
            })
            .AddCookie("cookie", option =>
            {
                // Session lifetime
                option.ExpireTimeSpan = TimeSpan.FromHours(8);

                option.SlidingExpiration = true;

                option.Cookie.Name = "__Host-Orginn";

                option.Cookie.SameSite = SameSiteMode.Strict;
            })
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = builder.Configuration["OIDC:Authority"];

                options.ClientId = builder.Configuration["OIDC:ClientId"];
                options.ClientSecret = builder.Configuration["OIDC:ClientSecret"];
                options.ResponseType = builder.Configuration["OIDC:ResponseType"] ?? "code";

                // query response type is compatible with strict SameSite mode
                options.ResponseMode = "query";

                // get claims without mappings
                options.MapInboundClaims = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    // RoleClaimType = "role"
                };


                // Automatic token management
                options.SaveTokens = true;
                
                // request scopes
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("name");
                options.Scope.Add("orginn_api");
                
                // add refresh token
                options.Scope.Add("offline_access");
                
            });
    }
}