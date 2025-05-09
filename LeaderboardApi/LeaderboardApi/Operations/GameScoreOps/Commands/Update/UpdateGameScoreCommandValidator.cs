using FluentValidation;

namespace LeaderboardApi.Operations.GameScoreOps.Commands;

public class UpdateGameScoreCommandValidator : AbstractValidator<UpdateGameScoreCommand>
{
    public UpdateGameScoreCommandValidator()
    {
        RuleFor(x => x.Model.Score).LessThan(double.MaxValue);
    }
}