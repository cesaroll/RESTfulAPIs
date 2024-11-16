using System;

namespace Movies.Db.Entities;

public class RatingEntity
{
  public Guid UserId { get; set; }
  public Guid MovieId { get; set; }
  public int Rating { get; set; }

  public required MovieEntity Movie { get; set; }
}
