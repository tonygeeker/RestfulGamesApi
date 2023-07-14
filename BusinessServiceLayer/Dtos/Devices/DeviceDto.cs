using RestfulGamesApi.DataAccessLayer.Enumerations;

namespace RestfulGamesApi.BusinessServiceLayer.Dtos.Devices
{
    public class DeviceDto
    {
        public DeviceTypes Type { get; set; }
        public string Name { get; set; }
    }
}
