using RestfulGamesApi.DataAccessLayer.Models;

namespace RestfulGamesApi.BusinessServiceLayer.Dtos.GameCollections
{
    public class CreateGameCollectionRequestDto
    {
        public string DisplayName { get; set; }
        public string DisplayIndex { get; set; }
        public List<int> GameIds { get; set; }
    }
}
