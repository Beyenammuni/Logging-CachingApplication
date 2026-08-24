using Logging_CachingApplication.Product.Queries.GetProducts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logging_CachingApplication.Product.Queries.GetProductById
{
    public sealed record GetProductByIdQuery(int Id) : IRequest<GetProductByIdResponse>;
}
