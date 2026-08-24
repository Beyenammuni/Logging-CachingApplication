using System;
using System.Collections.Generic;
using System.Text;

namespace Logging_CachingApplication.Product.Queries.GetProducts
{
   public sealed record GetProductsResponse(int Id, string Name, string Description, decimal price, int? Quantity);
}
