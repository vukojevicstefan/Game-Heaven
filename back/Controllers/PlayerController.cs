using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace GameHeaven.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayerController : ControllerBase
{
    public Context Context;

    public PlayerController(Context context)
    {
        Context = context;
    }

    [HttpGet("GetCurrentUserData")]
    public async Task<ActionResult> GetCurrentUserData()
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

            var player = await Context.Players.Where(p => p.ID == id).Include(p => p.ReviewsOfPlayer).FirstOrDefaultAsync();

            if (player == null)
                return BadRequest("Error with getting data about current user");

            return Ok(player);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}