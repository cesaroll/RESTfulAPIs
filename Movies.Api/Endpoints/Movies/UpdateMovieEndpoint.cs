using Movies.Api.Auth;
using Movies.Api.Mappers;
using Movies.App.Services;
using Movies.Contracts.Requests;

namespace Movies.Api.Endpoints.Movies;

public static class UpdateMovieEndpoint
{
    public const string Name = "UpdateMovie";

    public static IEndpointRouteBuilder MapUpdateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPut(ApiEndpoints.V1.Movies.Create, async (
            Guid id,
            UpdateMovieRequest request,
            AuthContext authContext,
            IMoviesService moviesService,
            CancellationToken token
        ) => {
            var movie = request.MapToMovie(id);

            var result = await moviesService.UpdateAsync(movie, authContext.UserId, token);

            return result.Match<IResult>(
                success =>
                    success? TypedResults.Ok(movie.MapToMovieResponse()) : Results.NotFound(),
                error =>
                    TypedResults.BadRequest(error)
            );
        })
        .WithName(Name);

        return app;
    }
}
