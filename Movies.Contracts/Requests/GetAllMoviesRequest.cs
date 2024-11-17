using System;

namespace Movies.Contracts.Requests;

public class GetAllMoviesRequest
{
  public string? Title { get; set; }
  public int? Year { get; set; }
}
