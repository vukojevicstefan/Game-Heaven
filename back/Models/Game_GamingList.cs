using System.Text.Json.Serialization;

namespace Models;

public class Game_GamingList
{
    public int GameID { get; set; }
    public int GamingListID { get; set; }
    public Game? Game { get; set; }
    [JsonIgnore]
    public GamingList? GamingList { get; set; }

}