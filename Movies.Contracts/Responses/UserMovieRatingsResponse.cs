
namespace Movies.Contracts.Responses;

public class UserMovieRatingsResponse
{
  public IEnumerable<UserMovieRatingResponse> Ratings { get; init; } = Enumerable.Empty<UserMovieRatingResponse>();
}

public class UserMovieRatingResponse
{
  public required Guid MovieId { get; init; }
  public required string Slug { get; init; }
  public required int Rating {get; init; }
}
