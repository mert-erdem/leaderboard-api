using AutoMapper;
using LeaderboardApi.DbOperations;
using LeaderboardApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeaderboardApi.Operations.GameScoreOps.Commands;

public class UpdateGameScoreCommand
{
    public int Id { get; set; }
    public UpdateGameScoreInputModel Model { get; set; }
    
    private readonly ILeaderboardDbContext _dbContext;
    private readonly IMapper _mapper;

    public UpdateGameScoreCommand(ILeaderboardDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task Handle()
    {
        var gameScore = await _dbContext.GameScores
            .SingleOrDefaultAsync(x => x.Id == Id);

        if (gameScore is null)
        {
            throw new InvalidOperationException("Game score not found!");
        }
        
        _mapper.Map(Model, gameScore);
        gameScore.LastEditedTime = DateTime.Now;

        await _dbContext.SaveAsync();
    }

    public class UpdateGameScoreInputModel
    {
        public double Score { get; set; }
    }
}