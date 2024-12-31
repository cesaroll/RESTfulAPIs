using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mappers;
using Movies.App.Services;
using Movies.Contracts.Responses;

namespace Movies.Api.Endpoints.Movies;

public static class GetMovieEndpoint
{
    public const string Name = "GetMovie";

    public static IEndpointRouteBuilder MapGetMovie(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.V1.Movies.Get, async (
            string idOrSlug,
            AuthContext authContext,
            IMoviesService moviesService,
            CancellationToken token
        ) => {

            var result = Guid.TryParse(idOrSlug, out var id)
                ? await moviesService.GetByIdAsync(id, authContext.UserId, token)
                : await moviesService.GetBySlugAsync(idOrSlug, authContext.UserId, token);

                return result.Match<IResult>(
                one => one.Match<IResult>(
                    movie => {
                        var response = movie.MapToMovieResponse();

                        return TypedResults.Ok(response);
                    },
                    Results.NotFound()
                    )
                ,
                error => TypedResults.BadRequest(error)
                );

            });

        return app;
    }
}
