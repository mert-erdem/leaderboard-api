using FluentValidation;

namespace LeaderboardApi.Operations.GameScoreOps.Queries.GetGameScore;

public class GetGameScoreQueryValidator : AbstractValidator<GetGameScoreQuery>
{
    public GetGameScoreQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .When(x => x.Id.HasValue); // when id set
        
        RuleFor(x => x.GameId)
            .GreaterThan(0)
            .When(x => x.GameId.HasValue);
        
        RuleFor(x => x.TopCount)
            .GreaterThan(0)
            .LessThanOrEqualTo(1000)
            .When(x => x.TopCount.HasValue);
    }
}