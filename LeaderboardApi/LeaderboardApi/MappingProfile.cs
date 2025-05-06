using AutoMapper;
using LeaderboardApi.Entities;
using LeaderboardApi.Operations.GameScoreOps.Queries;

namespace LeaderboardApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<GameScore, GetGameScoreQuery.GameScoreViewModel>()
            .ForMember(
                x => x.GameName, 
                opt => opt
                    .MapFrom(src => src.Game!.Name))
            .ForMember(
                x => x.PlayerName,
                opt => opt
                    .MapFrom(src => src.Player!.Name));
    }
}