using RestfulGamesApi.DataAccessLayer.Enumerations;

namespace RestfulGamesApi.BusinessServiceLayer.Dtos.Authentication
{
    public class AuthenticationTokenDto
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string RefreshToken { get; set; }
        public bool IsExpired { get; set; }
        public Role Role { get; set; }
    }
}
