using FluentValidation;

namespace LeaderboardApi.Operations.UserOps.Commands.Refresh;

public class RefreshSessionCommandValidator : AbstractValidator<RefreshSessionCommand>
{
    public RefreshSessionCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.")
            .MaximumLength(2048).WithMessage("Refresh token is too long.");
    }
}