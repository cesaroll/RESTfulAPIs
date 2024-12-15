using System.Runtime.Intrinsics;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;
using Refit;

namespace Movies.Api.Sdk;

[Headers("Authorization: Bearer")]
public interface IMoviesApi
{
    [Get(ApiEndpoints.V2.Movies.Get)]
    Task<MovieResponse> GetMovieAsync(string idOrSlug, CancellationToken token = default);

    [Get(ApiEndpoints.V1.Movies.GetAll)]
    Task<MoviesResponse> GetAllMoviesAsync(GetAllMoviesRequest request, CancellationToken token = default);

    [Post(ApiEndpoints.V1.Movies.Create)]
    Task<MovieResponse> CreateMovieAsync(CreateMovieRequest request, CancellationToken token = default);

    [Put(ApiEndpoints.V1.Movies.Update)]
    Task<MovieResponse> UpdateMovieAsync(Guid id, UpdateMovieRequest request, CancellationToken token = default);

    [Delete(ApiEndpoints.V1.Movies.Delete)]
    Task DeleteMovieAsync(Guid id);

    // TODO: Add all rating endpoints
}
