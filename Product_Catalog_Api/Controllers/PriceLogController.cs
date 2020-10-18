using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Product_Catalog_Api.Dtos;
using Product_Catalog_Api.Services;
using Product_Catalog_Api.Models;

namespace Product_Catalog_Api.Controllers
{
  /// <summary>
  /// API REST Endpoints for Price Logs
  /// </summary>
  [ApiController]
  [Route("[controller]")]
  public class PriceLogController : ControllerBase
  {
    private readonly IPriceLogService _service;
    private readonly ILogger<ProductsController> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="service"></param>
    /// <param name="logger"></param>
    public PriceLogController(IPriceLogService service, ILogger<ProductsController> logger)
    {
      _service = service;
      _logger = logger;
    }

    /// <summary>
    /// Fetches all price logs
    /// </summary>
    /// <returns>A collection of price logs</returns>
    /// <response code="200">OK if it was a successful fetch</response>
    /// <response code="404">Could not find any price logs in the database</response>
    /// <response code="500">Database failure</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<PriceLog>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<Product>>> GetAllPriceLogs()
    {
      try
      {
        _logger.LogInformation("Fetching Price Logs");

        var priceLogs = await _service.GetAllPriceLogsAsync();
        if (!priceLogs.Any()) return NotFound("There are no price logs stored in the database");

        return Ok(priceLogs.ToList());
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
      }
    }

    /// <summary>
    /// Fetches all price logs for a specific productId
    /// </summary>
    /// <returns>A collection of price logs for a specific productId</returns>
    /// <response code="200">OK if it was a successful fetch</response>
    /// <response code="404">Could not find any price logs with given productId</response>
    /// <response code="500">Database failure</response>
    [HttpGet("{productId:int}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public ActionResult<List<Product>> GetPriceLogsByProductId([FromRoute] int productId)
    {
      try
      {
        _logger.LogInformation($"Fetching Price Logs for ProductId {productId}");

        var priceLogs = _service.GetPriceLogByProductId(productId);
        if (!priceLogs.Any()) return NotFound($"No price logs were found for product id '{productId}'");
        return Ok(priceLogs.ToList());
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
      }
    }
  }
}
