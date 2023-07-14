using AutoMapper;
using RestfulGamesApi.BusinessServiceLayer.Dtos.GameCollections;
using RestfulGamesApi.BusinessServiceLayer.Dtos.Games;
using RestfulGamesApi.BusinessServiceLayer.Interfaces;
using RestfulGamesApi.DataAccessLayer.Models;

namespace RestfulGamesApi.BusinessServiceLayer.Configs.Resolvers
{
    public class GameResponseDtoResolver : IValueResolver<GameCollection, GameCollectionResponseDto, List<GameResponseDto>>
    {
        IMapper mapper;
        IService<Game> gameService;

        public GameResponseDtoResolver(IMapper mapper, IService<Game> gameService)
        {
            this.mapper = mapper;
            this.gameService = gameService;
        }
        public List<GameResponseDto> Resolve(GameCollection source, GameCollectionResponseDto destination, List<GameResponseDto> destMember, ResolutionContext context)
        {
            var games = source.Games;
            return games.Select(x=>mapper.Map<GameResponseDto>(x)).ToList();
        }
    }
}
