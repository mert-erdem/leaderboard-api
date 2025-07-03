using FluentValidation;

namespace LeaderboardApi.Operations.GameScoreOps.Queries.GetGameScore;

public class GetGameScoresQueryValidator : AbstractValidator<GetGameScoresQuery>
{
    public GetGameScoresQueryValidator()
    {
        RuleFor(x => x.GameId)
            .GreaterThan(0);
    }
}