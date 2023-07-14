using RestfulGamesApi.DataAccessLayer.Enumerations;

namespace RestfulGamesApi.BusinessServiceLayer.Dtos.User
{
    public class UpdateUserRequestDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Username { get; set; }
        public string LastName { get; set; }
        public Role Role { get; set; }
        public string Password { get; set; }
    }
}
