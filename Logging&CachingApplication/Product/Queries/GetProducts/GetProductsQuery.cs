using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logging_CachingApplication.Product.Queries.GetProducts
{
    public sealed record GetProductsQuery : IRequest<IEnumerable<Logging_CachingDomain.Models.Product>>;

}
