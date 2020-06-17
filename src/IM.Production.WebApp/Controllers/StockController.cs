using System;
using System.Collections.Generic;
using AutoMapper;
using Epam.ImitationGames.Production.Domain.Services;
using IM.Production.Services;
using IM.Production.WebApp.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace IM.Production.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _service;
        private readonly IMapper _mapper;

        public StockController(IStockService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<StockMaterialDto> GetMaterials(string login, Guid factoryId)
        {
            var materials = _service.GetMaterials(login, factoryId);
            return _mapper?.Map<IEnumerable<StockMaterialDto>>(materials);
        }
    }
}
