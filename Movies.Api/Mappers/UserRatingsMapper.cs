using Movies.App.Models;
using Movies.Contracts.Responses;
using Riok.Mapperly.Abstractions;

namespace Movies.Api.Mappers;

public static class UserRatingsMapperExtensions
{
  public static UserMovieRatingsResponse MapToResponse(this UserMovieRatingsList response) => 
    UserRatingsMapper.GetMapper().MapToResponse(response);
}

[Mapper]
public partial class UserRatingsMapper
{
  public static UserRatingsMapper GetMapper() => new UserRatingsMapper();

  public partial UserMovieRatingResponse MapToResponse(UserMovieRating  response);

  public partial UserMovieRatingsResponse MapToResponse(UserMovieRatingsList response);
}
