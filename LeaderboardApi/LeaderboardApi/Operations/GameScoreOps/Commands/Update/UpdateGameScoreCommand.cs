using AutoMapper;
using LeaderboardApi.DbOperations;
using LeaderboardApi.Entities;

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

    public void Handle()
    {
        var gameScore = _dbContext.GameScores
            .SingleOrDefault(x => x.Id == Id);

        if (gameScore is null)
        {
            throw new InvalidOperationException("Game score not found!");
        }
        
        _mapper.Map(Model, gameScore);
        gameScore.LastEditedTime = DateTime.Now;

        _dbContext.Save();
    }

    public class UpdateGameScoreInputModel
    {
        public double Score { get; set; }
    }
}