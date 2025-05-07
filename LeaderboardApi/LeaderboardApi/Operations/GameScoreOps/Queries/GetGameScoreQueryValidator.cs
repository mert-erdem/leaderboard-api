using FluentValidation;

namespace LeaderboardApi.Operations.GameScoreOps.Queries;

public class GetGameScoreQueryValidator : AbstractValidator<GetGameScoreQuery>
{
    public GetGameScoreQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}