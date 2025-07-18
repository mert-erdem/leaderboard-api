using AutoMapper;
using LeaderboardApi.DbOperations;
using LeaderboardApi.Entities;
using Microsoft.AspNetCore.Identity;

namespace LeaderboardApi.Operations.UserOps.Commands;

public class CreateUserCommand
{
    public CreateUserInputModel Model { get; set; }
    
    private readonly ILeaderboardDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly PasswordHasher<User> _passwordHasher;
    
    public CreateUserCommand(ILeaderboardDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _passwordHasher = new PasswordHasher<User>();
    }
    
    public async Task Handle()
    {
        var user = _mapper.Map<User>(Model);
        
        user.CreatedAt = DateTime.UtcNow;
        user.PasswordHash = _passwordHasher.HashPassword(user, Model.Password);
        
        // TODO: Create token for the user
        
        _dbContext.Users.Add(user);
        await _dbContext.SaveAsync();
    }
    
    public class CreateUserInputModel
    {
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}