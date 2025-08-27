using AutoMapper;
using FluentValidation;
using LeaderboardApi.DbOperations;
using LeaderboardApi.Entities;
using LeaderboardApi.Operations.UserOps.Commands.Create;
using LeaderboardApi.Operations.UserOps.Commands.Login;
using LeaderboardApi.TokenOperations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LeaderboardApi.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class UserController : Controller
{
    private readonly ILeaderboardDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly TokenHandler _tokenHandler;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserController(ILeaderboardDbContext dbContext, IMapper mapper, TokenHandler tokenHandler, PasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _tokenHandler = tokenHandler;
        _passwordHasher = passwordHasher;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserCommand.CreateUserInputModel model)
    {
        var command = new CreateUserCommand(_dbContext, _mapper, _tokenHandler, _passwordHasher)
        {
            Model = model
        };
        
        var validator = new CreateUserCommandValidator(_dbContext);
        await validator.ValidateAndThrowAsync(command);
        
        var result = await command.Handle();
        
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand.UserLoginModel model)
    {
        var command = new LoginUserCommand(_dbContext, _tokenHandler, _passwordHasher)
        {
            Model = model
        };

        var validator = new LoginUserCommandValidator(_dbContext);
        await validator.ValidateAndThrowAsync(command);

        var result = await command.Handle();
        
        return Ok(result);
    }
}