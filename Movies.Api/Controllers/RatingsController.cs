using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.App.Services;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;

[ApiController]
public class RatingsController : ControllerBase
{
    private readonly IMovieRatingsService _movieRatingsService;
    private readonly AuthContext _authContext;

    private readonly ILogger<RatingsController> _logger;

    public RatingsController(IMovieRatingsService movieRatingsService, ILogger<RatingsController> logger, AuthContext authContext)
    {
        _movieRatingsService = movieRatingsService;
        _logger = logger;
        _authContext = authContext;
    }

    [Authorize]
    [HttpPut(ApiEndpoints.Movies.Rate)]
    public async Task<IActionResult> Rate(
        [FromRoute] Guid id,
        [FromBody] RateMovieRequest request,
        CancellationToken token
    )
    {
        Guid userId = _authContext.UserId!.Value;

        var result = await _movieRatingsService.RateMovieAsync(id, userId, request.Rating, token);

        return result.Match<IActionResult>(
            success => success ? Ok() : NotFound(),
            error => BadRequest(error)
        );
    }

    [Authorize]
    [HttpDelete(ApiEndpoints.Movies.DeleteRating)]
    public async Task<IActionResult> DeleteRating(
        [FromRoute] Guid id,
        CancellationToken token
    )
    {
        Guid userId = _authContext.UserId!.Value;

        var result = await _movieRatingsService.DeleteRatingAsync(id, userId, token);

        return result.Match<IActionResult>(
            success => success ? Ok() : NotFound(),
            error => BadRequest(error)
        );
    }
}
