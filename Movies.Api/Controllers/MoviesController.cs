using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mappers;
using Movies.App.Models;
using Movies.App.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Controllers;

[ApiController]
public class MoviesController : ControllerBase
{
  private readonly IMoviesService _moviesService;

    public MoviesController(IMoviesService moviesService)
    {
        _moviesService = moviesService;
    }

  [HttpPost(ApiEndpoints.Movies.Create)]
  public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
  {
    var movie = request.MapToMovie();

    var result = await _moviesService.CreateAsync(movie);

    return result.Match<IActionResult>(
      success => CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie.MapToMovieResponse()),
      error => BadRequest(error)
    );
  }

  [HttpPut(ApiEndpoints.Movies.Update)]
  public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody] UpdateMovieRequest request)
  {
    var movie = request.MapToMovie(id);

    var result = await _moviesService.UpdateAsync(movie);

    return result.Match<IActionResult>(
      success => success? Ok(movie.MapToMovieResponse()) : NotFound(),
      error => BadRequest(error)
    );  
  }

  [HttpGet(ApiEndpoints.Movies.Get)]
  public async Task<IActionResult> Get([FromRoute] string idOrSlug)
  {
    var result = Guid.TryParse(idOrSlug, out var id)
      ? await _moviesService.GetByIdAsync(id)
      : await _moviesService.GetBySlugAsync(idOrSlug);

    return result.Match<IActionResult>(
      movie => movie.Match<IActionResult>(
          some => Ok(some.MapToMovieResponse()),
          NotFound()
        )
      ,
      error => BadRequest(error)
    );
  }

  [HttpGet(ApiEndpoints.Movies.GetAll)]
  public async Task<IActionResult> GetAll()
  {
    var result = await _moviesService.GetAllAsync();

    return result.Match<IActionResult>(
      moviesList => Ok(moviesList.Movies.MapToMoviesResponse()),
      error => BadRequest(error)
    );
  }

  [HttpDelete(ApiEndpoints.Movies.Delete)]
  public async Task<IActionResult> Delete([FromRoute]Guid id)
  {
    var result = await _moviesService.DeleteByIdAsync(id);

    return result.Match<IActionResult>(
      success => success? Ok() : NotFound(),
      error => BadRequest(error)
    );
  }
}
