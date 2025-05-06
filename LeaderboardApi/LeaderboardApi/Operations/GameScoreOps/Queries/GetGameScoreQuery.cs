using AutoMapper;
using LeaderboardApi.DbOperations;
using Microsoft.EntityFrameworkCore;

namespace LeaderboardApi.Operations.GameScoreOps.Queries;

public class GetGameScoreQuery
{
    private readonly ILeaderboardDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetGameScoreQuery(ILeaderboardDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public List<GameScoreViewModel> Handle()
    {
        var gameScores = _dbContext.GameScores
            .Include(x => x.Game)
            .Include(x => x.Player)
            .ToList()
            .OrderByDescending(x => x.Score);

        var gameScoreViewModels = _mapper.Map<List<GameScoreViewModel>>(gameScores);
        
        return gameScoreViewModels;
    }

    public class GameScoreViewModel
    {
        public required string GameName { get; set; }

        public required string PlayerName { get; set; }

        public double Score { get; set; }

        public DateTime LastEditedTime { get; set; }
    }
}