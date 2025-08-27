using FluentValidation;
using LeaderboardApi.DbOperations;

namespace LeaderboardApi.Operations.UserOps.Commands.Login;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator(ILeaderboardDbContext dbContext)
    {
        RuleFor(x => x.Model.Email)
            .NotEmpty()
            .EmailAddress()
            .Must(email => dbContext.Users.Any(x => x.Email == email))
            .WithMessage("User or password is incorrect!");

        RuleFor(x => x.Model.Password)
            .NotEmpty()
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long!")
            .MaximumLength(64).WithMessage("Password must be no longer than 64 characters!")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter!")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter!")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit!")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character!");
    }
}