﻿// See https://aka.ms/new-console-template for more information
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


Console.WriteLine("\nReading Movie:\n");
var movie = await moviesApi.GetMoviesAsync("f7f5bde4-5d02-4c5e-85d8-e2adadb4bbc5", default);

Console.WriteLine(JsonSerializer.Serialize(movie, new JsonSerializerOptions{
    WriteIndented = true
}));


Console.WriteLine("\nGet All Movies:\n");
var request = new GetAllMoviesRequest
{
    Page = 1,
    PageSize = 10
};

var movies = await moviesApi.GetAllMoviesAsync(request, default);

Console.WriteLine(JsonSerializer.Serialize(movies, new JsonSerializerOptions{
    WriteIndented = true
}));
