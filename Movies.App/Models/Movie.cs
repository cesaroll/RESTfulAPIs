using System.Text.RegularExpressions;

namespace Movies.App.Models;

public partial class Movie
{
  public required Guid Id { get; init; }
  public required string Title { get; set; }
  public string Slug => GenerateSlug();
  public required int YearOfRelease { get; set; }
  public required List<Genre> Genres { get; init; } = new();
  
  public float? Rating {get; set; }
  public int? UserRating {get; set; }


  [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 10)]
  private static partial Regex SlugRegex();
  private string GenerateSlug()
  {
    var sluggedTitle = SlugRegex().Replace(Title, string.Empty)
      .ToLower()
      .Replace(" ", "-");

    return $"{sluggedTitle}-{YearOfRelease}";
  }
}
