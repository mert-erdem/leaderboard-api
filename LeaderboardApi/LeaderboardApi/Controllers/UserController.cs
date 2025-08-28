using AutoMapper;
using FluentValidation;
using LeaderboardApi.DbOperations;
using LeaderboardApi.Entities;
using LeaderboardApi.Operations.UserOps.Commands.Create;
using LeaderboardApi.Operations.UserOps.Commands.Login;
using LeaderboardApi.Operations.UserOps.Commands.Logout;
using LeaderboardApi.Operations.UserOps.Commands.Refresh;
using LeaderboardApi.TokenRelated;
using Microsoft.AspNetCore.Authorization;
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
    [AllowAnonymous]
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
    [AllowAnonymous]
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

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutUserCommand.LogoutUserViewModel model)
    {
        var command = new LogoutUserCommand(_dbContext)
        {
            Model = model
        };
        
        var validator = new LogoutUserCommandValidator();
        await validator.ValidateAndThrowAsync(command);

        await command.Handle();
        
        return Ok("User logged out!");
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] string refreshToken)
    {
        var command = new RefreshSessionCommand(_dbContext, _tokenHandler)
        {
            RefreshToken = refreshToken
        };
        
        var validator = new RefreshSessionCommandValidator();
        await validator.ValidateAndThrowAsync(command);
        
        var result = await command.Handle();
        
        return Ok(result);
    }
}