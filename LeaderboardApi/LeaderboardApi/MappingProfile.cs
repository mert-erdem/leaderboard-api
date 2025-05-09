using AutoMapper;
using LeaderboardApi.Entities;
using LeaderboardApi.Operations.GameScoreOps.Commands;
using LeaderboardApi.Operations.GameScoreOps.Queries;

namespace LeaderboardApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<GameScore, GetGameScoreQuery.GameScoreViewModel>()
            .ForMember(
                dest => dest.GameName, 
                opt => opt
                    .MapFrom(src => src.Game!.Name))
            .ForMember(
                dest => dest.PlayerName,
                opt => opt
                    .MapFrom(src => src.Player!.Name));
        
        CreateMap<CreateGameScoreCommand.CreateGameScoreInputModel, GameScore>();

        CreateMap<UpdateGameScoreCommand.UpdateGameScoreInputModel, GameScore>();
    }
}