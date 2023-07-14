using RestfulGamesApi.DataAccessLayer.Enumerations;

namespace RestfulGamesApi.DataAccessLayer.Models
{
    public class Device: BaseModel
    {
        public DeviceTypes Type { get; set; }
        public string Name { get; set; }
        public List<Game> Games { get; set; }
    }
}
