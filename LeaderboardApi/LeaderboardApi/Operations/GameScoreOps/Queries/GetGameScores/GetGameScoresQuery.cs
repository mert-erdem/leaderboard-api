using AutoMapper;
using LeaderboardApi.DbOperations;
using LeaderboardApi.Operations.GameScoreOps.Queries.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LeaderboardApi.Operations.GameScoreOps.Queries.GetGameScores;

public class GetGameScoresQuery
{
    public int GameId { get; set; }
    
    private readonly ILeaderboardDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetGameScoresQuery(ILeaderboardDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<List<GameScoreViewModel>> Handle()
    {
        var gameScores = await _dbContext.GameScores
            .Include(x => x.Game)
            .Include(x => x.Player)
            .Where(x => x.GameId == GameId)
            .OrderByDescending(x => x.Score)
            .ToListAsync();

        if (gameScores.Count == 0)
        {
            throw new InvalidOperationException("No games found with given game ID!");
        }

        var gameScoreViewModels = _mapper.Map<List<GameScoreViewModel>>(gameScores);
        
        return gameScoreViewModels;
    }
}