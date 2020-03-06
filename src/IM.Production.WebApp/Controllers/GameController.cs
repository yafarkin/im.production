using Microsoft.AspNetCore.Mvc;
using Epam.ImitationGames.Production.Domain.Services;
using Microsoft.Extensions.Configuration;
using IM.Production.WebApp.Dtos;
using Microsoft.AspNetCore.Authorization;
using Epam.ImitationGames.Production.Domain.Authorization;

namespace IM.Production.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.Admin)]
    public class GameController : ControllerBase
    {
        private readonly IGameService _service;
        private readonly IConfiguration _config;

        public GameController(IGameService service, IConfiguration config)
        {
            _service = service;
            _config = config;
        }

        [HttpGet]
        [Route("get-game-config")]
        public GameConfigDto GetGameConfig()
        {
            var result = new GameConfigDto();
            _config.GetSection("Game").Bind(result);
            return result;
        }

        [HttpGet]
        [Route("get-current-day")]
        public int GetCurrentDay()
        {
            return _service.GetCurrentDay();
        }

        [HttpPut]
        [Route("calculate")]
        public void CalculateGame()
        {
            _service.CalculateDay();
        }

        [HttpPut]
        [Route("restart")]
        public void RestartGame()
        {
            _service.RestartGame();
        }

    }
}