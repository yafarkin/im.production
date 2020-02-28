using System;
using System.Collections.Generic;
using AutoMapper;
using Epam.ImitationGames.Production.Domain.Services;
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
        [Route("/{id}")]
        public IEnumerable<StockMaterialDto> GetMaterials(Guid id)
        {
            var materials = _service.GetMaterials(id);
            return _mapper?.Map<IEnumerable<StockMaterialDto>>(materials);
        }
    }
}
