using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mappers;
using Movies.App.Services;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;

[Authorize]
[ApiController]
public class MoviesController : ControllerBase
{
  private readonly IMoviesService _moviesService; 

    public MoviesController(IMoviesService moviesService)
    {
        _moviesService = moviesService;
    }

  [HttpPost(ApiEndpoints.Movies.Create)]
  public async Task<IActionResult> Create(
    [FromBody] CreateMovieRequest request,
    CancellationToken token
  )
  {
    var movie = request.MapToMovie();

    var result = await _moviesService.CreateAsync(movie, token);

    return result.Match<IActionResult>(
      success => CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie.MapToMovieResponse()),
      error => BadRequest(error)
    );
  }

  [HttpPut(ApiEndpoints.Movies.Update)]
  public async Task<IActionResult> Update(
    [FromRoute] Guid id, 
    [FromBody] UpdateMovieRequest request,
    CancellationToken token
  )
  {
    var movie = request.MapToMovie(id);

    var result = await _moviesService.UpdateAsync(movie, token);

    return result.Match<IActionResult>(
      success => success? Ok(movie.MapToMovieResponse()) : NotFound(),
      error => BadRequest(error)
    );  
  }

  [AllowAnonymous]
  [HttpGet(ApiEndpoints.Movies.Get)]
  public async Task<IActionResult> Get(
    [FromRoute] string idOrSlug,
    CancellationToken token
  )
  {
    var result = Guid.TryParse(idOrSlug, out var id)
      ? await _moviesService.GetByIdAsync(id, token)
      : await _moviesService.GetBySlugAsync(idOrSlug, token);

    return result.Match<IActionResult>(
      movie => movie.Match<IActionResult>(
          some => Ok(some.MapToMovieResponse()),
          NotFound()
        )
      ,
      error => BadRequest(error)
    );
  }

  [AllowAnonymous]
  [HttpGet(ApiEndpoints.Movies.GetAll)]
  public async Task<IActionResult> GetAll(CancellationToken token)
  {
    var result = await _moviesService.GetAllAsync(token);

    return result.Match<IActionResult>(
      moviesList => Ok(moviesList.Movies.MapToMoviesResponse()),
      error => BadRequest(error)
    );
  }

  [HttpDelete(ApiEndpoints.Movies.Delete)]
  public async Task<IActionResult> Delete(
    [FromRoute]Guid id,
    CancellationToken token
  )
  {
    var result = await _moviesService.DeleteByIdAsync(id, token);

    return result.Match<IActionResult>(
      success => success? Ok() : NotFound(),
      error => BadRequest(error)
    );
  }
}
 