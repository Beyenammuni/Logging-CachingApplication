using System;
using System.Collections.Generic;
using System.Text;

namespace Logging_CachingApplication.Product.Queries.GetProductById
{
    public sealed record GetProductByIdResponse(int Id, string Name, string Description, decimal price, int? Quantity);
}
