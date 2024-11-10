using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Movies.Db;

public class MoviesDbContextFactory : IDesignTimeDbContextFactory<MoviesDbContext>
{
    public MoviesDbContext CreateDbContext(string[] args)
    {
        var connectionString = args[0];
        var optionsBuilder = new DbContextOptionsBuilder<MoviesDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return new MoviesDbContext(optionsBuilder.Options);
    }
}
