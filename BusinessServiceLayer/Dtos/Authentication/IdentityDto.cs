using RestfulGamesApi.DataAccessLayer.Enumerations;

namespace RestfulGamesApi.BusinessServiceLayer.Dtos.Authentication
{
    public class IdentityDto
    {
        public int ProfileId { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}
