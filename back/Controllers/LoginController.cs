using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using GameHeaven.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace GameHeaven.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    public Context Context;
    private IConfiguration configuration;

    public LoginController(Context context, IConfiguration config)
    {
        Context = context;
        configuration = config;
    }


    [AllowAnonymous]
    [HttpGet("LoginPlayer/{Email}/{Password}")]
    public async Task<ActionResult> LoginUser(string Email, string Password)
    {
        try
        {
            var player = await Context.Players.Where(p => p.Email == Email).FirstOrDefaultAsync();

            if (player == null)
                return BadRequest("Error with email");

            if (VerifyPassword(Password, player.Password, player.Salt))
                return Ok(player);
            else
                return BadRequest("Wrong password");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    private bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
    {
        if (password == null) throw new ArgumentNullException("password");
        if (String.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or be white space", "password");
        if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash", "passwordHash");
        if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt", "passwordHash");

        using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
                if (computedHash[i] != storedHash[i]) return false;
        }

        return true;
    }


    [HttpPost("GetToken")]
    public async Task<ActionResult> GetToken([FromBody] UserAuth player)
    {
        try
        {
            var u = await Context.Players.Where(p => p.Email == player.Email).FirstOrDefaultAsync();

            if (u == null)
                return BadRequest(null);

            if (VerifyPassword(player.Password, u.Password, u.Salt))
            {
                var token = Generate(u);
                return Ok(new { token = token });
            }
            else
            {
                return Ok(null);
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    private object Generate(Player p)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]{
            new Claim(ClaimTypes.NameIdentifier,p.UserName),
            new Claim(ClaimTypes.Email,p.Email),
            new Claim(ClaimTypes.Sid,p.ID.ToString())
        };

        var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    [AllowAnonymous]
    [HttpPost("SignUp/{Name}/{Email}/{Password}/{ConformPassword}")]
    public async Task<ActionResult> SignUpUser(string Name, string Email, string Password, string ConformPassword)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Name) && Name.Length > 20)
                return BadRequest("You have to enter a name shorter than 20 characters");
            if (string.IsNullOrWhiteSpace(Email))
                return BadRequest("You have to enter email");
            if (string.IsNullOrWhiteSpace(Password))
                return BadRequest("You have to enter a password");
            if (string.IsNullOrWhiteSpace(ConformPassword))
                return BadRequest("You have to enter a password again");
            if (ConformPassword != Password)
                return BadRequest("Password do not match");

            foreach (var p in Context.Players.ToList())
            {
                if (p.UserName.CompareTo(Name) == 0)
                    return BadRequest("That user name is already taken");
                if (p.Email.CompareTo(Email) == 0)
                    return BadRequest("Profile with this email already exist");
            }

            Player player = new Player
            {
                UserName = Name,
                Email = Email
            };

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(Password, out passwordHash, out passwordSalt);
            player.Password = passwordHash;
            player.Salt = passwordSalt;

            await Context.Players.AddAsync(player);
            await Context.SaveChangesAsync();

            var verify = await Context.Players.Where(p => p.Email == Email).FirstOrDefaultAsync();

            try
            {
                Verification(verify);
            }
            catch (Exception)
            {
                return BadRequest("Email address is not valid");
            }

            return Ok(null);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        if (password == null) throw new ArgumentNullException("password");
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or white space");

        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private static async void Verification(Player player)
    {
        string message;
        message = $"{player.UserName} \n Welcome to the Game Heaven community.\n\n With respect, \n Game Heaven";

        SmtpClient Client = new SmtpClient()
        {
            Host = "smtp.outlook.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential()
            {
                UserName = "gameHeaven2024@outlook.com",
                Password = "tester123"
            }
        };

        MailAddress fromMail = new MailAddress("gameHeaven2024@outlook.com", "Game Heaven");
        MailAddress toMail = new MailAddress(player.Email, player.UserName);
        MailMessage mesg = new MailMessage()
        {
            From = fromMail,
            Subject = "Welcoming mail",
            Body = message
        };

        mesg.To.Add(toMail);
        await Client.SendMailAsync(mesg);
    }

}