using AutoMapper;
using BallastLaneApplication.Data.Service.Interfaces;
using BallastLaneApplication.Domain.DTOs;
using BallastLaneApplication.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BallastLaneApplication.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _service;

        public ProductController(
            IMapper mapper,
            ILogger<ProductController> logger,
            IProductService service
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
            _service = service ?? throw new ArgumentNullException(nameof(_service));
            _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult> GetAllAsync()
        {
            var email = User.Claims.First().Value;

            var products = await _service.GetAllAsync(email);
            var dtos = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(dtos);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ProductDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetAsync(string id)
        {
            var email = User.Claims.First().Value;

            var product = await _service.GetAsync(id, email);
            if (product == null)
            {
                _logger.LogError($"Product not found");
                return NotFound();
            }

            var dto = _mapper.Map<ProductDTO>(product);

            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> AddAsync(ProductDTO dto)
        {
            if (dto == null)
            {
                _logger.LogError($"ProductDTO is null");
                return BadRequest();
            }

            var email = User.Claims.First().Value;

            var product = _mapper.Map<Product>(dto);
            await _service.AddAsync(product, email);

            return Ok(dto);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> UpdateAsync(ProductDTO dto)
        {
            if (dto == null)
            {
                _logger.LogError($"ProductDTO is null");
                return BadRequest();
            }

            var email = User.Claims.First().Value;
            var updated = await _service.UpdateAsync(dto, email);
            if (updated)
            {
                return Ok();
            }
            else
            {
                _logger.LogError($"Product record was not updated");
                return BadRequest("Record was not updated");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var email = User.Claims.First().Value;

            var deleted = await _service.DeleteAsync(id, email);
            if (deleted)
            {
                return Ok();
            }
            else
            {
                _logger.LogError($"Product record was not deleted");
                return BadRequest("Record was not deleted");
            }
        }
    }
}
