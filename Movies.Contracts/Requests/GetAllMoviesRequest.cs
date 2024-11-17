using System;

namespace Movies.Contracts.Requests;

public class GetAllMoviesRequest : PagedRequests
{
  public string? Title { get; set; }
  public int? Year { get; set; }

  public string? SortBy { get; set; }
}
