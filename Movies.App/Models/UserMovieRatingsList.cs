namespace Movies.App.Models;

public class UserMovieRatingsList
{
  public IList<UserMovieRating> Ratings { get; set; } = new List<UserMovieRating>();
}

public class UserMovieRating
{
  public required Guid MovieId { get; init; }
  public required string Slug { get; init; }
  public required int Rating {get; init; }
}