using AutoMapper;
using LeaderboardApi.DbOperations;
using LeaderboardApi.Operations.GameScoreOps.Queries.GetGameScore;
using Microsoft.EntityFrameworkCore;

namespace LeaderboardApi.Operations.GameScoreOps.Queries.GetById;

public class GetGameScoreById
{
    public int Id { get; set; }
    
    private readonly ILeaderboardDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetGameScoreById(ILeaderboardDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public GetGameScoresQuery.GameScoreViewModel Handle()
    {
        var gameScore = _dbContext.GameScores
            .Include(x => x.Game)
            .Include(x => x.Player)
            .SingleOrDefault(x => x.Id == Id);

        if (gameScore is null)
        {
            throw new InvalidOperationException("Could not find game score with given game ID!");
        }

        var gameScoreViewModel = _mapper.Map<GetGameScoresQuery.GameScoreViewModel>(gameScore);
        
        return gameScoreViewModel;
    }
}