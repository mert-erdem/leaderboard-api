using AutoMapper;
using LeaderboardApi.DbOperations;
using LeaderboardApi.Operations.GameScoreOps.Queries.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LeaderboardApi.Operations.GameScoreOps.Queries.GetTop;

public class GetGameScoresTopQuery
{
    public int GameId { get; set; }

    public int TopCount { get; set; }
    
    private readonly ILeaderboardDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetGameScoresTopQuery(ILeaderboardDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public List<GameScoreViewModel> Handle()
    {
        var gameScores = _dbContext.GameScores
            .Include(x => x.Game)
            .Include(x => x.Player)
            .Where(x => x.Game!.Id == GameId)
            .OrderByDescending(x => x.Score)
            .Take(TopCount) // top border safe
            .ToList();

        if (gameScores.Count == 0)
        {
            throw new InvalidOperationException("No games found with given game ID!");
        }
        
        var gameScoreViewModels = _mapper.Map<List<GameScoreViewModel>>(gameScores);
        
        return gameScoreViewModels;
    }
}