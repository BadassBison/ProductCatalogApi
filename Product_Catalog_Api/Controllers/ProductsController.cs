using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AutoMapper;

using Product_Catalog_Api.Dtos;
using Product_Catalog_Api.Services;
using Product_Catalog_Api.Models;

namespace Product_Catalog_Api.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class ProductsController : ControllerBase
  {
    private readonly IProductService _service;
    private readonly IHttpContextAccessor _http;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService service, IHttpContextAccessor http, IMapper mapper, ILogger<ProductsController> logger)
    {
      _service = service;
      _http = http;
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
    public async Task<ActionResult<Product>> GetProductById([FromRoute] int id)
    {
      try {
        var product = await _service.GetProductByIdAsync(id);
        return _mapper.Map<Product>(product);
      }
      catch (InvalidOperationException)
      {
        return NotFound($"No product with id '{id}' is in the database");
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
      }
    }

    [HttpGet("search")]
    public ActionResult<List<Product>> SearchProductByName(string name)
    {
      try
      {
        var products = _service.GetProductsByName(name);
        if (!products.Any()) return NotFound($"No products were found that were similiar to '{name}'");

        return _mapper.Map<List<Product>>(products);
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
      }
    }

    [HttpPost]
    public async Task<ActionResult<Product>> AddProduct([FromBody] Product dto)
    {
      try
      {
        var product = _mapper.Map<ProductEntity>(dto);
        product.CreatedDate = DateTime.Now;
        product.LastUpdatedDate = product.CreatedDate;

        await _service.AddProductAsync(product);

        var uri = _http.HttpContext.Request.Host.Value;
        return Created(uri, _mapper.Map<Product>(product));
      }
      catch (InvalidOperationException)
      {
        return BadRequest("Product already exists");
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
      }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> UpdateProduct([FromBody] Product model, [FromRoute] int id)
    {
      try
      {
        var productEntity = await _service.GetProductByIdAsync(id);

        productEntity = await _service.UpdateProductAsync(productEntity, model);
        return _mapper.Map<Product>(productEntity);
      }
      catch (InvalidOperationException)
      {
        return BadRequest($"Product with id {id} or name {model.Name} does not exist");
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure ");
      }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Product>> DeleteProduct([FromRoute] int id)
    {
      try
      {
        var product = await _service.GetProductByIdAsync(id);

        await _service.RemoveProductAsync(product);
        return Ok($"Product '{product.Name}' was removed from the database");
      }
      catch (InvalidOperationException)
      {
        return BadRequest($"Product with id {id} is not in the database to be removed");
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
      }
    }
  }
}
