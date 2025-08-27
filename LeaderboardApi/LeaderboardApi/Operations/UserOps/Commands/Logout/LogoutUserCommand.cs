using LeaderboardApi.DbOperations;
using Microsoft.EntityFrameworkCore;

namespace LeaderboardApi.Operations.UserOps.Commands.Logout;

public class LogoutUserCommand
{
    public LogoutUserViewModel Model { get; set; }
    
    private readonly ILeaderboardDbContext _dbContext;

    public LogoutUserCommand(ILeaderboardDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Handle()
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.RefreshToken == Model.RefreshToken);

        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid refresh token!");
        }

        user.RefreshToken = null;
        user.RefreshTokenExpireTime = null;

        await _dbContext.SaveAsync();
    }

    public class LogoutUserViewModel
    {
        public required string RefreshToken { get; set; }
    }
}