using AutoMapper;
using FluentValidation;
using LeaderboardApi.DbOperations;
using LeaderboardApi.Operations.GameScoreOps.Commands;
using LeaderboardApi.Operations.GameScoreOps.Commands.Create;
using LeaderboardApi.Operations.GameScoreOps.Commands.Delete;
using LeaderboardApi.Operations.GameScoreOps.Queries;
using LeaderboardApi.Operations.GameScoreOps.Queries.GetById;
using LeaderboardApi.Operations.GameScoreOps.Queries.GetGameScore;
using LeaderboardApi.Operations.GameScoreOps.Queries.GetNearestGameScores;
using LeaderboardApi.Operations.GameScoreOps.Queries.GetTop;
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
    public IActionResult GetAll([FromQuery] int gameId)
    {
        var query = new GetGameScoresQuery(_dbContext, _mapper)
        {
            GameId = gameId
        };

        var validator = new GetGameScoresQueryValidator();
        validator.ValidateAndThrow(query);
        
        var result = query.Handle();
        
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var query = new GetGameScoreById(_dbContext, _mapper)
        {
            Id = id
        };
        
        var validator = new GetGameScoreByIdValidator();
        validator.ValidateAndThrow(query);
        
        var result = query.Handle();
        
        return Ok(result);
    }

    [HttpGet("top")]
    public IActionResult GetTop([FromQuery] int gameId, [FromQuery] int count)
    {
        var query = new GetGameScoresTopQuery(_dbContext, _mapper)
        {
            GameId = gameId,
            TopCount = count
        };
        
        var validator = new GetGameScoresTopQueryValidator();
        validator.ValidateAndThrow(query);
        
        var result = query.Handle();
        
        return Ok(result);
    }

    [HttpGet("nearest")]
    public async Task<IActionResult> GetNearest([FromQuery] GetNearestGameScoresQuery.NearestScoresQueryProps queryProps)
    {
        var query = new GetNearestGameScoresQuery(_dbContext, _mapper)
        {
            QueryProps = queryProps
        };
        
        var validator = new NearestScoresQueryPropsValidator();
        await validator.ValidateAndThrowAsync(query.QueryProps);

        var result = await query.Handle();
        
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Add([FromBody] CreateGameScoreCommand.CreateGameScoreInputModel model)
    {
        var command = new CreateGameScoreCommand(_dbContext, _mapper)
        {
            Model = model
        };

        var validator = new CreateGameScoreCommandValidator();
        validator.ValidateAndThrow(command);
        
        command.Handle();
        
        return Ok("Game score added!");
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] UpdateGameScoreCommand.UpdateGameScoreInputModel model)
    {
        var command = new UpdateGameScoreCommand(_dbContext, _mapper)
        {
            Id = id,
            Model = model
        };
        
        var validator = new UpdateGameScoreCommandValidator();
        validator.ValidateAndThrow(command);
        
        command.Handle();
        
        return Ok("Game score updated!");
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var command = new DeleteGameScoreCommand(_dbContext)
        {
            Id = id
        };
        
        var validator = new DeleteGameScoreCommandValidator();
        validator.ValidateAndThrow(command);
        
        command.Handle();
        
        return Ok("Game score deleted!");
    }
}