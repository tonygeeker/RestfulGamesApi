namespace RestfulGamesApi.BusinessServiceLayer.Dtos.GameCollections
{
    public class UpdateGameCollectionGamesDto
    {
        public int GameCollectionId { get; set; }
        public List<int> GameIds { get; set; }
    }
}
