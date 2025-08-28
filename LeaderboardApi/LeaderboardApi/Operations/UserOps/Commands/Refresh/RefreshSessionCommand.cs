using LeaderboardApi.DbOperations;
using LeaderboardApi.TokenRelated;
using Microsoft.EntityFrameworkCore;

namespace LeaderboardApi.Operations.UserOps.Commands.Refresh;

public class RefreshSessionCommand
{
    public string RefreshToken { get; set; }
    
    private readonly ILeaderboardDbContext _dbContext;
    private readonly TokenHandler _tokenHandler;

    public RefreshSessionCommand(ILeaderboardDbContext dbContext, TokenHandler tokenHandler)
    {
        _dbContext = dbContext;
        _tokenHandler = tokenHandler;
    }
    
    public async Task<TokenDto> Handle()
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.RefreshToken == RefreshToken);

        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid refresh token!");
        }
        
        var token = _tokenHandler.GenerateToken(user);
        user.RefreshToken = token.RefreshToken;
        user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);
        
        await _dbContext.SaveAsync();
        
        var tokenDto = new TokenDto(token.AccessToken, token.RefreshToken);
        
        return tokenDto;
    }
}