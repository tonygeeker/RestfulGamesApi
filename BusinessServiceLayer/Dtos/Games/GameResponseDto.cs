using RestfulGamesApi.BusinessServiceLayer.Dtos.Devices;
using RestfulGamesApi.BusinessServiceLayer.Dtos.Images;
using RestfulGamesApi.DataAccessLayer.Enumerations;
using RestfulGamesApi.DataAccessLayer.Models;

namespace RestfulGamesApi.BusinessServiceLayer.Dtos.Games
{
    public class GameResponseDto
    {
        public string DisplayName { get; set; }
        public string DisplayIndex { get; set; }
        public DateTime ReleaseDate { get; set; }
        public GameCategory GameCategory { get; set; }
        public ImageResponseDto Thumbnail { get; set; }
        public List<DeviceDto> AvailableDevices { get; set; }
    }
}
