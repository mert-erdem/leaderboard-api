using AutoMapper;
using LeaderboardApi.DbOperations;
using LeaderboardApi.Entities;

namespace LeaderboardApi.Operations.GameScoreOps.Commands.Create;

public class CreateGameScoreCommand
{
    public required CreateGameScoreInputModel Model { get; set; }
    
    private readonly ILeaderboardDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateGameScoreCommand(ILeaderboardDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public void Handle()
    {
        var gameScore = _dbContext.GameScores
            .SingleOrDefault(x => 
            x.GameId == Model.GameId && 
            x.PlayerId == Model.PlayerId);

        if (gameScore is not null)
        {
            throw new InvalidOperationException("Game score already exists!");
        }
        
        var gameScoreNew = _mapper.Map<GameScore>(Model);
        gameScoreNew.LastEditedTime = DateTime.Now;

        _dbContext.GameScores.Add(gameScoreNew);
        _dbContext.Save();
    }

    public class CreateGameScoreInputModel
    {
        public int GameId { get; set; }

        public int PlayerId { get; set; }

        public double Score { get; set; }
    }
}