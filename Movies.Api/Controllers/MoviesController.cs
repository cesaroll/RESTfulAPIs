using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mappers;
using Movies.App.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Controllers;

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

  [Authorize(AuthConstants.TrustedMemberPolicyName)]
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

  [Authorize(AuthConstants.TrustedMemberPolicyName)]
  [HttpPut(ApiEndpoints.Movies.Update)]
  public async Task<IActionResult> Update(
    [FromRoute] Guid id,
    [FromBody] UpdateMovieRequest request,
    CancellationToken token
  )
  {
    var movie = request.MapToMovie(id);

    var result = await _moviesService.UpdateAsync(movie, _authContext.UserId, token);

    return result.Match<IActionResult>(
      success => success? Ok(movie.MapToMovieResponse()) : NotFound(),
      error => BadRequest(error)
    );
  }

  [AllowAnonymous]
  [HttpGet(ApiEndpoints.Movies.Get)]
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

            response.Links.Add(new Link(){
              Href = linkGenerator.GetPathByAction(HttpContext, nameof(Get), values: new {idOrSlug = movie.Id}),
              Rel = "Self",
              Type = "GET"
            });

            response.Links.Add(new Link(){
              Href = linkGenerator.GetPathByAction(HttpContext, nameof(Update), values: movie),
              Rel = "Self",
              Type = "PUT"
            });

            response.Links.Add(new Link(){
              Href = linkGenerator.GetPathByAction(HttpContext, nameof(Delete), values: movie),
              Rel = "Self",
              Type = "DELETE"
            });

            return Ok(response);
          },
          NotFound()
        )
      ,
      error => BadRequest(error)
    );
  }

  [AllowAnonymous]
  [HttpGet(ApiEndpoints.Movies.GetAll)]
  public async Task<IActionResult> GetAll(
    [FromQuery] GetAllMoviesRequest request, CancellationToken token)
  {

    var options = request.MapToOptions()
      .WithUserId(_authContext.UserId);

    var result = await _moviesService.GetAllAsync(options, token);

    return result.Match<IActionResult>(
      moviesList => Ok(moviesList.MapToMoviesResponse()),
      error => BadRequest(error)
    );
  }

  [Authorize(AuthConstants.AdminUserPolicyName)]
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
