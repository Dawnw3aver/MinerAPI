using MinerAPI.APIObjects;
using MinerAPI.Entities;
using Newtonsoft.Json;

namespace MinerAPI.EndpointHandlers
{
    public class PostRequestsHandler(List<Game> games)
    {
        public List<Game> Games { get; set; } = games;

        public async Task<IResult> NewGame(HttpContext context)
        {
            using var reader = new StreamReader(context.Request.Body);
            var postData = await reader.ReadToEndAsync();
            if (string.IsNullOrEmpty(postData))
                return Results.BadRequest();

            NewGameRequest request = JsonConvert.DeserializeObject<NewGameRequest>(postData);
            if (request == null)
                return Results.BadRequest(new ErrorResponse("Запрос не распознан."));
            bool isValid = (request.width <= 30 || request.height <= 30) && (request.width * request.height > request.mines_count);
            if (!isValid)
                return Results.BadRequest(new ErrorResponse("Недопустимые параметры поля."));

            Game game = new Game(request.width, request.height, request.mines_count);
            Games.Add(game);
            if (Games.Count > 10)
                Games.Remove(Games.First());
            return Results.Ok(game);
        }

        public async Task<IResult> GameTurn(HttpContext context)
        {
            using var reader = new StreamReader(context.Request.Body);
            var postData = await reader.ReadToEndAsync();
            GameTurnRequest request = JsonConvert.DeserializeObject<GameTurnRequest>(postData);
            if (request == null)
                return Results.BadRequest(new ErrorResponse("Не удалось разобрать запрос."));

            Game game = Games.SingleOrDefault(x => x.Game_id == request.Game_id && x.Completed == false);
            if (game == null)
                return Results.BadRequest(new ErrorResponse("Запрошенная игра не найдена или завершена."));

            return Results.Ok(game.GameTurn(request));
        }
    }
}
