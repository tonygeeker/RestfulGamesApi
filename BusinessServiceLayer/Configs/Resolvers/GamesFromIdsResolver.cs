using AutoMapper;
using RestfulGamesApi.BusinessServiceLayer.Dtos.GameCollections;
using RestfulGamesApi.BusinessServiceLayer.Interfaces;
using RestfulGamesApi.DataAccessLayer.Models;

namespace RestfulGamesApi.BusinessServiceLayer.Configs.Resolvers
{
    public class GamesFromIdsResolver : IValueResolver<object, GameCollection, List<Game>>
    {
        IService<Game> gameService;
        IService<GameCollection> gameCollectionService;

        public GamesFromIdsResolver(IService<Game> gameService, IService<GameCollection> gameCollectionService)
        {
            this.gameCollectionService = gameCollectionService;
            this.gameService = gameService;
        }
        public List<Game> Resolve(object source, GameCollection destination, List<Game> destMember, ResolutionContext context)
        {
            if (source.GetType() == typeof(CreateGameCollectionRequestDto))
            {
                var dto = (CreateGameCollectionRequestDto)source;
                var games = dto.GameIds?.Select(y => gameService.GetQueryable(x => x.Id == y).SingleOrDefault()).ToList();
                return games;
            }
            else
            {
                var dto = (UpdateGameCollectionRequestDto)source;
                var games = dto.GameIds?.Select(y => gameService.GetQueryable(x => x.Id == y).SingleOrDefault()).ToList();
                return games;
            }

            return default;
        }
    }
}
