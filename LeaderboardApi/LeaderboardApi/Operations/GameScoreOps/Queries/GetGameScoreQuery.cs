using AutoMapper;
using LeaderboardApi.DbOperations;
using Microsoft.EntityFrameworkCore;

namespace LeaderboardApi.Operations.GameScoreOps.Queries;

public class GetGameScoreQuery
{
    public int Id { get; set; }

    public int? GameId { get; set; }

    public int TopCount { get; set; }
    
    private readonly ILeaderboardDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetGameScoreQuery(ILeaderboardDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public List<GameScoreViewModel> Handle()
    {
        var query = _dbContext.GameScores
            .Include(x => x.Game)
            .Include(x => x.Player)
            .AsQueryable();

        if (GameId != 0)
        {
            query = query.Where(x => x.GameId == GameId);

            if (!query.Any())
            {
                throw new InvalidOperationException("No games found with given game ID.");
            }
        }

        var gameScores = query
            .OrderByDescending(x => x.Score)
            .ToList();

        var gameScoreViewModels = _mapper.Map<List<GameScoreViewModel>>(gameScores);
        
        return gameScoreViewModels;
    }
    
    public GameScoreViewModel HandleWithId()
    {
        var gameScore = _dbContext.GameScores
            .Include(x => x.Game)
            .Include(x => x.Player)
            .SingleOrDefault(x => x.Id == Id);

        if (gameScore is null)
        {
            throw new InvalidOperationException("Could not find game score");
        }

        var gameScoreViewModel = _mapper.Map<GameScoreViewModel>(gameScore);
        
        return gameScoreViewModel;
    }

    public List<GameScoreViewModel> HandleTop()
    {
        var gameScores = _dbContext.GameScores
            .Include(x => x.Game)
            .Include(x => x.Player)
            .Where(x => x.Game!.Id == GameId)
            .OrderByDescending(x => x.Score)
            .Take(TopCount) // top border safe
            .ToList();
        
        var gameScoreViewModels = _mapper.Map<List<GameScoreViewModel>>(gameScores);
        
        return gameScoreViewModels;
    }

    public class GameScoreViewModel
    {
        public int Id { get; set; }
        
        public required string GameName { get; set; }

        public required string PlayerName { get; set; }

        public double Score { get; set; }

        public DateTime LastEditedTime { get; set; }
    }
}