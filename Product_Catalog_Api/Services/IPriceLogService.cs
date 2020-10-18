using System.Collections.Generic;
using System.Threading.Tasks;
using Product_Catalog_Api.Models;

namespace Product_Catalog_Api.Services
{
  public interface IPriceLogService
  {
    Task<IEnumerable<PriceLog>> GetAllPriceLogsAsync();
    List<PriceLog> GetPriceLogByProductId(int productId);
  }
}
