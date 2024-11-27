using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Movies.App.Health;
using Movies.App.Services;
using Movies.App.Validators;

namespace Movies.App.Extensions;

public static class ApplicationServiceCollectionExtensions
{
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    services.AddScoped<IMovieRatingsService, MovieRatingsService>();
    services.AddScoped<IMoviesService, MoviesService>();

    services.AddValidatorsFromAssemblyContaining<MovieValidator>(ServiceLifetime.Scoped);
    services.AddValidatorsFromAssemblyContaining<GetAllMoviesOptionsValidator>(ServiceLifetime.Singleton);

    services.AddScoped<IDatabaseHealthCheck, DatabaseHealthCheck>();

    return services;
  }
}
