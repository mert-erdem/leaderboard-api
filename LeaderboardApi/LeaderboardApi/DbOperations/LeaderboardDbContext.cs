using LeaderboardApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeaderboardApi.DbOperations;

public class LeaderboardDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Player> Players { get; set; }

    public DbSet<Game> Games { get; set; }

    public DbSet<GameScore> GameScores { get; set; }
    
    public LeaderboardDbContext(DbContextOptions<LeaderboardDbContext> options) : base(options)
    {
    }

    public new int SaveChanges()
    {
        return base.SaveChanges();
    }
}