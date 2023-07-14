namespace RestfulGamesApi.DataAccessLayer.Models
{
    public class Image: BaseModel
    {
        public int GameId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string AlternateText { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
