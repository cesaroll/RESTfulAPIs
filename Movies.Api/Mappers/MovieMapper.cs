using Movies.App.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Mappers;

public static class MovieMapper
{
  public static Movie MapToMovie(this CreateMovieRequest request)
  {
    var id = Guid.NewGuid();
    var movie = new Movie()
    {
      Id = id,
      Title = request.Title,
      YearOfRelease = request.YearOfRelease,
      Genres = request.Genres.MapToGenres(id)
    };

    return movie;
  }

  public static Movie MapToMovie(this UpdateMovieRequest request, Guid id) => new()
  {
    Id = id,
    Title = request.Title,
    YearOfRelease = request.YearOfRelease,
    Genres = request.Genres.MapToGenres(id)
  };

  public static MovieResponse MapToMovieResponse(this Movie movie) => new()
  {
    Id = movie.Id,
    Title = movie.Title,
    Slug = movie.Slug,
    YearOfRelease = movie.YearOfRelease,
    Genres = movie.Genres.Select(x => x.Name),
    Rating = movie.Rating,
    UserRating = movie.UserRating
  };

  public static MoviesResponse MapToMoviesResponse(this IEnumerable<Movie> movies) => new()
  {
    Items = movies.Select(movie => movie.MapToMovieResponse())
  };   
}
