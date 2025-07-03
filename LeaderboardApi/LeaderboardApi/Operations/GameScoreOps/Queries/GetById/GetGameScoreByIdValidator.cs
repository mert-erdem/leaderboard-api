using FluentValidation;

namespace LeaderboardApi.Operations.GameScoreOps.Queries.GetById;

public class GetGameScoreByIdValidator : AbstractValidator<GetGameScoreById>
{
    public GetGameScoreByIdValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}