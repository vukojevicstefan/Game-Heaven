using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models;

public class Game
{
    [Key]
    public int ID { get; set; }
    public string? Title { get; set; }
    public Genre Genre { get; set; }
    public Platform Platform { get; set; }
    public string? Description { get; set; }
    public string? Image    {get; set; }
    public List<Review>? ReviewsOfGame { get; set; }
    [JsonIgnore]
    public List<Game_GamingList>? GameListsOfGame { get; set; }
}

public enum Platform
{
    PlayStation,
    PC,
    Nintendo,
    XBox,
    Mobile
}

public enum Genre
{
    Action,
    Adventure,
    Horror,
    Rpg,
    Multiplayer,
    Singleplayer,
    Board,
    Platformer,
    Fps,
    Sports,
    BattleRoyal,
    Strategy,
    Mmo,
    Racing,
    Simulation,
    Puzzle,
    Fighting,
    HackAndSlash,
    Survivale,
    TwoD,
    ThreeD,
    Moba
}