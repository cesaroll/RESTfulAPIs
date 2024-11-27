using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Movies.App.Models;
using Movies.App.Services;
using Movies.Db;

namespace Movies.App.Validators;

public class MovieValidator : AbstractValidator<Movie>
{
  private readonly MoviesDbContext _dbContext;

  public MovieValidator(MoviesDbContext dbContext)
  {
    _dbContext = dbContext;

    RuleFor(x => x.Id)
      .NotEmpty();

    RuleFor(x => x.Title)
      .NotEmpty()
      .MaximumLength(100);

    RuleFor(x => x.Genres)
      .NotEmpty();
    
    RuleFor(x => x.YearOfRelease)
      .InclusiveBetween(1900, DateTime.Now.Year);

    RuleFor(x => x.Slug)
      .MustAsync(ValidateSlug)
      .WithMessage("This movie already exists in the system");
  }

  public async Task<bool> ValidateSlug(Movie movie, string slug, CancellationToken token)
  {
    var existingMovie = await _dbContext.Movies
          .AsNoTracking()
          .FirstOrDefaultAsync(m => m.Slug == slug);

    if (existingMovie is not null)
    {
      return movie.Id == existingMovie.Id; // If same then allow since it is an update
    }

    return existingMovie is null;
  }
}
