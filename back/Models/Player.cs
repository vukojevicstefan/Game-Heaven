using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models;

public class Player
{

    [Key]
    public int ID { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public byte[] Password { get; set; } = null!;
    public byte[] Salt { get; set; } = null!;
    public List<GamingList> GamingListsOfPlayer { get; set; } = null!;
    public List<Review> ReviewsOfPlayer { get; set; } = null!;
}