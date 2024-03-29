using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace GameHeaven.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    public Context Context;

    public GameController(Context context)
    {
        Context = context;
    }

    [HttpPost("CreateGame")]
    public async Task<ActionResult> CreateGame([FromBody] Game game)
    {
        try
        {
            if (game.Title.IsNullOrEmpty())
                return BadRequest("You have to insert the name of the game");

            game.ReviewsOfGame = new List<Review>();
            game.GameListsOfGame = new List<Game_GamingList>();

            await Context.Games.AddAsync(game);
            await Context.SaveChangesAsync();

            return Ok("Game added successfully!");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("GetGames")]
    public async Task<ActionResult> GetGames()
    {
        try
        {
            var games = await Context.Games.ToListAsync();

            if (games == null)
                return BadRequest("Error with getting games from the database");

            if (games.Count == 0)
                return NotFound("There are still no games in the database");

            var gamesWithStrings = games.Select(game => new
            {
                Id = game.ID,
                Title = game.Title,
                Genre = Enum.GetName(typeof(Genre), game.Genre),
                Platform = Enum.GetName(typeof(Platform), game.Platform),
                Description = game.Description,
                Image = game.Image
            });

            return Ok(gamesWithStrings);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}