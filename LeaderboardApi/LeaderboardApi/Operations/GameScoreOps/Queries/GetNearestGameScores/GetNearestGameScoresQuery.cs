using AutoMapper;
using LeaderboardApi.DbOperations;
using LeaderboardApi.Operations.GameScoreOps.Queries.ViewModels;
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

    public async Task<List<GameScoreWithRankViewModel>> Handle()
    {
        // Retrieve game scores descending
        var gameScores = await _dbContext.GameScores
            .Include(x => x.Player)
            .Include(x => x.Game)
            .Where(x => x.Game!.Id == QueryProps.GameId)
            .OrderByDescending(x => x.Score)
            .ToListAsync();

        // Find the index of the player's score
        var playerIndex = gameScores.FindIndex(x => x.Player!.Id == QueryProps.PlayerId);
        if (playerIndex == -1)
        {
            throw new InvalidOperationException("No player score found for given game!");
        }

        // Calculate the range to include scores
        int aboveIndex = Math.Max(0, playerIndex - QueryProps.CountAbove);
        int belowIndex = Math.Min(gameScores.Count - 1, playerIndex + QueryProps.CountBelow);

        // Extract the relevant range of scores and map them
        var selectedGameScores = gameScores
            .Skip(aboveIndex)
            .Take(belowIndex - aboveIndex + 1)
            .Select((gameScore, index) =>
            {
                var viewModel = _mapper.Map<GameScoreWithRankViewModel>(gameScore);
                viewModel.Rank = aboveIndex + index + 1; // Rank is 1-based
                return viewModel;
            })
            .ToList();

        return selectedGameScores;
    }
    
    public record NearestScoresQueryProps(int PlayerId, int GameId, int CountAbove, int CountBelow);
}