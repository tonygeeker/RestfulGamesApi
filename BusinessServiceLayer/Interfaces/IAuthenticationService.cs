using RestfulGamesApi.BusinessServiceLayer.Dtos.Authentication;
using RestfulGamesApi.DataAccessLayer.Models;

namespace RestfulGamesApi.BusinessServiceLayer.Interfaces
{
    public interface IAuthenticationService
    {
        User Authenticate(string username, string password);
        AuthenticationTokenDto GetToken(AuthenticationRequestDto authenticationDto, int profileId);
        AuthenticationTokenDto RefreshToken(AuthenticationRequestDto authenticationDto);
        string HashPassword(string username, string password);
        string ResetPassword(string username);
        User GetUserByEmail(string email);
        User UpdatePassword(string username, string password);
        User UpdatePassword(int profileId, string username, string password);

    }
}
