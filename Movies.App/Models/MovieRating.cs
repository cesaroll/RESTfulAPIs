using System;

namespace Movies.App.Models;

public class MovieRating
{
  public float? AverageRating { get; set; }
  public int? UserRating { get; set; }

  
  public void Deconstruct(out float? averageRating, out int? userRating)
  {
    averageRating = AverageRating;
    userRating = UserRating;
  }
}
