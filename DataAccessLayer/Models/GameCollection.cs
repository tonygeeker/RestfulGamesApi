namespace RestfulGamesApi.DataAccessLayer.Models
{
    public class GameCollection: BaseModel
    {
        public string  DisplayName { get; set; }
        public string  DisplayIndex { get; set; }
        public virtual List<Game> Games { get; set; }
        public virtual List<SubCollection> SubCollections { get; set; }
    }
}
