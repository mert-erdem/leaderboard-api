using FluentValidation;

namespace LeaderboardApi.Operations.GameScoreOps.Commands.Delete;

public class DeleteGameScoreCommandValidator : AbstractValidator<DeleteGameScoreCommand>
{
    public DeleteGameScoreCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}