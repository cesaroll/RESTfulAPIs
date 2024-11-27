using Microsoft.Extensions.Logging;
using Movies.App.Models;
using Movies.Db;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Movies.Db.Entities;
namespace Movies.App.Services;

public class MovieRatingsService : IMovieRatingsService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MovieRatingsService> _logger;

    public MovieRatingsService(IServiceProvider serviceProvider, ILogger<MovieRatingsService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<Either<Error, MovieRating>> GetMovieRatingAsync(Guid id, Guid? userId, CancellationToken token = default)
    {
        try {
            var averageRatingTask = GetAverageMovieRatingAsync(id, token);
            var userRatingTask = GetUserMovieRatingAsync(id, userId, token);

            // Wait for both tasks to complete
            await Task.WhenAll(averageRatingTask, userRatingTask);

            // Assign the results
            var averageRating = await averageRatingTask;
            var userRating = await userRatingTask;

            return new MovieRating() {
                AverageRating = averageRating,
                UserRating = userRating
        };
        } catch (Exception ex)
        {
            var msg ="Error getting movie rating";
            _logger.LogError(ex, msg);
            return new Error { Message = msg };
        }
    }

    private async Task<float?> GetAverageMovieRatingAsync(Guid id, CancellationToken token = default)
    {
        using var scope = _serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MoviesDbContext>();

        var ratings = await dbContext.Ratings
            .AsNoTracking()
            .Where(r => r.MovieId == id)
            .ToListAsync(token);

        return ratings.Any()
            ? (float)ratings.Average(r => r.Rating)
            : null;
    }
        

    private async Task<int?> GetUserMovieRatingAsync(Guid id, Guid? userId, CancellationToken token = default)
    {
        using var scope = _serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MoviesDbContext>();

        return await dbContext.Ratings
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.MovieId == id && r.UserId == userId, token)
            .Select(r => r?.Rating);
    }

    public async Task<Either<Error, bool>> RateMovieAsync(Guid id, Guid userId, int rating, CancellationToken token = default)
    {
        try {
            using var scope = _serviceProvider.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MoviesDbContext>();

            var ratingEntity = await dbContext.Ratings
                .FirstOrDefaultAsync(r => r.MovieId == id && r.UserId == userId, token);

            if (ratingEntity is null) {
                ratingEntity = new RatingEntity() {
                    MovieId = id,
                    UserId = userId,
                    Rating = rating
                };

                dbContext.Ratings.Add(ratingEntity);
            } else {
                ratingEntity.Rating = rating;
            }

            return await dbContext.SaveChangesAsync(token) > 0;
            
        } catch (Exception ex)
        {
            var msg = "Error rating movie";
            _logger.LogError(ex, msg);
            return new Error { Message = msg };
        }
    }

    public async Task<Either<Error, bool>> DeleteRatingAsync(Guid id, Guid userId, CancellationToken token = default)
    {
        try {
            using var scope = _serviceProvider.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MoviesDbContext>();

            var ratingEntity = await dbContext.Ratings
                .FirstOrDefaultAsync(r => r.MovieId == id && r.UserId == userId, token);

            if (ratingEntity is null) {
                return false;
            }

            dbContext.Ratings.Remove(ratingEntity);
            return await dbContext.SaveChangesAsync(token) > 0;

        } catch (Exception ex)
        {
            var msg = "Error deleting movie rating";
            _logger.LogError(ex, msg);
            return new Error { Message = msg };
        }
    }

    public async Task<Either<Error, UserMovieRatingsList>> GetUserRatingsAsync(Guid userId, CancellationToken token = default)
    {
        try {
            using var scope = _serviceProvider.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MoviesDbContext>();

            var ratings = await dbContext.Ratings
                .AsNoTracking()
                .Include(r => r.Movie)
                .Where(r => r.UserId == userId)
                .ToListAsync(token);

            var userRatings = ratings.Select(r => new UserMovieRating() {
                MovieId = r.MovieId,
                Slug = r.Movie.Slug,
                Rating = r.Rating
            }).ToList();

            return new UserMovieRatingsList() {
                Ratings = userRatings
            };

        } catch (Exception ex)
        {
            var msg = "Error getting user ratings";
            _logger.LogError(ex, msg);
            return new Error { Message = msg };
        }
    }
}
