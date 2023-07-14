using Microsoft.IdentityModel.Tokens;
using RestfulGamesApi.BusinessServiceLayer.Dtos.Authentication;
using RestfulGamesApi.BusinessServiceLayer.Interfaces;
using RestfulGamesApi.DataAccessLayer.Enumerations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestfulGamesApi.BusinessServiceLayer.Implementations
{
    public class JwtTokenService : IJwtTokenService
    {
        IConfiguration configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IdentityDto GetJWTIdentity(string token)
        {
            //Get Claims
            var tokenHandler = new JwtSecurityTokenHandler();
            var oldToken = tokenHandler.ReadJwtToken(token);

            var email = oldToken.Claims.First(x => x.Type == "email")?.Value;
            var role = oldToken.Claims.First(x => x.Type == "role")?.Value;

            var profileId = 0;
            int.TryParse(oldToken.Claims.First(x => x.Type == ClaimTypes.PostalCode)?.Value, out profileId);

            return new IdentityDto
            {
                Email = email,
                Role = (Role)Enum.Parse(typeof(Role), role),
                ProfileId = profileId
            };
        }

        public string GetJWTToken(string emailAddress, int tokenDurationMinutes, Role role, int profileId)
        {
            var claims = new[]
           {
                new Claim(ClaimTypes.Email,emailAddress),
                new Claim(ClaimTypes.Role, role.ToString()),
                new Claim(ClaimTypes.PostalCode, profileId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Security:JWTSecurityKey:SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
