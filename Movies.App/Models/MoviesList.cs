namespace Movies.App.Models;

public class MoviesList
{
  public required int Page { get; init; }
  public required int PageSize { get; init; }
  public required int TotalCount { get; init; }
  public IList<Movie> Movies { get; set; } = new List<Movie>();
}
