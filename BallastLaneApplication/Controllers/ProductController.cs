using AutoMapper;
using BallastLaneApplication.Data.Service.Interfaces;
using BallastLaneApplication.Domain.DTOs;
using BallastLaneApplication.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BallastLaneApplication.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductService _service;
        private readonly string userId;

        public ProductController(
            IMapper mapper,
            IProductService service
            )
        {
            _mapper = mapper;
            _service = service;

            userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _service.GetProductsAsync(userId);
            var dtos = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create(ProductDTO dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            var product = _mapper.Map<Product>(dto);
            await _service.AddProduct(product);

            return Ok(dto);
        }

        [HttpPut]
        public async Task<ActionResult> Update(ProductDTO dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            return Ok(await _service.UpdateProductAsync(dto, userId));
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            return Ok(await _service.DeleteProductAsync(id, userId));
        }
    }
}
