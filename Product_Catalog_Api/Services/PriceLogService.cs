using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Product_Catalog_Api.Database;
using Product_Catalog_Api.Models;
using Product_Catalog_Api.Dtos;

namespace Product_Catalog_Api.Services
{
  public class PriceLogService : IPriceLogService
  {
    public readonly ProductCatalogApiDbContext _context;

    public PriceLogService(ProductCatalogApiDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<PriceLog>> GetAllPriceLogsAsync()
    {
      return await _context.PriceLogs.ToListAsync();
    }

    public List<PriceLog> GetPriceLogByProductId(int productId)
    {
      return _context.PriceLogs
        .FromSqlInterpolated($"SELECT * FROM dbo.pricelog WHERE productId = {productId}")
        .ToList();
    }
  }

}
