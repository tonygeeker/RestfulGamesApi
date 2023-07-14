using AutoMapper;
using RestfulGamesApi.BusinessServiceLayer.Dtos.Games;
using RestfulGamesApi.BusinessServiceLayer.Interfaces;
using RestfulGamesApi.DataAccessLayer.Models;

namespace RestfulGamesApi.BusinessServiceLayer.Configs.Resolvers
{
    public class AvailableDevicesResolver : IValueResolver<object, object, List<Device>>
    {
        IService<Game> gameService;
        IService<Device> deviceService;

        public AvailableDevicesResolver(IService<Game> gameService, IService<Device> deviceService)
        {
            this.gameService = gameService;
            this.deviceService = deviceService;
        }
        public List<Device> Resolve(object source, object destination, List<Device> destMember, ResolutionContext context)
        {
            if (source.GetType() == typeof(CreateGameRequestDto))
            {
                var request = (CreateGameRequestDto)source;
                var availableDevices = request.AvailableDevices.Select(x => new Device
                {
                    Name = x.ToString(),
                    Type = x,

                });

                return availableDevices.ToList();
            }
            else if (source.GetType() == typeof(Game))
            {
                var request = (Game)source;
                return request.AvailableDevices;
            }

            return default;
        }
    }
}
