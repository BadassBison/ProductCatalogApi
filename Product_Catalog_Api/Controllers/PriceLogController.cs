using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Product_Catalog_Api.Dtos;
using Product_Catalog_Api.Services;

namespace Product_Catalog_Api.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class PriceLogController : ControllerBase
  {
    private readonly IPriceLogService _service;
    private readonly ILogger<ProductsController> _logger;

    public PriceLogController(IPriceLogService service, ILogger<ProductsController> logger)
    {
      _service = service;
      _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllPriceLogs()
    {
      try
      {
        var priceLogs = await _service.GetAllPriceLogsAsync();
        if (!priceLogs.Any()) return NotFound("There are no price logs stored in the database");

        return Ok(priceLogs);
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
      }
    }

    [HttpGet("{productId:int}")]
    public ActionResult<IEnumerable<Product>> GetPriceLogsByProductId([FromRoute] int productId)
    {
      try
      {
        var priceLogs = _service.GetPriceLogByProductId(productId);
        if (!priceLogs.Any()) return NotFound($"No price logs were found for product id '{productId}'");
        return Ok(priceLogs);
      }
      catch (Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
      }
    }
  }
}
