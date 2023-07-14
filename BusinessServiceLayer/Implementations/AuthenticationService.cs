using AutoMapper;
using RestfulGamesApi.BusinessServiceLayer.Dtos.Authentication;
using RestfulGamesApi.BusinessServiceLayer.Interfaces;
using RestfulGamesApi.DataAccessLayer.Models;
using System.Formats.Tar;
using System.Security.Claims;

namespace RestfulGamesApi.BusinessServiceLayer.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        IService<User> userService;
        IService<AuthenticationToken> tokenService;
        IConfiguration configuration;
        IJwtTokenService jwtTokenService;
        private string keyOfHash;
        IMapper mapper;
        public AuthenticationService(IService<User> userService, IConfiguration configuration, IJwtTokenService jwtTokenService, IService<AuthenticationToken> tokenService, IMapper mapper)
        {
            this.userService = userService;
            this.configuration = configuration;
            keyOfHash = configuration["Security:JWTSecurityKey:KeyOfHash"];
            this.jwtTokenService = jwtTokenService;
            this.tokenService = tokenService;
            this.mapper = mapper;

        }
        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(username))
                return null;

            //Create the Hash of the password
            string passwordHash = SecurityUtility.GetHashString(password + keyOfHash + username);

            var existingPerson = userService.GetQueryable(x =>
                                                             x.Username.ToLower() == username.ToLower() &&
                                                             x.PasswordHash == passwordHash).SingleOrDefault();

            return existingPerson;
        }

        public AuthenticationTokenDto GetToken(AuthenticationRequestDto authenticationDto, int profileId)
        {
            var authenticationToken = new AuthenticationToken
            {
                Token = jwtTokenService.GetJWTToken(authenticationDto.Email,
                                    authenticationDto.TokenDurationMinutes, authenticationDto.Role, profileId),
                RefreshToken = Guid.NewGuid().ToString(),
                ExpiryDate = DateTime.Now.AddMinutes(authenticationDto.TokenDurationMinutes > 0 ? authenticationDto.TokenDurationMinutes : 30),
            };

            //Save token to database
            tokenService.Add(authenticationToken);
            tokenService.SaveChanges();

            var authenticationTokenDto = mapper.Map<AuthenticationTokenDto>(authenticationToken, opt=>opt.AfterMap((src, dest) =>
            {
                dest.Role = userService.GetQueryable(x=>x.Id==profileId).SingleOrDefault().Role;
            }));

            authenticationTokenDto.Role = authenticationDto.Role;

            return authenticationTokenDto;
        }

        public AuthenticationTokenDto RefreshToken(AuthenticationRequestDto authenticationDto)
        {
            //Validate refreshToken
            var refreshToken = tokenService.GetQueryable(x =>
                                                                       x.RefreshToken == authenticationDto.RefreshToken &&
                                                                       !x.IsExpired).SingleOrDefault();

            //If token is not found => exit
            if (refreshToken == null)
            {
                return null;
            }

            // get token Identity from old token
            var tokenIdentity = jwtTokenService.GetJWTIdentity(refreshToken.Token);

            //Generate new Token
            var authenticationToken = new AuthenticationToken
            {
                Token = jwtTokenService.GetJWTToken(tokenIdentity.Email, authenticationDto.TokenDurationMinutes, tokenIdentity.Role, tokenIdentity.ProfileId),
                RefreshToken = Guid.NewGuid().ToString(),
                ExpiryDate = DateTime.Now.AddMinutes(authenticationDto.TokenDurationMinutes > 0 ? authenticationDto.TokenDurationMinutes : 30)
            };

            // Invalidate refreshToken
            refreshToken.IsExpired = true;
            tokenService.Update(refreshToken);

            //Save token to database
            tokenService.Add(authenticationToken);
            tokenService.SaveChanges();

            var authenticationTokenDto = mapper.Map<AuthenticationTokenDto>(authenticationToken);
            authenticationTokenDto.Role = authenticationDto.Role;

            return authenticationTokenDto;
        }

        public User UpdatePassword(int profileId, string username, string password)
        {
            var profile = userService.GetQueryable(x => x.Id == profileId).SingleOrDefault();

            if (profile == null)
                return null;

            //Create the Hash of the password            
            string passwordHash = this.HashPassword(username.ToLower(), password);

            //Update Password
            profile.PasswordHash = passwordHash;
            profile.ResetPasswordHash = ""; // Delete unused hash
            profile.PasswordResetExpiryDate = DateTime.Now; // Expired password
            userService.Update(profile);
            userService.SaveChanges();

            //Get Full Profile
            var person = userService.GetQueryable(x => x.Id == profile.Id).SingleOrDefault();

            return person;
        }

        public string ResetPassword(string username)
        {
            var profile = userService.GetQueryable(x => x.Email.ToLower() == username.ToLower()).SingleOrDefault();

            //If user is not found  => Return OK (should not inform user that email was not found)
            if (profile == null)
                return string.Empty;

            //Create Hash
            string hash = SecurityUtility.GetHashString(profile.PasswordHash + DateTime.Now.ToShortTimeString() + profile.Email);
            profile.ResetPasswordHash = hash;
            profile.PasswordResetExpiryDate = DateTime.Now.AddDays(1);

            userService.SaveChanges();

            return hash;
        }

        public User UpdatePassword(string username, string password)
        {
            var profile = userService.GetQueryable(x => x.Email.ToLower() == username.ToLower()).SingleOrDefault();

            if (profile == null)
                return null;

            //Create the Hash of the password            
            string passwordHash = this.HashPassword(username.ToLower(), password);

            //Update Password
            profile.PasswordHash = passwordHash;
            profile.ResetPasswordHash = ""; // Delete unused hash
            profile.PasswordResetExpiryDate = DateTime.Now; // Expired password
            userService.Update(profile);
            userService.SaveChanges();

            //Get Full Profile
            var person = userService.GetQueryable(x => x.Id == profile.Id).SingleOrDefault();

            return person;
        }

        public string HashPassword(string username, string password)
        {
            return SecurityUtility.GetHashString(password + keyOfHash + username);
        }

        public User GetUserByEmail(string email)
        {
            return userService.GetQueryable(x => x.Email.ToLower() == email.ToLower()).SingleOrDefault();
        }
    }
}
