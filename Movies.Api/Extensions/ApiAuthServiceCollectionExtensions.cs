using Movies.Api.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Movies.Api.Extensions;

public static class ApiCollectionExtensions
{
  public static IServiceCollection AddAuth(this IServiceCollection services, IConfigurationManager config)
  {
    services.AddScoped<AuthContext>();

    services.AddAuthentication(x =>
      {
          x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
          x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(x => 
      {
          x.TokenValidationParameters = new TokenValidationParameters
          {
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:key"]!)),
              ValidateIssuerSigningKey = true,
              ValidateLifetime = true,
              ValidIssuer = config["Jwt:Issuer"],
              ValidateIssuer = true,
              ValidAudience = config["Jwt:Audience"],
              ValidateAudience = true
          };
      });


    services.AddAuthorization(x =>
      {
          x.AddPolicy(AuthConstants.AdminUserPolicyName, 
              p => p.RequireClaim(AuthConstants.AdminUserClaimName, "true"));

          x.AddPolicy(AuthConstants.TrustedMemberPolicyName,
              p => p.RequireAssertion(c =>
                  c.User.HasClaim(m => m is { Type: AuthConstants.AdminUserClaimName, Value: "true"}) ||
                  c.User.HasClaim(m => m is { Type: AuthConstants.TrustedMemberClaimName, Value: "true"})    
                  )
              );
      });

      return services;
  }
}
