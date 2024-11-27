
using Riok.Mapperly.Abstractions;
using Movies.App.Models;
using Movies.Db.Entities;

namespace Movies.App.Mappers;

public static class MovieMapperExtensions
{
    public static Movie MapToMovieModel(this MovieEntity entity) =>
        MovieMapper.GetMapper().MapToModel(entity);

    public static MovieEntity MapToMovieEntity(this Movie model) =>
        MovieMapper.GetMapper().MapToEntity(model);

    public static MoviesList MapToMoviesList(this IList<MovieEntity> entities, int totalCount, int page, int pageSize)
    {
        var movies = MovieMapper.GetMapper().MapToModelList(entities);
        var moviesList = new MoviesList() {
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
        
        if(movies is not null)
            moviesList.Movies = movies;

        return moviesList;
    }
        
}

[Mapper]
public partial class MovieMapper
{
    public static MovieMapper GetMapper() => new MovieMapper();

    public partial Movie MapToModel(MovieEntity entity);

    public partial IList<Movie> MapToModelList(IList<MovieEntity> entities);

    public partial MovieEntity MapToEntity(Movie model);

}
