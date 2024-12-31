using Movies.Api.Auth;
using Movies.App.Services;

namespace Movies.Api.Endpoints.Movies;

public static class DeleteMovieEndpoint
{
    public static IEndpointRouteBuilder MapDeleteMovie(this IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiEndpoints.V1.Movies.Delete, async (
            Guid id,
            AuthContext authContext,
            IMoviesService moviesService,
            CancellationToken token
        ) => {
            var result = await moviesService.DeleteByIdAsync(id, token);

            return result.Match<IResult>(
                success =>
                    success? TypedResults.Ok() : Results.NotFound(),
                error =>
                    TypedResults.BadRequest(error)
            );
        });

        return app;
    }
}
