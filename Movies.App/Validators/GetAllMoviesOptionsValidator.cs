using FluentValidation;
using Movies.App.Models;

namespace Movies.App.Validators;

public class GetAllMoviesOptionsValidator: AbstractValidator<GetAllMoviesOptions>
{
  public static readonly string[] SortFields = new[] { "Title", "YearOfRelease"};
  public GetAllMoviesOptionsValidator()
  {
    RuleFor(x => x.Year)
      .InclusiveBetween(1900, DateTime.Now.Year);

    RuleFor(x => x.SortField)
      .Must(x => x is null || SortFields.Contains(x, StringComparer.OrdinalIgnoreCase))
      .WithMessage($"Sort field must be one of: {string.Join(", ", SortFields)}");

    RuleFor(x => x.Page)
      .GreaterThan(0);
    
    RuleFor(x => x.PageSize)
      .InclusiveBetween(1, 25)
      .WithMessage("Page size must be between 1 and 25");
  }
}
