using RestfulGamesApi.BusinessServiceLayer.Dtos.Authentication;
using RestfulGamesApi.DataAccessLayer.Enumerations;

namespace RestfulGamesApi.BusinessServiceLayer.Interfaces
{
    public interface IJwtTokenService
    {
        string GetJWTToken(string emailAddress, int tokenDurationMinutes, Role role, int profileId);

        IdentityDto GetJWTIdentity(string token);
    }
}
