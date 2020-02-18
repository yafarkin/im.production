using Microsoft.AspNetCore.Mvc;
using Epam.ImitationGames.Production.Domain.Services;
using Microsoft.Extensions.Configuration;
using IM.Production.WebApp.Dtos;

namespace IM.Production.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            _config.GetSection("GameConfig").Bind(result);
            _service.SetGameMaxDays(result.MaxDays);
            return result;
        }

        [HttpGet]
        [Route("calculate")]
        public int CalculateGame()
        {
            return _service.CalculateDay();
        }

        [HttpGet]
        [Route("restart")]
        public void RestartGame()
        {
            _service.RestartGame();
        }

    }
}