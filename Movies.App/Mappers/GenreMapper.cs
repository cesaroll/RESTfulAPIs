using Riok.Mapperly.Abstractions;
using Movies.App.Models;
using Movies.Db.Entities;

namespace Movies.App.Mappers;

public static class GenreMapperExtensions
{
  public static Genre MapToGenreModel(this GenreEntity entity) =>
    GenreMapper.GetMapper().MapToModel(entity);
  
  public static GenreEntity MapToGenreEntity(this Genre model) =>
    GenreMapper.GetMapper().MapToEntity(model);
  
  public static IList<GenreEntity> MapToGenreEntityList(this IList<Genre> entities) =>
    entities.Select(MapToGenreEntity).ToList();
}

[Mapper]
public partial class GenreMapper
{
  public static GenreMapper GetMapper() => new GenreMapper();

  public partial Genre MapToModel(GenreEntity entity);
  public partial GenreEntity MapToEntity(Genre model);

}
