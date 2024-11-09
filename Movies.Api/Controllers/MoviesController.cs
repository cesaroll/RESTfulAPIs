using System.Reflection.Metadata.Ecma335;
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

  [HttpGet(ApiEndpoints.Movies.Get)]
  public async Task<IActionResult> Get(Guid id)
  {
    var movie = await _moviesRepository.GetByIdAsync(id);

    return movie.Match<IActionResult>(
      movie => Ok(movie.MapToMovieResponse()),
      () => NotFound()
    );
  }

  [HttpGet(ApiEndpoints.Movies.GetAll)]
  public async Task<IActionResult> GetAll()
  {
    var movies = await _moviesRepository.GetAllAsync();

    var moviesResponse = movies.MapToMoviesResponse();

    return Ok(moviesResponse);
  }

}
