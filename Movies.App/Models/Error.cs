using System;

namespace Movies.App.Models;

public class Error
{
  public required string Message { get; init; }

  public List<ValidationError>? ValidationErrors { get; init; }

  public bool IsInternalServerError => ValidationErrors is null || !ValidationErrors.Any();
  public bool IsValidationError => ValidationErrors is not null && ValidationErrors.Any();

}

public class ValidationError
{
    public required string PropertyName { get; init; }
    public required string Message { get; init; }
}
