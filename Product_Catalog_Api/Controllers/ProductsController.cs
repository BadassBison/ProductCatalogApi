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
  /// <summary>
  /// API REST Endpoints for Products
  /// </summary>
  [ApiController]
  [Route("[controller]")]
  public class ProductsController : ControllerBase
  {
    private readonly IProductService _service;
    private readonly IHttpContextAccessor _http;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductsController> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="service"></param>
    /// <param name="http"></param>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    public ProductsController(IProductService service, IHttpContextAccessor http, IMapper mapper, ILogger<ProductsController> logger)
    {
      _service = service;
      _http = http;
      _mapper = mapper;
      _logger = logger;
    }

    /// <summary>
    /// Fetches all products
    /// </summary>
    /// <returns>A collection of products</returns>
    /// <response code="200">OK if it was a successful fetch</response>
    /// <response code="404">Could not find any products in the database</response>
    /// <response code="500">Database failure</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<Product>>> GetAllProducts()
    {
      try
      {
        _logger.LogInformation("Fetching All Products");

        var products = await _service.GetAllProductsAsync();
        if (!products.Any()) return NotFound("No products were found in the database");

        return _mapper.Map<List<Product>>(products);
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
      }
    }

    /// <summary>
    /// Fetches a product with a specific ID
    /// </summary>
    /// <returns>A product with a specific ID</returns>
    /// <response code="200">OK if it was a successful fetch</response>
    /// <response code="404">Could not find product with given id</response>
    /// <response code="500">Database failure</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Product>> GetProductById([FromRoute] int id)
    {
      try
      {
        _logger.LogInformation($"Fetching Product with id {id}");

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

    /// <summary>
    /// Fetches products with a name similiar to the search term
    /// </summary>
    /// <returns>Products with a name similiar to the search term</returns>
    /// <response code="200">OK if it was a successful fetch</response>
    /// <response code="404">Could not find product with given id</response>
    /// <response code="500">Database failure</response>
    [HttpGet("search")]
    [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public ActionResult<List<Product>> SearchProductByName(string name)
    {
      try
      {
        _logger.LogInformation($"Fetching Products with name similiar to {name}");

        var products = _service.GetProductsByName(name);
        if (!products.Any()) return NotFound($"No products were found that were similiar to '{name}'");

        return _mapper.Map<List<Product>>(products);
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
      }
    }

    /// <summary>
    /// Adds a product
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /products
    ///     {
    ///        "name": "productName",
    ///        "price": 10,
    ///        "cost": 8
    ///        "quantity": 5
    ///        "description": "A new product"
    ///        "manufacturerId": 20
    ///     }
    ///
    /// </remarks>
    /// <returns>The product that was added</returns>
    /// <response code="201">OK if it was a successful post</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">Database failure</response>
    [HttpPost]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Product>> AddProduct([FromBody] Product dto)
    {
      try
      {
        _logger.LogInformation($"Adding Product {dto.Name}");

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

    /// <summary>
    /// Updates a product.
    /// You can pass any properties and the whole record will be updated
    /// This will also add a new record for the price log
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /products/:id
    ///     {
    ///        "name": "productName",
    ///        "price": 10,
    ///        "cost": 8
    ///        "quantity": 5
    ///        "description": "A new product"
    ///        "manufacturerId": 20
    ///     }
    ///
    /// </remarks>
    /// <returns>The product that was updated</returns>
    /// <response code="200">OK if it was a successful put</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">Database failure</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Product>> UpdateProduct([FromBody] Product dto, [FromRoute] int id)
    {
      try
      {
        _logger.LogInformation($"Updating Product {dto.Name}");

        var productEntity = await _service.GetProductByIdAsync(id);

        productEntity = await _service.UpdateProductAsync(productEntity, dto);
        return _mapper.Map<Product>(productEntity);
      }
      catch (InvalidOperationException)
      {
        return BadRequest($"Product with id {id} or name {dto.Name} does not exist");
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure ");
      }
    }

    /// <summary>
    /// Removes a product from the database
    /// </summary>
    /// <returns></returns>
    /// <response code="200">OK if it was a successful removal</response>
    /// <response code="400">Bad request</response>
    /// <response code="500">Database failure</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Product>> DeleteProduct([FromRoute] int id)
    {
      try
      {
        _logger.LogInformation($"Deleting Product with id {id}");

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
