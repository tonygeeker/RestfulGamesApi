namespace RestfulGamesApi.DataAccessLayer.Models
{
    public class SubCollection: BaseModel
    {
        public string DisplayName { get; set; }
        public List<Game> Games { get; set; }
    }
}
