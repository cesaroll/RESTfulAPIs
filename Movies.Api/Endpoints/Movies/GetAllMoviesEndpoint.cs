using Movies.Api.Auth;
using Movies.Api.Mappers;
using Movies.App.Services;
using Movies.Contracts.Requests;

namespace Movies.Api.Endpoints.Movies;

public static class GetAllMoviesEndpoint
{
    public const string Name = "GetAllMovies";

    public static IEndpointRouteBuilder MapGetAllMovies(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.V1.Movies.GetAll, async (
            [AsParameters] GetAllMoviesRequest request,
            AuthContext authContext,
            IMoviesService moviesService,
            CancellationToken token
        ) => {
            var options = request.MapToOptions()
                .WithUserId(authContext.UserId);

            var result = await moviesService.GetAllAsync(options, token);

            return result.Match<IResult>(
            moviesList => TypedResults.Ok(moviesList.MapToMoviesResponse()),
            error => TypedResults.BadRequest(error)
            );
        })
        .WithName(Name);

        return app;
    }
}
