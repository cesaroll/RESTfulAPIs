using Microsoft.EntityFrameworkCore;
using Movies.Db.Entities;

namespace Movies.Db;

public class MoviesDbContext : DbContext
{
  public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options)
  {
  }

  public DbSet<Movie> Movies { get; set; }
  public DbSet<Genre> Genres { get; set; }

  override protected void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Genre>()
      .HasKey(e => new { e.MovieId, e.Name });
    
    modelBuilder.Entity<Genre>()
      .HasOne(e => e.Movie)
      .WithMany(e => e.Genres)
      .HasForeignKey(e => e.MovieId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
