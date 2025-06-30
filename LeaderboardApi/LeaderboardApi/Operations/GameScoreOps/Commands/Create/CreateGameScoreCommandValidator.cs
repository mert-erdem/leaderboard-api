using FluentValidation;
using LeaderboardApi.Operations.GameScoreOps.Commands.Create;

namespace LeaderboardApi.Operations.GameScoreOps.Commands;

public class CreateGameScoreCommandValidator : AbstractValidator<CreateGameScoreCommand>
{
    public CreateGameScoreCommandValidator()
    {
        RuleFor(x => x.Model.PlayerId).GreaterThan(0);
        RuleFor(x => x.Model.GameId).GreaterThan(0);
        RuleFor(x => x.Model.Score).GreaterThan(0).LessThan(double.MaxValue);
    }
}