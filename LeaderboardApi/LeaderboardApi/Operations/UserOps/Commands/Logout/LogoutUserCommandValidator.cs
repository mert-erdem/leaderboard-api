using FluentValidation;

namespace LeaderboardApi.Operations.UserOps.Commands.Logout;

public class LogoutUserCommandValidator : AbstractValidator<LogoutUserCommand>
{
    public LogoutUserCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.")
            .MaximumLength(2048).WithMessage("Refresh token is too long.");
    }
}
