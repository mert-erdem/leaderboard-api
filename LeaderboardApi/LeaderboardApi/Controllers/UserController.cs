using AutoMapper;
using FluentValidation;
using LeaderboardApi.DbOperations;
using LeaderboardApi.Operations.UserOps.Commands;
using Microsoft.AspNetCore.Mvc;

namespace LeaderboardApi.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class UserController : Controller
{
    private readonly ILeaderboardDbContext _dbContext;
    private readonly IMapper _mapper;

    public UserController(ILeaderboardDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateUserCommand.CreateUserInputModel model)
    {
        var command = new CreateUserCommand(_dbContext, _mapper)
        {
            Model = model
        };
        
        var validator = new CreateUserCommandValidator(_dbContext);
        await validator.ValidateAndThrowAsync(command);
        
        await command.Handle();
        
        return Ok("User created!");
    }
}