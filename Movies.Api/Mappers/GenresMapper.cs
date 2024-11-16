using Movies.App.Models;

namespace Movies.Api.Mappers;

public static class GenresMapper
{
  public static Genre MapToGenre(this string genre, Guid movieId) => new()
  {
    MovieId = movieId,
    Name = genre
  };

  public static List<Genre> MapToGenres(this IEnumerable<string> genres, Guid movieId) => 
    genres.Select(genre => genre.MapToGenre(movieId)).ToList();
}
