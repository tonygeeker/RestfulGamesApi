using RestfulGamesApi.DataAccessLayer.Enumerations;
using System.Drawing;

namespace RestfulGamesApi.DataAccessLayer.Models
{
    public class Game: BaseModel
    {
        public int GameCollectionId { get; set; }
        public string DisplayName { get; set; }
        public string DisplayIndex { get; set; }
        public DateTime ReleaseDate { get; set; }
        public GameCategory GameCategory { get; set; }
        public virtual Image Image { get; set; }
        public List<Device> AvailableDevices { get; set; }
        public List<GameCollection> MemberCollections { get; set; }
    }
}
