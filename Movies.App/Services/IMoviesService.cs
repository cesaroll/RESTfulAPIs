using LanguageExt;
using Movies.App.Models;

namespace Movies.App.Services;

public interface IMoviesService
{
  Task<Either<Error, bool>> CreateAsync(Movie movie, CancellationToken token = default);
  Task<Either<Error, bool>> UpdateAsync(Movie movie, Guid? userId, CancellationToken token = default);
  Task<Either<Error, bool>> DeleteByIdAsync(Guid id, CancellationToken token = default);
  Task<Either<Error, Option<Movie>>> GetByIdAsync(Guid id, Guid? userId, CancellationToken token = default);
  Task<Either<Error, Option<Movie>>> GetBySlugAsync(string slug, Guid? userId, CancellationToken token = default);
  Task<Either<Error, MoviesList>> GetAllAsync(GetAllMoviesOptions options, CancellationToken token = default); 
}
