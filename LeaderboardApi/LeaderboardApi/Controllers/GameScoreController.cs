using AutoMapper;
using FluentValidation;
using LeaderboardApi.DbOperations;
using LeaderboardApi.Operations.GameScoreOps.Commands;
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

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var query = new GetGameScoreQuery(_dbContext, _mapper)
        {
            Id = id
        };
        
        var validator = new GetGameScoreQueryValidator();
        validator.ValidateAndThrow(query);
        
        var result = query.HandleWithId();
        
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Add([FromBody] CreateGameScoreCommand.GameScoreInputModel model)
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
}