using Microsoft.OpenApi.Models;

namespace Movies.Api.Extensions;

public static class ApiSwaggerServiceCollectionExtensions
{
  public static IServiceCollection AddSwagger(this IServiceCollection services)
  {
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "Movies.Api", Version = "v1" });

        // Add the JWT Bearer token configuration
            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token in the format 'Bearer {token}'",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
    });

    return services;
  }
}
