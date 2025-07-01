using FluentValidation;

namespace LeaderboardApi.Operations.GameScoreOps.Queries;

public class GetNearestGameScoresQueryValidator : AbstractValidator<GetNearestGameScoresQuery.NearestScoresQueryProps>
{
    public GetNearestGameScoresQueryValidator()
    {
        RuleFor(x => x.PlayerId).GreaterThan(0);
        RuleFor(x => x.GameId).GreaterThan(0);
        RuleFor(x => x.CountBelow).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CountAbove).GreaterThanOrEqualTo(0);
    }
}