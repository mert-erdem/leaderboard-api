using LeaderboardApi.DbOperations;
using LeaderboardApi.Entities;
using LeaderboardApi.TokenRelated;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LeaderboardApi.Operations.UserOps.Commands.Login;

public class LoginUserCommand
{
    public UserLoginModel Model { get; set; }
    
    private readonly ILeaderboardDbContext _dbContext;
    private readonly TokenHandler _tokenHandler;
    private readonly PasswordHasher<User> _passwordHasher;

    public LoginUserCommand(ILeaderboardDbContext dbContext, TokenHandler tokenHandler, PasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;
        _tokenHandler = tokenHandler;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<TokenDto> Handle()
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == Model.Email);

        // user's mail has checked by validator already
        var result = _passwordHasher.VerifyHashedPassword(user!, user!.PasswordHash, Model.Password);

        switch (result)
        {
            case PasswordVerificationResult.Failed:
                throw new InvalidOperationException("Email or password is not correct!");
            case PasswordVerificationResult.SuccessRehashNeeded:
                user.PasswordHash = _passwordHasher.HashPassword(user, Model.Password);
                await _dbContext.SaveAsync();
                break;
        }

        var token = _tokenHandler.GenerateToken(user);
        user.RefreshToken = token.RefreshToken;
        
        await _dbContext.SaveAsync();
        
        var tokenDto = new TokenDto(token.AccessToken, token.RefreshToken);
        
        return tokenDto;
    }
    
    public class UserLoginModel
    {
        public required string Email { get; set; }
        
        public required string Password { get; set; }
    }
}