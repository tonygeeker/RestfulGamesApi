using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestfulGamesApi.BusinessServiceLayer.Dtos.Authentication;
using RestfulGamesApi.BusinessServiceLayer.Dtos.User;
using RestfulGamesApi.BusinessServiceLayer.Interfaces;
using RestfulGamesApi.DataAccessLayer.Enumerations;
using RestfulGamesApi.DataAccessLayer.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RestfulGamesApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AuthenticationController: Controller
    {
        IService<AuthenticationToken> tokenService;
        IService<User> userService;
        IAuthenticationService authenticationService;
        IConfiguration configuration;
        IMapper mapper;
        public AuthenticationController(IService<AuthenticationToken> tokenService, 
                                        IAuthenticationService authenticationService, 
                                        IService<User> userService, 
                                        IMapper mapper,
                                        IConfiguration configuration)
        {
            this.tokenService = tokenService;
            this.authenticationService = authenticationService;
            this.userService = userService;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequestDto authenticationRequestDto)
        {
            try
            {
                if (string.IsNullOrEmpty(authenticationRequestDto.Password) || (string.IsNullOrEmpty(authenticationRequestDto.Username)))
                    return BadRequest("Please provide all required fields");

                var authenticatedUser = authenticationService.Authenticate(authenticationRequestDto.Username, authenticationRequestDto.Password);

                if (authenticatedUser == null)
                    return Unauthorized("User not found");

                //get Authentication Token
                var profileToken = authenticationService.GetToken(new AuthenticationRequestDto
                {
                    Email = authenticationRequestDto.Email,
                    Role = authenticatedUser.Role,
                    TokenDurationMinutes = authenticationRequestDto.TokenDurationMinutes
                }, authenticatedUser.Id);

                var user = userService.GetQueryable(x => x.Id == authenticatedUser.Id).SingleOrDefault();

                var authenticatedResponse = new AuthenticationResponseDto<UserResponseDto>
                {
                    Profile = mapper.Map<UserResponseDto>(user),
                    AuthenticationToken = profileToken
                };

                return Ok(authenticatedResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdatePassword")]
        public IActionResult UpdatePassword([FromBody] UpdatePasswordRequestDto updatePasswordRequest)
        {
            try
            {
                var player = authenticationService.UpdatePassword(updatePasswordRequest.ProfileId, updatePasswordRequest.Email, updatePasswordRequest.Password);
                return Ok(mapper.Map<UserResponseDto>(player));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ResetPassword/{emailAddress}")]
        public async Task<IActionResult> ResetPassword(string emailAddress)
        {
            try
            {
                User authenticatedProfile = null;

                //Authenticate using Username and Password for Player
                if (string.IsNullOrEmpty(emailAddress))
                    return BadRequest("Please provide email address");

                authenticatedProfile = authenticationService.GetUserByEmail(emailAddress);

                if (authenticatedProfile == null)
                    return NotFound();

                string hashCode = "";
                        hashCode = (authenticationService.ResetPassword(emailAddress)).Substring(0, int.Parse(configuration["Security:LenghtResetPasswordCode"]));
                // email sending mechanism to send the hash with the email

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
