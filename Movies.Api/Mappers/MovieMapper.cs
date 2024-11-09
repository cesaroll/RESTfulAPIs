using System;
using Movies.App.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Mappers;

public static class MovieMapper
{
  public static Movie MapToMovie(this CreateMovieRequest request) => new()
  {
    Id = Guid.NewGuid(),
    Title = request.Title,
    YearOfRelease = request.YearOfRelease,
    Genres = request.Genres.ToList()
  };

  public static MovieResponse MapToMovieResponse(this Movie movie) => new()
  {
    Id = movie.Id,
    Title = movie.Title,
    YearOfRelease = movie.YearOfRelease,
    Genres = movie.Genres
  };
   
}
