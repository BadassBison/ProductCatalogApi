using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AutoMapper;

using Product_Catalog_Api.Dtos;
using Product_Catalog_Api.Services;

namespace Product_Catalog_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService service, IMapper mapper, ILogger<ProductsController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            try
            {
                var products = await _service.GetAllProductsAsync();
                if (!products.Any()) return NotFound();

                return _mapper.Map<Product[]>(products);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            try {
                var product = await _service.GetProductByIdAsync(id);
                return _mapper.Map<Product>(product);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        // TODO: Add search
        // [HttpGet("search")]
        // public async Task<ActionResult<IEnumerable<Product>>> SearchProductByName(string name)
        // {
        //     try
        //     {
        //         var product = await _service.GetProductByNameAsync(name);
        //         if (!products.Any()) return NotFound();

        //         return _mapper.Map<Product>(products);
        //     }
        //     catch (Exception)
        //     {
        //         return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
        //     }
        // }
    }
}
