using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace GameHeaven.Controllers;

[ApiController]
[Route("[controller]")]
public class ReviewController : ControllerBase
{
    public Context Context;

    public ReviewController(Context context)
    {
        Context = context;
    }

    [HttpGet("GetReviewsOfGame/{gameTitle}")]
    public async Task<ActionResult> GetReviewsByGameTitle(string gameTitle)
    {
        try
        {
            if (string.IsNullOrEmpty(gameTitle))
            {
                return BadRequest("There is no game name entered");
            }

            var reviews = await Context.Reviews
                .Where(r => r.ReviewedGame.Title == gameTitle)
                .ToListAsync();

            if (reviews == null || reviews.Count == 0)
            {
                return NotFound();
            }

            float average = reviews.Average(r => r.Rating);

            return Ok(reviews.Select(r => new
            {
                r.Rating,
                r.Comment,
                AverageRating = average
            }));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    [HttpPost("PostReview/{comment}/{grade}/{gameTitle}")]
    public async Task<ActionResult> CreateReview(string comment, int grade, string gameTitle)
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
                return BadRequest("Error with getting data for current player!");
            }

            var userClaims = identity.Claims;
            int id = int.Parse(userClaims.FirstOrDefault(p => p.Type == ClaimTypes.Sid)!.Value);

            var player = await Context.Players.Where(p => p.ID == id).Include(p => p.ReviewsOfPlayer).FirstOrDefaultAsync();

            if (player == null)
                return BadRequest("Error with getting player from database!");

            if (string.IsNullOrEmpty(comment))
                return BadRequest("Comment needs to be entered");

            if (grade < 0 || grade > 5)
                return BadRequest("Grade must be between 0 and 5");

            if (string.IsNullOrEmpty(gameTitle))
                return BadRequest("Back didn't get title of the game");

            var game = await Context.Games.Where(game => game.Title == gameTitle).Include(g => g.ReviewsOfGame).FirstOrDefaultAsync();

            if (game == null)
                return BadRequest("There is no game with that title");


            var checkReview = await Context.Reviews.Where(r => r.CreatorOfReview == player && r.ReviewedGame == game).FirstOrDefaultAsync();    //nisam siguran dal mora sa ID proveri

            if (checkReview != null)
                return BadRequest("You already posted a review for this game");

            Review review = new Review
            {
                Comment = comment,
                Rating = grade,
                ReviewedGame = game,
                CreatorOfReview = player
            };

            await Context.Reviews.AddAsync(review);

            if (player.ReviewsOfPlayer == null)
                player.ReviewsOfPlayer = new List<Review>();

            player.ReviewsOfPlayer.Add(review);

            Context.Update(player);

            if (game.ReviewsOfGame == null)
                game.ReviewsOfGame = new List<Review>();

            game.ReviewsOfGame.Add(review);

            Context.Update(game);

            await Context.SaveChangesAsync();

            return Ok(review);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}