using LeaderboardApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeaderboardApi.DbOperations;

public static class DataGenerator
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        var dbContext = new LeaderboardDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<LeaderboardDbContext>>());
        
        dbContext.Games.AddRange(
            new Game
            {
                Id = 1,
                Name = "Snake"
            },
            new Game
            {
                Id = 2,
                Name = "Tetris"
            },
            new Game
            {
                Id = 3,
                Name = "Bomberman"
            }
            );

        var users = new List<User>();
        int userCount = 100;

        for (int i = 1; i <= userCount; i++)
        {
            users.Add(new User
            {
                Username = $"user{i}",
                Email = $"user{i}@example.com",
                PasswordHash = $"hashed_password_{i}"
            });
        }

        dbContext.Users.AddRange(users);
        
        var players = new List<Player>();

        for (int i = 1; i <= userCount; i++)
        {
            players.Add(new Player
            {
                Id = i,
                UserId = i,
                Name = $"Player{i}"
            });
        }

        dbContext.Players.AddRange(players);

        var usedPairs = new HashSet<(int gameId, int playerId)>();
        var random = new Random();
        int id = 1;

        while (usedPairs.Count < userCount)
        {
            int gameId = random.Next(1, 4);
            int playerId = random.Next(1, 101);
            var pair = (gameId, playerId);

            if (!usedPairs.Add(pair))
                continue;

            var gameScore = new GameScore
            {
                Id = id++,
                GameId = gameId,
                PlayerId = playerId,
                Score = random.Next(0, 1000),
                LastEditedTime = DateTime.Now
            };

            dbContext.GameScores.Add(gameScore);
        }
        
        dbContext.SaveChanges();
    }
}