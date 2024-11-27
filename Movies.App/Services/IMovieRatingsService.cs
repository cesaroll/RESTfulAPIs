using LanguageExt;
using Movies.App.Models;

namespace Movies.App.Services;

public interface IMovieRatingsService
{
  Task<Either<Error, MovieRating>> GetMovieRatingAsync(Guid id, Guid? userId, CancellationToken token = default);
  Task<Either<Error, bool>> RateMovieAsync(Guid id, Guid userId, int rating, CancellationToken token = default);
  Task<Either<Error, bool>> DeleteRatingAsync(Guid id, Guid userId, CancellationToken token = default);
  Task<Either<Error, UserMovieRatingsList>> GetUserRatingsAsync(Guid userId, CancellationToken token = default);
}
