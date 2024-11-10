using System.ComponentModel.DataAnnotations;

namespace Movies.Db.Entities;

public class Genre
{
    [Required]
    public Guid MovieId { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }

    [Required]
    public required Movie Movie { get; set; }
}
