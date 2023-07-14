using RestfulGamesApi.DataAccessLayer.Enumerations;
using RestfulGamesApi.DataAccessLayer.Models;

namespace RestfulGamesApi.BusinessServiceLayer.Dtos.Games
{
    public class UpdateGameRequestDto
    {
        public string DisplayName { get; set; }
        public string DisplayIndex { get; set; }
        public DateTime ReleaseDate { get; set; }
        public GameCategory GameCategory { get; set; }
        public List<DeviceTypes> AvailableDevices { get; set; }
    }
}
