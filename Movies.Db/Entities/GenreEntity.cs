using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Db.Entities;

[Table("Genres")]
public class GenreEntity
{
    [Required]
    public Guid MovieId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public MovieEntity Movie { get; set; }
}
