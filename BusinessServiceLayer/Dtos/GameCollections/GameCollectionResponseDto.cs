using RestfulGamesApi.BusinessServiceLayer.Dtos.Games;
using RestfulGamesApi.DataAccessLayer.Models;

namespace RestfulGamesApi.BusinessServiceLayer.Dtos.GameCollections
{
    public class GameCollectionResponseDto
    {
        public string DisplayName { get; set; }
        public string DisplayIndex { get; set; }
        public virtual List<GameResponseDto> Games { get; set; }
        public virtual List<SubCollection> SubCollections { get; set; }
    }
}
