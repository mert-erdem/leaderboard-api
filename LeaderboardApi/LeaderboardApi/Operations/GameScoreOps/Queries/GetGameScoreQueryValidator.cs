using FluentValidation;

namespace LeaderboardApi.Operations.GameScoreOps.Queries;

public class GetGameScoreQueryValidator : AbstractValidator<GetGameScoreQuery>
{
    public GetGameScoreQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .When(x => x.Id != 0); // when id set
        
        RuleFor(x => x.TopCount)
            .GreaterThan(0)
            .LessThanOrEqualTo(1000)
            .When(x => x.TopCount != 0);
    }
}