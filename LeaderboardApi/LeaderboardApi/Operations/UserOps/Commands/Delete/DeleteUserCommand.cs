using LeaderboardApi.DbOperations;
using Microsoft.EntityFrameworkCore;

namespace LeaderboardApi.Operations.UserOps.Commands.Delete;

public class DeleteUserCommand
{
    public int Id { get; init; }
    
    private readonly ILeaderboardDbContext _dbContext;

    public DeleteUserCommand(ILeaderboardDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle()
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == Id);

        if (user is null)
        {
            throw new InvalidOperationException("User not found!");
        }

        _dbContext.Users.Remove(user);
        
        await _dbContext.SaveAsync();
    }
}