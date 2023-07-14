using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestfulGamesApi.BusinessServiceLayer;
using RestfulGamesApi.BusinessServiceLayer.Dtos.User;
using RestfulGamesApi.BusinessServiceLayer.Interfaces;
using RestfulGamesApi.DataAccessLayer;
using RestfulGamesApi.DataAccessLayer.Models;

namespace RestfulGamesApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsersController: Controller
    {
        IService<User> userService;
        IConfiguration configuration;
        IMapper mapper;
        public UsersController(IService<User> userService, IConfiguration configuration, IMapper mapper)
        {
            this.userService = userService;
            this.configuration = configuration;
            this.mapper = mapper;
        }

        // Get api/Users/{id}
        [HttpGet("{id}"), Authorize]
        public IActionResult Get(int id)
        {
            if (id < 1)
                return BadRequest("Invalid id provided!");

            var user = userService.GetQueryable(x => x.Id == id).SingleOrDefault();
            if (user == null)
                return NotFound("Specified user id not found");

            return Ok(mapper.Map<UserResponseDto>(user));
        }

        // GET api/Users
        [HttpGet, Authorize]
        public IActionResult GetAll(int page = 1, int pageSize = 20)
        {
            var usersList = userService.GetQueryable(x => x.IsEnabled == true);

            var list = new LazyPagination<User>(usersList, page, pageSize);

            if (usersList == null)
                return NotFound("No users were found!");

            // pagination headers
            this.Response.Headers.Add("ItemCount", list.TotalItems.ToString());
            this.Response.Headers.Add("PageCount", list.TotalPages.ToString());

            return Ok(list.Select(x => mapper.Map<UserResponseDto>(x)));
        }

        // POST api/Users
        [HttpPost, AllowAnonymous]
        public IActionResult Post(CreateUserRequestDto createUserRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Please provide all required user information");

            if (!SecurityUtility.ValidateUsernameAvailability(createUserRequestDto.Email, createUserRequestDto.Username, userService))
                return BadRequest("Username already taken, please choose a unique username");

            var existingUser = userService.GetQueryable(x=>x.Email.ToLower() ==  createUserRequestDto.Email.ToLower()).SingleOrDefault();
            if (existingUser != null)
                return BadRequest("Email address already exists");

            var user = mapper.Map<User>(createUserRequestDto, opt => opt.AfterMap((src, dest) =>
            {
                dest.PasswordHash = SecurityUtility.GetHashString(createUserRequestDto.Password + configuration["Security:JWTSecurityKey:KeyOfHash"] + createUserRequestDto.Username);
            }));

            userService.Add(user);
            userService.SaveChanges();

            return Ok(mapper.Map<UserResponseDto>(user));
        }

        // PUT api/Users/{id}
        [HttpPut("{id}"), Authorize]
        public IActionResult Put(UpdateUserRequestDto updateUserRequestDto, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Please provide all required user information");

            var exisitingUser = userService.GetQueryable(x => x.Id == id).SingleOrDefault();
            if (exisitingUser == null)
                return NotFound("Specified user id could not be found");

            exisitingUser.FirstName = updateUserRequestDto.FirstName;
            exisitingUser.LastName = updateUserRequestDto.LastName;
            exisitingUser.Role = updateUserRequestDto.Role;
            exisitingUser.Email = updateUserRequestDto.Email;

            userService.Update(exisitingUser);
            userService.SaveChanges();

            return Ok(mapper.Map<UserResponseDto>(exisitingUser));
        }

        // DELETE api/Users/{id}
        [HttpDelete("{id}"), Authorize(Roles = "ContentManager")]
        public IActionResult Delete(int id)
        {
            var game = userService.GetQueryable(x => x.Id == id).SingleOrDefault();
            if (game == null)
                return NotFound("Specified user id not found");

            userService.Delete(game);
            return Ok();
        }
    }
}
