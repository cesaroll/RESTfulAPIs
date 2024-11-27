namespace Movies.App.Models;

public class Genre
{
    public Guid MovieId { get; set; }
    public required string Name { get; set; }
}
