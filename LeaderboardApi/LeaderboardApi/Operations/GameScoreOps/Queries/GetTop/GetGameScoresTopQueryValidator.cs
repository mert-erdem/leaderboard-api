using FluentValidation;

namespace LeaderboardApi.Operations.GameScoreOps.Queries.GetTop;

public class GetGameScoresTopQueryValidator: AbstractValidator<GetGameScoresTopQuery>
{
    public GetGameScoresTopQueryValidator()
    {
        RuleFor(x => x.GameId)
            .GreaterThan(0);
        
        RuleFor(x => x.TopCount)
            .GreaterThan(0)
            .LessThanOrEqualTo(1000);
    }
}