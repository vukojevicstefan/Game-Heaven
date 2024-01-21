using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models;

public class GamingList
{
    [Key]
    public int ID { get; set; }
    public string? ListName { get; set; }
    public List<Game_GamingList> GamesInGamingList { get; set; } = null!;
    [JsonIgnore]
    public Player CreatorOfGamingList { get; set; } = null!;
}