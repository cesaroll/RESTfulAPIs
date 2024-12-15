// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using Movies.Api.Sdk;
using Refit;

var moviesApi = RestService.For<IMoviesApi>("http://localhost:5149");


Console.WriteLine("\nReading Movie:\n");
var movie = await moviesApi.GetMoviesAsync("f7f5bde4-5d02-4c5e-85d8-e2adadb4bbc5", default);

Console.WriteLine(JsonSerializer.Serialize(movie, new JsonSerializerOptions{
    WriteIndented = true
}));
