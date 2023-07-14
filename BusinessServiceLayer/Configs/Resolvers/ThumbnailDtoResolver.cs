using AutoMapper;
using RestfulGamesApi.BusinessServiceLayer.Dtos.Games;
using RestfulGamesApi.BusinessServiceLayer.Dtos.Images;
using RestfulGamesApi.BusinessServiceLayer.Interfaces;
using RestfulGamesApi.DataAccessLayer.Models;

namespace RestfulGamesApi.BusinessServiceLayer.Configs.Resolvers
{
    public class ThumbnailDtoResolver : IValueResolver<Game, GameResponseDto, ImageResponseDto>
    {
        IService<Game> gameService;
        IService<Image> imageService;
        IMapper mapper;

        public ThumbnailDtoResolver(IService<Game> gameService, IService<Image> imageService, IMapper mapper)
        {
            this.gameService = gameService;
            this.imageService = imageService;
            this.mapper = mapper;
        }
        public ImageResponseDto Resolve(Game source, GameResponseDto destination, ImageResponseDto destMember, ResolutionContext context)
        {
            var image = imageService.GetQueryable(x => x.GameId == source.Id).SingleOrDefault();
            if (image == null)
                return null;

            var imageResponseDto = mapper.Map<ImageResponseDto>(image);
            return imageResponseDto;
        }
    }
}
