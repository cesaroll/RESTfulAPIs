using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Movies.Db.Extensions;

public static class MoviesDbExtensions
{
  public static IServiceCollection AddDatabase(this IServiceCollection services, Func<string> getConnection)
  {
    var connString = getConnection();
    services.AddDbContext<MoviesDbContext>(options => options.UseNpgsql(connString));
    return services;
  }

  public static async Task InitializeDbAsync(this IServiceProvider serviceProvider)
  {
    using var scope = serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<MoviesDbContext>();
    await context.Database.MigrateAsync();
  }
}
