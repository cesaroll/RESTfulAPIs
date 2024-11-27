using FluentValidation.Results;
using Movies.App.Models;

namespace Movies.App.Mappers;

public static class ErrorMapper
{
    public static Error MapToError(this ValidationResult result)
    {
        var validationErrors = result.Errors.Select(x => new ValidationError()
          {
              PropertyName = x.PropertyName,
              Message = x.ErrorMessage
          })
          .ToList();

        return new Error(){
            Message = "Bad Request",
            ValidationErrors = validationErrors
        };
    }
}
