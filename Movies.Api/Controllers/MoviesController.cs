using Microsoft.AspNetCore.Mvc;
using Movies.App.Models;
using Movies.App.Repositories;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
  private readonly IMoviesRepository _moviesRepository;

    public MoviesController(IMoviesRepository moviesRepository)
    {
        _moviesRepository = moviesRepository;
    }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
  {
    var movie = new Movie
    {
      Id = Guid.NewGuid(),
      Title = request.Title,
      YearOfRelease = request.YearOfRelease,
      Genres = request.Genres.ToList()
    };

    await _moviesRepository.CreateAsync(movie);

    var movieResponse = new MovieResponse
    {
      Id = movie.Id,
      Title = movie.Title,
      YearOfRelease = movie.YearOfRelease,
      Genres = movie.Genres
    };

    return Created($"api/movies/{movieResponse.Id}", movieResponse);
  }

}
