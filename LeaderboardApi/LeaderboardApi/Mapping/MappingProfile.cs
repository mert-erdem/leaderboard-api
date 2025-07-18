using AutoMapper;
using LeaderboardApi.Entities;
using LeaderboardApi.Operations.GameScoreOps.Commands;
using LeaderboardApi.Operations.GameScoreOps.Commands.Create;
using LeaderboardApi.Operations.GameScoreOps.Queries.ViewModels;
using LeaderboardApi.Operations.UserOps.Commands;

namespace LeaderboardApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateGameScoreMaps();
        CreateUserMaps();
    }

    private void CreateGameScoreMaps()
    {
        CreateMap<GameScore, GameScoreViewModel>()
            .ForMember(
                dest => dest.GameName, 
                opt => opt
                    .MapFrom(src => src.Game!.Name))
            .ForMember(
                dest => dest.PlayerName,
                opt => opt
                    .MapFrom(src => src.Player!.Name));
        
        CreateMap<GameScore, GameScoreWithRankViewModel>()
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

    private void CreateUserMaps()
    {
        CreateMap<CreateUserCommand.CreateUserInputModel, User>();
    }
}