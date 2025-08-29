using LeaderboardApi.DbOperations;
using Microsoft.EntityFrameworkCore;

namespace LeaderboardApi.Operations.GameScoreOps.Commands.Delete;

public class DeleteGameScoreCommand
{
    public int Id { get; set; }
    
    private readonly ILeaderboardDbContext _dbContext;
    
    public DeleteGameScoreCommand(ILeaderboardDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle()
    {
        var gameScore = await _dbContext.GameScores.SingleOrDefaultAsync(x => x.Id == Id);

        if (gameScore is null)
        {
            throw new InvalidOperationException("No game score found with id: " + Id);
        }
        
        if (CheckIfPlayerExists(gameScore.PlayerId))
        {
            throw new InvalidOperationException("Game score that has player with id: " + 
                                                gameScore.PlayerId + 
                                                " can not be deleted");
        }
        
        _dbContext.GameScores.Remove(gameScore);
        
        await _dbContext.SaveAsync();
    }

    private bool CheckIfPlayerExists(int id)
    {
        return _dbContext.Players.Any(x => x.Id == id);
    }
}