using FluentValidation;

namespace LeaderboardApi.Operations.GameScoreOps.Queries.GetNearestGameScores;

public class NearestScoresQueryPropsValidator : AbstractValidator<GetNearestGameScoresQuery.NearestScoresQueryProps>
{
    public NearestScoresQueryPropsValidator()
    {
        RuleFor(x => x.PlayerId)
            .GreaterThan(0);
        
        RuleFor(x => x.GameId)
            .GreaterThan(0);
        
        RuleFor(x => x.CountBelow)
            .GreaterThanOrEqualTo(0);
        
        RuleFor(x => x.CountAbove)
            .GreaterThanOrEqualTo(0);
    }
}