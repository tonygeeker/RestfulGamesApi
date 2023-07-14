using AutoMapper;
using RestfulGamesApi.BusinessServiceLayer.Configs.Resolvers;
using RestfulGamesApi.BusinessServiceLayer.Dtos.Authentication;
using RestfulGamesApi.BusinessServiceLayer.Dtos.GameCollections;
using RestfulGamesApi.BusinessServiceLayer.Dtos.Games;
using RestfulGamesApi.BusinessServiceLayer.Dtos.Images;
using RestfulGamesApi.BusinessServiceLayer.Dtos.User;
using RestfulGamesApi.DataAccessLayer.Models;

namespace RestfulGamesApi.BusinessServiceLayer.Configs
{
    public class AutomapperConfig: Profile
    {
        public AutomapperConfig()
        {
            CreateMap<User, UserResponseDto>();
            CreateMap<UserResponseDto, User>();
            CreateMap<CreateUserRequestDto, User>();

            CreateMap<CreateGameRequestDto, Game>()
                .ForMember(dest => dest.AvailableDevices, opt => opt.MapFrom<AvailableDevicesResolver>());

            CreateMap<UpdateGameRequestDto, Game>()
                .ForMember(dest => dest.AvailableDevices, opt => opt.MapFrom<AvailableDevicesResolver>());


            CreateMap<CreateGameCollectionRequestDto, GameCollection>()
                .ForMember(dest => dest.Games, opt => opt.MapFrom<GamesFromIdsResolver>());

            CreateMap<UpdateGameCollectionGamesDto, GameCollection>()
                .ForMember(dest => dest.Games, opt => opt.MapFrom<GamesFromIdsResolver>());

            CreateMap<GameCollection, GameCollectionResponseDto>()
                .ForMember(dest => dest.Games, opt => opt.MapFrom<GameResponseDtoResolver>());


            CreateMap<Game, GameResponseDto>()
                .ForMember(dest => dest.AvailableDevices, opt => opt.MapFrom<DevicesDtoResolver>())
                .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom<ThumbnailDtoResolver>());

            CreateMap<AuthenticationTokenDto, AuthenticationToken>();
            CreateMap<AuthenticationToken, AuthenticationTokenDto>();

            CreateMap<UpdateGameImageDto, Image>();
            CreateMap<Image, ImageResponseDto>();

        }
    }
}
