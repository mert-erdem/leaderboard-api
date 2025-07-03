using AutoMapper;
using LeaderboardApi.DbOperations;
using LeaderboardApi.Operations.GameScoreOps.Queries.GetGameScore;
using Microsoft.EntityFrameworkCore;
using InvalidOperationException = System.InvalidOperationException;

namespace LeaderboardApi.Operations.GameScoreOps.Queries.GetNearestGameScores;

public class GetNearestGameScoresQuery
{
    public NearestScoresQueryProps QueryProps { get; set; }
    
    private readonly ILeaderboardDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetNearestGameScoresQuery(ILeaderboardDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<GetGameScoreQuery.GameScoreViewModel>> Handle()
    {
        var playerGameScore = await _dbContext.GameScores
            .Include(x => x.Player)
            .Where(x => x.Game!.Id == QueryProps.GameId && x.Player!.Id == QueryProps.PlayerId)
            .SingleOrDefaultAsync();

        if (playerGameScore == null)
        {
            throw new InvalidOperationException("No player score found for given game!");
        }
        
        // Higher scores
        var aboveScores = await _dbContext.GameScores
            .Include(x => x.Player)
            .Include(x => x.Game)
            .Where(x => x.Game!.Id == QueryProps.GameId && x.Score > playerGameScore.Score)
            .OrderBy(s => s.Score) // closest higher first
            .Take(QueryProps.CountAbove)
            .ToListAsync();

        // Lower scores
        var belowScores = await _dbContext.GameScores
            .Include(x => x.Player)
            .Include(x => x.Game)
            .Where(x => x.Game!.Id == QueryProps.GameId && x.Score < playerGameScore.Score)
            .OrderByDescending(s => s.Score) // closest lower first
            .Take(QueryProps.CountBelow)
            .ToListAsync();
        
        var leaderboardWindow = new List<GetGameScoreQuery.GameScoreViewModel>();
        var aboveScoresViewModel = _mapper.Map<List<GetGameScoreQuery.GameScoreViewModel>>(aboveScores);
        var playerScoreViewModel = _mapper.Map<GetGameScoreQuery.GameScoreViewModel>(playerGameScore);
        var belowScoresViewModel = _mapper.Map<List<GetGameScoreQuery.GameScoreViewModel>>(belowScores);

        // Sort higher scores descending (higher scores above player)
        leaderboardWindow.AddRange(aboveScoresViewModel.OrderByDescending(s => s.Score));

        // Add the player's own score
        leaderboardWindow.Add(playerScoreViewModel);

        // Add lower scores descending
        leaderboardWindow.AddRange(belowScoresViewModel.OrderByDescending(s => s.Score));

        return leaderboardWindow;
    }
    
    public record NearestScoresQueryProps(int PlayerId, int GameId, int CountAbove, int CountBelow);
}