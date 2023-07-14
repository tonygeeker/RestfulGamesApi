using AutoMapper;
using RestfulGamesApi.BusinessServiceLayer.Dtos.Devices;
using RestfulGamesApi.BusinessServiceLayer.Dtos.Games;
using RestfulGamesApi.BusinessServiceLayer.Interfaces;
using RestfulGamesApi.DataAccessLayer.Models;

namespace RestfulGamesApi.BusinessServiceLayer.Configs.Resolvers
{
    public class DevicesDtoResolver : IValueResolver<Game, GameResponseDto, List<DeviceDto>>
    {
        IService<Game> gameService;
        IService<Device> deviceService;

        public DevicesDtoResolver(IService<Game> gameService, IService<Device> deviceService)
        {
            this.gameService = gameService;
            this.deviceService = deviceService;
        }
        public List<DeviceDto> Resolve(Game source, GameResponseDto destination, List<DeviceDto> destMember, ResolutionContext context)
        {
            var devices = deviceService.GetQueryable(x=>x.Games.Contains(source))?.Select(d => new DeviceDto
            {
                Name = d.Name,
                Type = d.Type
            }).ToList();

            return devices;
        }
    }
}
