 namespace Movies.App.Models;

public class GetAllMoviesOptions
{
  public Guid? UserId { get; set; }
  public required string? Title { get; init; }
  public required int? Year { get; init; }
  public required string? SortField { get; init; }
  public required SortOrder SortOrder { get; init; }

  public required int Page { get; set; }
  public required int PageSize { get; set; }
}

public enum SortOrder
{
  Unsorted,
  Ascending,
  Descending
}
