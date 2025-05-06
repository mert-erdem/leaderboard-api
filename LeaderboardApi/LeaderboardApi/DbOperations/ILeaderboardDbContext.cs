using LeaderboardApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeaderboardApi.DbOperations;

public interface ILeaderboardDbContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Player> Players { get; set; }

    public DbSet<Game> Games { get; set; }

    public DbSet<GameScore> GameScores { get; set; }

    int Save();
}