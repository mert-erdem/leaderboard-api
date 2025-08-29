using AutoMapper;
using LeaderboardApi.DbOperations;
using LeaderboardApi.Entities;
using LeaderboardApi.TokenRelated;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LeaderboardApi.Operations.UserOps.Commands.Create;

public class CreateUserCommand
{
    public CreateUserInputModel Model { get; set; }
    
    private readonly ILeaderboardDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly TokenHandler _tokenHandler;
    private readonly PasswordHasher<User> _passwordHasher;
    
    public CreateUserCommand(ILeaderboardDbContext dbContext, IMapper mapper, TokenHandler tokenHandler, 
        PasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _tokenHandler = tokenHandler;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<TokenDto> Handle()
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == Model.Email);

        if (user is not null)
        {
            throw new InvalidOperationException("User with given email already exists!");
        }
        
        var newUser = _mapper.Map<User>(Model);
        
        newUser.CreatedAt = DateTime.UtcNow;
        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, Model.Password);
        
        // TODO: Create token for the user
        var token = _tokenHandler.GenerateToken(newUser);
        newUser.RefreshToken = token.RefreshToken;
        
        _dbContext.Users.Add(newUser);
        await _dbContext.SaveAsync();
        
        var tokenDto = new TokenDto(token.AccessToken, token.RefreshToken);
        
        return tokenDto;
    }
    
    public class CreateUserInputModel
    {
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}