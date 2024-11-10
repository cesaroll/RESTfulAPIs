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

    return CreatedAtAction(nameof(Get), new { idOrSlug = movieResponse.Id }, movieResponse);
  }

  [HttpGet(ApiEndpoints.Movies.Get)]
  public async Task<IActionResult> Get([FromRoute] string idOrSlug)
  {
    var movie = Guid.TryParse(idOrSlug, out var id)
      ? await _moviesRepository.GetByIdAsync(id)
      : await _moviesRepository.GetBySlugAsync(idOrSlug);

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

  [HttpPut(ApiEndpoints.Movies.Update)]
  public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody] UpdateMovieRequest request)
  {
    var movie = request.MapToMovie(id);

    var updated = await _moviesRepository.UpdateAsync(movie);

    if (!updated)
    {
      return NotFound();
    }

    return Ok(movie.MapToMovieResponse());

  }

  [HttpDelete(ApiEndpoints.Movies.Delete)]
  public async Task<IActionResult> Delete([FromRoute]Guid id)
  {
    var deleted = await _moviesRepository.DeleteByIdAsync(id);

    if (!deleted)
    {
      return NotFound();
    }

    return Ok();
  }
}
