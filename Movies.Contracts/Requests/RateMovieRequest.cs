using System;

namespace Movies.Contracts.Requests;

public class RateMovieRequest
{
  public required int Rating { get; set; }

}