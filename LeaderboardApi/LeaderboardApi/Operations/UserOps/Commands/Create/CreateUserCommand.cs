using AutoMapper;
using LeaderboardApi.DbOperations;
using LeaderboardApi.Entities;
using LeaderboardApi.TokenOperations;
using Microsoft.AspNetCore.Identity;

namespace LeaderboardApi.Operations.UserOps.Commands.Create;

public class CreateUserCommand
{
    public CreateUserInputModel Model { get; set; }
    
    private readonly ILeaderboardDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly TokenHandler _tokenHandler;
    private readonly PasswordHasher<User> _passwordHasher;
    
    public CreateUserCommand(ILeaderboardDbContext dbContext, IMapper mapper, TokenHandler tokenHandler)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _tokenHandler = tokenHandler;
        _passwordHasher = new PasswordHasher<User>();
    }
    
    public async Task<TokenDto> Handle()
    {
        var user = _mapper.Map<User>(Model);
        
        user.CreatedAt = DateTime.UtcNow;
        user.PasswordHash = _passwordHasher.HashPassword(user, Model.Password);
        
        // TODO: Create token for the user
        var token = _tokenHandler.GenerateToken(user);
        user.RefreshToken = token.RefreshToken;
        user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);
        
        _dbContext.Users.Add(user);
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