using RestfulGamesApi.DataAccessLayer.Enumerations;

namespace RestfulGamesApi.BusinessServiceLayer.Dtos.Authentication
{
    public class AuthenticationRequestDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int TokenDurationMinutes { get; set; }
        public string RefreshToken { get; set; }
        public Role Role { get; set; }
    }
}
