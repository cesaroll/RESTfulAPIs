using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movies.Db.Entities;

[Table("Movies")]
public class MovieEntity
{
    [Key]
    public Guid Id { get; init; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [Required]
    [MaxLength(100)]
    public string Slug { get; set; }

    [Required]
    public int YearOfRelease { get; set; }

    [Required]
    public List<GenreEntity> Genres { get; init; } = new();

}
