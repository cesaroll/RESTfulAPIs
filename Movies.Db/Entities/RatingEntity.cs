using System;
using System.ComponentModel.DataAnnotations;

namespace Movies.Db.Entities;

public class RatingEntity
{
  [Required]
  public Guid UserId { get; set; }

  [Required]
  public Guid MovieId { get; set; }

  [Required]
  public int Rating { get; set; }

  [Required]
    public MovieEntity Movie { get; set; }
}
