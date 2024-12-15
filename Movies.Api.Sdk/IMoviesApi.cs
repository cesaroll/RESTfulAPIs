using Movies.Contracts.Requests;
using Movies.Contracts.Responses;
using Refit;

namespace Movies.Api.Sdk;

public interface IMoviesApi
{
    [Get(ApiEndpoints.V2.Movies.Get)]
    Task<MovieResponse> GetMoviesAsync(string idOrSlug, CancellationToken token = default);

    [Get(ApiEndpoints.V1.Movies.GetAll)]
    Task<MoviesResponse> GetAllMoviesAsync(GetAllMoviesRequest request, CancellationToken token = default);
}
