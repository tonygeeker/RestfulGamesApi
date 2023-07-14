using Microsoft.AspNetCore.Mvc;
using RestfulGamesApi.BusinessServiceLayer.Interfaces;
using RestfulGamesApi.DataAccessLayer.Models;
using RestfulGamesApi.DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using RestfulGamesApi.BusinessServiceLayer.Dtos.GameCollections;
using AutoMapper;
using RestfulGamesApi.BusinessServiceLayer.Dtos.Games;
using RestfulGamesApi.BusinessServiceLayer.Dtos.Devices;
using RestfulGamesApi.BusinessServiceLayer.Dtos.Images;

namespace RestfulGamesApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class GameCollectionsController: Controller
    {
        IService<GameCollection> gameCollectionService;
        IService<Game> gameService;
        IMapper mapper;
        public GameCollectionsController(IService<GameCollection> gameCollectionService, IService<Game> gameService, IMapper mapper)
        {
            this.gameCollectionService = gameCollectionService;
            this.gameService = gameService;
            this.mapper = mapper;
        }

        // GET api/GameCollections
        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 20)
        {
            var gameCollections = gameCollectionService.GetQueryable(x => x.IsEnabled == true);

            var list = new LazyPagination<GameCollection>(gameCollections, page, pageSize);

            if (gameCollections.Count() < 1)
                return NotFound("No game collections were found!");

            // pagination headers
            this.Response.Headers.Add("ItemCount", list.TotalItems.ToString());
            this.Response.Headers.Add("PageCount", list.TotalPages.ToString());

            return Ok(list.Select(x=>mapper.Map<GameCollectionResponseDto>(x, opt=>opt.AfterMap((src, dest) =>
            {
                dest.Games = x.Games?.Select(y=>mapper.Map<GameResponseDto>(y)).ToList();
            }))));
        }

        // GET api/GameCollections/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var gameCollection = gameCollectionService.GetQueryable(x => x.Id == id).SingleOrDefault();
            if (gameCollection == null)
                return NotFound("Specified game collection id not found!");

            return Ok(mapper.Map<GameCollectionResponseDto>(gameCollection));
        }

        // POST api/GameCollections
        [HttpPost, Authorize(Roles = "Admin")]
        public IActionResult Post([FromBody]CreateGameCollectionRequestDto createGameRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Please provide all required game collection information");

            var gameCollection = mapper.Map<GameCollection>(createGameRequestDto);

            gameCollectionService.Add(gameCollection);
            gameCollectionService.SaveChanges();

            return Ok(mapper.Map<GameCollectionResponseDto>(gameCollection));
        }

        // PUT api/GameCollections/{id}
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public IActionResult Put([FromBody]UpdateGameCollectionRequestDto updateGameCollectionRequestDto, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Please provide all required game information");

            var existingGameCollection = gameCollectionService.GetQueryable(x => x.Id == id).SingleOrDefault();
            if (existingGameCollection == null)
                return NotFound("Specified game id could not be found");

            existingGameCollection = mapper.Map<GameCollection>(updateGameCollectionRequestDto);

            gameCollectionService.Update(existingGameCollection);
            gameCollectionService.SaveChanges();

            return Ok(mapper.Map<GameCollectionResponseDto>(existingGameCollection));
        }

        // DELETE api/GameCollections/{id}
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var gameCollection = gameCollectionService.GetQueryable(x => x.Id == id).SingleOrDefault();
            if (gameCollection == null)
                return NotFound("Specified game id not found");

            gameCollectionService.Delete(gameCollection);

            return Ok();
        }

        [HttpPatch("{id}"), Authorize(Roles = "Admin")]
        public IActionResult UpdateGameCollectionGames(UpdateGameCollectionGamesDto updateDto, int id)
        {
            if (!ModelState.IsValid || id < 1)
                return BadRequest("Please provide all required fields");

            var gameCollection = gameCollectionService.GetQueryable(x => x.Id == id).SingleOrDefault();
            if (gameCollection == null)
                return BadRequest("Specified game doesn't exist");

            gameCollection.Games = updateDto.GameIds?.Select(y => gameService.GetQueryable(x => x.Id == y).SingleOrDefault()).ToList();

            gameCollectionService.Update(gameCollection);
            gameService.SaveChanges();

            return Ok(mapper.Map<GameCollectionResponseDto>(gameCollection));
        }
    }
}
