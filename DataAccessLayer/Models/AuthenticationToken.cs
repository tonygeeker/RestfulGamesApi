namespace RestfulGamesApi.DataAccessLayer.Models
{
    public class AuthenticationToken: BaseModel
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string RefreshToken { get; set; }
        public bool IsExpired { get; set; }
    }
}
