using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models;

public class Review
{
    [Key]
    public int ID { get; set; }
    public float Rating { get; set; }
    public string? Comment { get; set; }
    [JsonIgnore]
    public Player? CreatorOfReview { get; set; }
    [JsonIgnore]
    public Game? ReviewedGame { get; set; }
}