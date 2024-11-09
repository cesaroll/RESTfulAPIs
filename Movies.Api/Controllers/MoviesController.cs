using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mappers;
using Movies.App.Models;
using Movies.App.Repositories;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Controllers;

[ApiController]
public class MoviesController : ControllerBase
{
  private readonly IMoviesRepository _moviesRepository;

    public MoviesController(IMoviesRepository moviesRepository)
    {
        _moviesRepository = moviesRepository;
    }

  [HttpPost(ApiEndpoints.Movies.Create)]
  public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
  {
    var movie = request.MapToMovie();

    await _moviesRepository.CreateAsync(movie);

    var movieResponse = movie.MapToMovieResponse(); 

    return Created($"{ApiEndpoints.Movies.Create }/{movieResponse.Id}", movieResponse);
  }

}
