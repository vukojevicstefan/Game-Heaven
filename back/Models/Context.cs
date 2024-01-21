using Microsoft.EntityFrameworkCore;

namespace Models;

public class Context : DbContext
{
    public Context(DbContextOptions options) : base(options) { }

    public DbSet<Player> Players { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<GamingList> GamingLists { get; set; }
    public DbSet<Game_GamingList> Game_GamingLists { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game_GamingList>().HasKey(gl => new { gl.GameID, gl.GamingListID });
        modelBuilder.Entity<Game_GamingList>().HasOne(gl => gl.Game).WithMany(g => g.GameListsOfGame).HasForeignKey(gl => gl.GameID);
        modelBuilder.Entity<Game_GamingList>().HasOne(gl => gl.GamingList).WithMany(g => g.GamesInGamingList).HasForeignKey(gl => gl.GamingListID);
    }
}