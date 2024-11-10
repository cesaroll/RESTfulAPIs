using Microsoft.EntityFrameworkCore;
using Movies.Db.Entities;

namespace Movies.Db;

public class MoviesDbContext : DbContext
{
  public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options)
  {
  }

  public DbSet<MovieEntity> Movies { get; set; }
  public DbSet<GenreEntity> Genres { get; set; }

  override protected void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<GenreEntity>()
      .HasKey(e => new { e.MovieId, e.Name });
    
    modelBuilder.Entity<GenreEntity>()
      .HasOne(e => e.Movie)
      .WithMany(e => e.Genres)
      .HasForeignKey(e => e.MovieId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
