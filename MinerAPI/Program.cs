using MinerAPI.EndpointHandlers;
using MinerAPI.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(options => {
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
    options.AllowAnyHeader();
    });

List<Game> games = [];
PostRequestsHandler postRequestsHandler = new(games);

app.MapPost("/api/new", async (HttpContext context) =>
{
    return await postRequestsHandler.NewGame(context);
});

app.MapPost("/api/turn", async (HttpContext context) =>
{
    return await postRequestsHandler.GameTurn(context);
});

app.Run();