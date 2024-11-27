using LanguageExt;
using Movies.App.Models;
using Movies.Db;
using Movies.App.Mappers;
using Movies.Db.Entities;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace Movies.App.Services;

public class MoviesService : IMoviesService
{
    private readonly MoviesDbContext _dbContext;
    private readonly IValidator<Movie> _movieValidator;
    private readonly IValidator<GetAllMoviesOptions> _getAllMoviesOptionsValidator;
    private readonly IMovieRatingsService _movieRatingsService;
    private readonly ILogger<MoviesService> _logger;

    public MoviesService(
        MoviesDbContext dbContext, 
        IValidator<Movie> movieValidator, 
        IValidator<GetAllMoviesOptions> getAllMoviesOptionsValidator,
        IMovieRatingsService movieRatingsService,
        ILogger<MoviesService> logger)
    {
        _dbContext = dbContext;
        _movieValidator = movieValidator;
        _getAllMoviesOptionsValidator = getAllMoviesOptionsValidator;
        _movieRatingsService = movieRatingsService;
        _logger = logger;
    }

    public async Task<Either<Error, bool>> CreateAsync(Movie movie, CancellationToken token = default)
    {
        try {
            var result = await _movieValidator.ValidateAsync(movie, token);

            if (!result.IsValid)
                return result.MapToError();

            await _dbContext.Movies.AddAsync(movie.MapToMovieEntity(), token);

            return await _dbContext.SaveChangesAsync(token) > 0;

        } catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating movie");
            return new Error { Message = "Error creating movie" };
        }
    }

    public async Task<Either<Error, bool>> UpdateAsync(Movie movie, Guid? userId, CancellationToken token = default)
    {
        try {
            var result = await _movieValidator.ValidateAsync(movie, token);

            if (!result.IsValid)
                return result.MapToError();

            var movieEntity = await _dbContext.Movies
              .Include(x => x.Genres)
              .FirstOrDefaultAsync(m => m.Id == movie.Id, token);
            
            if (movieEntity is null)
            {
                var msg = $"Movie not found. Id: {movie.Id}";
                _logger.LogInformation(msg);
                return new Error { Message = msg };
            }
            
            movieEntity.Title = movie.Title;
            movieEntity.Slug = movie.Slug;
            movieEntity.YearOfRelease = movie.YearOfRelease;
            movieEntity.Genres = movie.Genres.MapToGenreEntityList().ToList();

            _dbContext.Movies.Update(movieEntity);

            if (await _dbContext.SaveChangesAsync(token) == 0)
            {
                return false;
            }   

            movie = await GetMovieRatingASync(movie, userId, token);

            return true;

        } catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating movie");
            return new Error { Message = "Error updating movie" };
        }
    }

    public async Task<Either<Error, bool>> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        try {
            _dbContext.Movies.Remove(new MovieEntity { Id = id });
            return await _dbContext.SaveChangesAsync(token) > 0;

        } catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting movie");
            return new Error { Message = "Error deleting movie" };
        }
    }

    public async Task<Either<Error, Option<Movie>>> GetByIdAsync(Guid id, Guid? userId, CancellationToken token = default)
    {
        try {
            var movieEntity = await _dbContext.Movies
              .AsNoTracking()
              .Include(x => x.Genres)
              .FirstOrDefaultAsync(m => m.Id == id, token);

            if (movieEntity is null)
                return Option<Movie>.None;

            var movie = movieEntity?.MapToMovieModel();

            movie = await GetMovieRatingASync(movie!, userId, token);

            return Option<Movie>.Some(movie);

        } catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting movie by id");
            return new Error { Message = "Error getting movie by id" };
        }
    }

    public async Task<Either<Error, Option<Movie>>> GetBySlugAsync(string slug, Guid? userId, CancellationToken token = default)
    {
        try {
            var movieEntity = await _dbContext.Movies
              .AsNoTracking()
              .Include(x => x.Genres)
              .FirstOrDefaultAsync(m => m.Slug == slug, token);
            
            if (movieEntity is null)
                return Option<Movie>.None;

            var movie = movieEntity?.MapToMovieModel();

            await GetMovieRatingASync(movie!, userId, token);

            return Option<Movie>.Some(movie!);

        } catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting movie by slug");
            return new Error { Message = "Error getting movie by slug" };
        }
    }

    public async Task<Either<Error, MoviesList>> GetAllAsync(GetAllMoviesOptions options, CancellationToken token = default)
    {
        try {
            
            var result = await _getAllMoviesOptionsValidator.ValidateAsync(options, token);

            if (!result.IsValid)
                return result.MapToError();

            var moviesQuery = _dbContext.Movies
              .AsNoTracking()
              .Include(x => x.Genres)
              .Where(x => 
                string.IsNullOrWhiteSpace(options.Title) || 
                EF.Functions.Like(x.Title.ToLower(), $"%{options.Title.ToLower()}%"))
              .Where(x => !options.Year.HasValue || x.YearOfRelease == options.Year);
              

            var totalCount = await moviesQuery.CountAsync(token);

            if (options.SortOrder != SortOrder.Unsorted && options.SortField is not null)
            {
                moviesQuery = moviesQuery
                    .OrderBy($"{options.SortField} {(options.SortOrder == SortOrder.Ascending ? "asc" : "desc")}");
            }

            var movieEntities = await moviesQuery
                .Skip((options.Page - 1) * options.PageSize)
                .Take(options.PageSize)
                .ToListAsync(token);
            
            var movies = movieEntities.MapToMoviesList(totalCount, options.Page, options.PageSize);

            await Parallel.ForEachAsync(
                movies.Movies, 
                token, 
                async (movie, _) => await GetMovieRatingASync(movie, options.UserId, token)
            );

            return movies;

        } catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all movies");
            return new Error { Message = "Error getting all movies" };
        }
    }

    private async Task<Movie> GetMovieRatingASync(Movie movie, Guid? userId, CancellationToken token = default)
    {
        var result = await _movieRatingsService.GetMovieRatingAsync(movie.Id, userId, token);
        
        return result.Match<Movie>(
            rating => {
                movie.Rating = rating.AverageRating;
                movie.UserRating = rating.UserRating;
                return movie;
            },
            error => movie
        );
    }
}
