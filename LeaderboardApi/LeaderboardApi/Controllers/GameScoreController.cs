using AutoMapper;
using LeaderboardApi.DbOperations;
using LeaderboardApi.Operations.GameScoreOps.Queries;
using Microsoft.AspNetCore.Mvc;

namespace LeaderboardApi.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class GameScoreController : ControllerBase
{
    private readonly ILeaderboardDbContext _dbContext;
    private readonly IMapper _mapper;

    public GameScoreController(ILeaderboardDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var query = new GetGameScoreQuery(_dbContext, _mapper);
        var result = query.Handle();
        
        return Ok(result);
    }
}