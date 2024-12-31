using Movies.Api.Mappers;
using Movies.App.Services;
using Movies.Contracts.Requests;

namespace Movies.Api.Endpoints.Movies;

public static class CreateMovieEndpoint
{
    public const string Name = "CreateMovie";

    public static IEndpointRouteBuilder MapCreateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.V1.Movies.Create, async (
            CreateMovieRequest request,
            IMoviesService moviesService,
            CancellationToken token
        ) => {
            var movie = request.MapToMovie();

            var result = await moviesService.CreateAsync(movie, token);

            return result.Match(
                success =>
                    TypedResults.CreatedAtRoute(movie.MapToMovieResponse, GetMovieEndpoint.Name, new { idOrSlug = movie.Id }),
                error =>
                    TypedResults.BadRequest(error)
            );
        })
        .WithName(Name);

        return app;
    }
}
