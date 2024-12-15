// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Movies.Api.Sdk;
using Movies.Api.Sdk.Consumer;
using Movies.Contracts.Requests;
using Refit;


// var moviesApi = RestService.For<IMoviesApi>("http://localhost:5149");

var services = new ServiceCollection();


services
    .AddHttpClient()
    .AddSingleton<AuthTokenProvider>()
    .AddRefitClient<IMoviesApi>(s => new RefitSettings
    {
        AuthorizationHeaderValueGetter = async (req, res) => await s.GetRequiredService<AuthTokenProvider>().GetTokenAsync()
    })
    .ConfigureHttpClient(x =>
        x.BaseAddress = new Uri("http://localhost:5149")
    );

var provider = services.BuildServiceProvider();
var moviesApi = provider.GetRequiredService<IMoviesApi>();


Console.WriteLine("\nCreating Movie:\n");
var movie = await moviesApi.CreateMovieAsync(new CreateMovieRequest
{
    Title = "Spiderman",
    YearOfRelease = 2002,
    Genres = new[] { "Action" }
});

Console.WriteLine(JsonSerializer.Serialize(movie, new JsonSerializerOptions{
    WriteIndented = true
}));

Console.WriteLine("\nUpdating Movie:\n");
movie = await moviesApi.UpdateMovieAsync(movie.Id, new UpdateMovieRequest
    {
        Title = movie.Title,
        YearOfRelease = 2003,
        Genres = new[] { "Action", "Adventure"}
    });
Console.WriteLine(JsonSerializer.Serialize(movie, new JsonSerializerOptions{
    WriteIndented = true
}));

Console.WriteLine("\nReading Movie:\n");
movie = await moviesApi.GetMovieAsync(movie.Id.ToString());

Console.WriteLine(JsonSerializer.Serialize(movie, new JsonSerializerOptions{
    WriteIndented = true
}));


Console.WriteLine("\nDeleting movie:\n");
await moviesApi.DeleteMovieAsync(movie.Id);


Console.WriteLine("\nGet All Movies:\n");
var request = new GetAllMoviesRequest
{
    Page = 1,
    PageSize = 10
};

var movies = await moviesApi.GetAllMoviesAsync(request);

Console.WriteLine(JsonSerializer.Serialize(movies, new JsonSerializerOptions{
    WriteIndented = true
}));
