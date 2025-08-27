using AutoMapper;
using FluentValidation;
using LeaderboardApi.DbOperations;
using LeaderboardApi.Operations.UserOps.Commands;
using LeaderboardApi.TokenOperations;
using Microsoft.AspNetCore.Mvc;

namespace LeaderboardApi.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class UserController : Controller
{
    private readonly ILeaderboardDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly TokenHandler _tokenHandler;

    public UserController(ILeaderboardDbContext dbContext, IMapper mapper, TokenHandler tokenHandler)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _tokenHandler = tokenHandler;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserCommand.CreateUserInputModel model)
    {
        var command = new CreateUserCommand(_dbContext, _mapper, _tokenHandler)
        {
            Model = model
        };
        
        var validator = new CreateUserCommandValidator(_dbContext);
        await validator.ValidateAndThrowAsync(command);
        
        var result = await command.Handle();
        
        return Ok(result);
    }
}