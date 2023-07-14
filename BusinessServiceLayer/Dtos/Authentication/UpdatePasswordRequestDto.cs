using RestfulGamesApi.DataAccessLayer.Enumerations;

namespace RestfulGamesApi.BusinessServiceLayer.Dtos.Authentication
{
    public class UpdatePasswordRequestDto
    {
        public int ProfileId { get; set; }
        public Role Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
