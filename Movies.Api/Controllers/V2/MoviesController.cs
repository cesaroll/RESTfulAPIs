using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mappers;
using Movies.App.Services;

namespace Movies.Api.Controllers.V2;

[Authorize]
[ApiController]
public class MoviesController : ControllerBase
{
  private readonly IMoviesService _moviesService;
  private readonly AuthContext _authContext;
  private readonly ILogger<MoviesController> _logger;

  public MoviesController(IMoviesService moviesService, AuthContext authContext, ILogger<MoviesController> logger)
  {
      _moviesService = moviesService;
      _authContext = authContext;
      _logger = logger;
  }

  [AllowAnonymous]
  [HttpGet(ApiEndpoints.V2.Movies.Get)]
  public async Task<IActionResult> Get(
    [FromRoute] string idOrSlug,
    [FromServices] LinkGenerator linkGenerator,
    CancellationToken token
  )
  {
    var result = Guid.TryParse(idOrSlug, out var id)
      ? await _moviesService.GetByIdAsync(id, _authContext.UserId, token)
      : await _moviesService.GetBySlugAsync(idOrSlug, _authContext.UserId, token);

    return result.Match<IActionResult>(
      one => one.Match<IActionResult>(
          movie => {
            var response = movie.MapToMovieResponse();

            return Ok(response);
          },
          NotFound()
        )
      ,
      error => BadRequest(error)
    );
  }
}
