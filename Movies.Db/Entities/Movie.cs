using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Movies.Db.Entities;

public class Movie
{
    [Key]
    public Guid MovieId { get; init; }

    [Required]
    [MaxLength(100)]
    public required string Title { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Slug { get; set; }

    [Required]
    public required int YearOfRelease { get; set; }

    [Required]
    public required List<Genre> Genres { get; init; }

}
