using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestfulGamesApi.BusinessServiceLayer;
using RestfulGamesApi.BusinessServiceLayer.Dtos.Devices;
using RestfulGamesApi.BusinessServiceLayer.Dtos.Games;
using RestfulGamesApi.BusinessServiceLayer.Dtos.Images;
using RestfulGamesApi.BusinessServiceLayer.Dtos.User;
using RestfulGamesApi.BusinessServiceLayer.Interfaces;
using RestfulGamesApi.DataAccessLayer;
using RestfulGamesApi.DataAccessLayer.Models;
using static System.Net.Mime.MediaTypeNames;

namespace RestfulGamesApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class GamesController: Controller
    {
        IService<Game> gameService;
        IService<DataAccessLayer.Models.Image> imageService;
        IService<Device> devicesService;
        IMapper mapper;
        public GamesController(IService<Game> gameService, IService<DataAccessLayer.Models.Image> imageService, IMapper mapper, IService<Device> devicesService)
        {
            this.gameService = gameService;
            this.imageService = imageService;
            this.mapper = mapper;
            this.devicesService = devicesService;
        }

        // GET api/Games/
        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 20)
        {
            var gamesList = gameService.GetQueryable(x => x.IsEnabled == true);

            var list = new LazyPagination<Game>(gamesList, page, pageSize);

            if (gamesList.Count() < 1) 
                return NotFound("No games were found");

            // pagination headers
            this.Response.Headers.Add("ItemCount", list.TotalItems.ToString());
            this.Response.Headers.Add("PageCount", list.TotalPages.ToString());

            return Ok(list.Select(y=> mapper.Map<GameResponseDto>(y)));
        }

        // GET api/Games/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var game = gameService.GetQueryable(x => x.Id == id).SingleOrDefault();
            if (game== null)
                return NotFound("Specified game id not found");

            return Ok(mapper.Map<GameResponseDto>(game));
        }

        // POST api/Games/
        [HttpPost, Authorize]
        public IActionResult Post([FromBody]CreateGameRequestDto createGameRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Please provide all required game information");

            var game = mapper.Map<Game>(createGameRequestDto);

            gameService.Add(game);
            gameService.SaveChanges();
            return Ok(mapper.Map<GameResponseDto>(game));
        }

        // PUT api/Games/{id}
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public IActionResult Put(UpdateGameRequestDto updateGameRequestDto, int id)
        {
            if (!ModelState.IsValid || id < 1)
                return BadRequest("Please provide all required game information");

            var existingGame = gameService.GetQueryable(x => x.Id == id).SingleOrDefault();
            if (existingGame == null)
                return NotFound("Specified game id could not be found");

            existingGame = mapper.Map<Game>(updateGameRequestDto);

            gameService.Update(existingGame);
            gameService.SaveChanges();
            return Ok(mapper.Map<GameResponseDto>(existingGame));

        }

        // DELETE api/Games/{id}
        [HttpDelete("{id}"), Authorize(Roles = "ContentManager")]
        public IActionResult Delete(int id)
        {
            var game = gameService.GetQueryable(x => x.Id == id).SingleOrDefault();
            if (game == null)
                return NotFound("Specified game id not found");

            gameService.Delete(game);

            return Ok();
        }

        [HttpPatch("{id}"), Authorize(Roles = "Admin")]
        public IActionResult UpdateGameThumbnail([FromBody]UpdateGameImageDto updateGameImageDto, int id)
        {
            if (!ModelState.IsValid || id < 1)
                return BadRequest("Please provide all required fields");

            var game = gameService.GetQueryable(x => x.Id == id).SingleOrDefault();
            if (game == null)
                return BadRequest("Specified game doesn't exist");

            var image = mapper.Map<DataAccessLayer.Models.Image>(updateGameImageDto);

            imageService.Add(image);
            imageService.SaveChanges();

            return Ok(mapper.Map<GameResponseDto>(game));
        }
    }
}
