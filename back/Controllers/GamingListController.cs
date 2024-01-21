using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace GameHeaven.Controllers;

[ApiController]
[Route("[controller]")]
public class GamingListController : ControllerBase
{
    public Context Context;

    public GamingListController(Context context)
    {
        Context = context;
    }

    [HttpPost("CreateNewGamingList/{gamingListTitle}")]
    public async Task<ActionResult> CreateNewGamingList(string gamingListTitle)
    {
        try
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest("No logged-in user. Please log in.");
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
            {
                return BadRequest("Error with getting the current user");
            }
            var userClaims = identity.Claims;
            int id = int.Parse(userClaims.FirstOrDefault(p => p.Type == ClaimTypes.Sid)!.Value);

            var player = await Context.Players.Where(p => p.ID == id).Include(p => p.ReviewsOfPlayer).Include(p => p.GamingListsOfPlayer).FirstOrDefaultAsync();

            if (player == null)
                return BadRequest("Error with getting data about current user");


            if (string.IsNullOrEmpty(gamingListTitle))
                return BadRequest("You need to insert a name of the gaming list");

            if (player.GamingListsOfPlayer != null)
                foreach (GamingList gList in player.GamingListsOfPlayer)
                {
                    if (gList.ListName == gamingListTitle)
                        return BadRequest("You already have gaming list with that name");
                }
            else
                player.GamingListsOfPlayer = new List<GamingList>();


            GamingList gamingList = new GamingList
            {
                ListName = gamingListTitle
            };

            await Context.GamingLists.AddAsync(gamingList);


            player.GamingListsOfPlayer.Add(gamingList);

            Context.Update(player);

            await Context.SaveChangesAsync();

            return Ok("Sucessfully created gaming list");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    [HttpPut("AddGameToGamingList/{gameTitle}/{gamingListName}")]
    public async Task<ActionResult> AddGameToGamingList(string gameTitle, string gamingListName)
    {
        try
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest("No logged-in user. Please log in.");
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
            {
                return BadRequest("Error with getting the current user");
            }

            var userClaims = identity.Claims;
            int id = int.Parse(userClaims.FirstOrDefault(p => p.Type == ClaimTypes.Sid)!.Value);

            var player = await Context.Players.Where(p => p.ID == id).Include(p => p.GamingListsOfPlayer).FirstOrDefaultAsync();

            if (player == null)
                return BadRequest("Error with getting data about current user");

            if (string.IsNullOrEmpty(gameTitle))
                return BadRequest("You need to enter game title");

            if (string.IsNullOrEmpty(gamingListName))
                return BadRequest("You need to enter gaming list name");

            var game = await Context.Games.Where(g => g.Title == gameTitle).FirstOrDefaultAsync();

            if (game == null)
                return BadRequest("There is no game with that title");

            var gList = await Context.GamingLists.Where(gl => gl.ListName == gamingListName && gl.CreatorOfGamingList == player).FirstOrDefaultAsync();

            if (gList == null)
                return BadRequest("There is no gaming list with that name");

            var checkList = await Context.Game_GamingLists.Where(g => g.GamingList == gList && g.Game == game).FirstOrDefaultAsync();

            if (checkList != null)
                return Ok("That game is already in that list!");

            Game_GamingList newGgList = new Game_GamingList
            {
                Game = game,
                GamingList = gList,
                GameID = game.ID,
                GamingListID = gList.ID
            };

            await Context.Game_GamingLists.AddAsync(newGgList);

            if (game.GameListsOfGame == null)
                game.GameListsOfGame = new List<Game_GamingList>();

            if (gList.GamesInGamingList == null)
                gList.GamesInGamingList = new List<Game_GamingList>();

            game.GameListsOfGame.Add(newGgList);
            gList.GamesInGamingList.Add(newGgList);

            Context.Games.Update(game);
            Context.GamingLists.Update(gList);

            await Context.SaveChangesAsync();

            return Ok("Successfully added a game to your gaming list");

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("GetGamingListsWithGames")]
    public async Task<ActionResult> GetGamingListsWithGames()
    {
        try
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest("No logged-in user. Please log in.");
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
            {
                return BadRequest("Error with getting the current user");
            }

            var userClaims = identity.Claims;
            int id = int.Parse(userClaims.FirstOrDefault(p => p.Type == ClaimTypes.Sid)!.Value);

            var player = await Context.Players
                .Where(p => p.ID == id)
                .Include(p => p.GamingListsOfPlayer)
                    .ThenInclude(p => p.GamesInGamingList)
                        .ThenInclude(p => p.Game)
                            .ThenInclude(p => p.ReviewsOfGame)
                .FirstOrDefaultAsync();

            if (player == null)
                return BadRequest("Error with getting data about the current user");

            var gamingListsWithGamesAndRating = player.GamingListsOfPlayer.Select(gamingList => new
            {
                Id = gamingList.ID,
                Name = gamingList.ListName,
                GamesInGamingList = gamingList.GamesInGamingList.Select(gameInList => new
                {
                    GameId = gameInList.Game.ID,
                    Title = gameInList.Game.Title,
                    Genre = Enum.GetName(typeof(Genre), gameInList.Game.Genre),
                    Platform = Enum.GetName(typeof(Platform), gameInList.Game.Platform),
                    Description = gameInList.Game.Description,
                    Image = gameInList.Game.Image,
                    Rating = gameInList.Game.ReviewsOfGame.Any()
                        ? gameInList.Game.ReviewsOfGame.Average(review => review.Rating)
                        : 0
                })
            });

            return Ok(gamingListsWithGamesAndRating);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("DeleteGamingList/{listName}")]
    public async Task<ActionResult> DeleteGamingList(string listName)
    {
        try
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest("No logged-in user. Please log in.");
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
            {
                return BadRequest("Error with getting the current user");
            }

            var userClaims = identity.Claims;
            int id = int.Parse(userClaims.FirstOrDefault(p => p.Type == ClaimTypes.Sid)!.Value);

            var player = await Context.Players
                .Where(p => p.ID == id)
                .Include(p => p.GamingListsOfPlayer)
                    .ThenInclude(p => p.GamesInGamingList)
                        .ThenInclude(p => p.Game)
                            .ThenInclude(p => p.ReviewsOfGame)
                .FirstOrDefaultAsync();

            if (player == null)
                return BadRequest("Error with getting data about the current user");

            var list = await Context.GamingLists.Where(l => l.ListName == listName && l.CreatorOfGamingList == player).Include(l => l.GamesInGamingList).FirstOrDefaultAsync();

            if (list == null)
                return BadRequest("There is no game list with that name");

            var ggList = await Context.Game_GamingLists.Where(gg => gg.GamingList == list).FirstOrDefaultAsync();

            if (ggList == null)
                return BadRequest("Error with findind connection between list and games");

            Context.GamingLists.Remove(list);
            Context.Game_GamingLists.Remove(ggList);

            await Context.SaveChangesAsync();

            return Ok("Successfully deleted gaming list");

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


}