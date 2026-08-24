using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logging_CachingApplication.Product.Commands.CreateProduct
{
    public sealed record CreateProductCommand(
        string Name,
        string? Description,
        decimal Price,
        int? Quatity
   ) : IRequest<int>;
}
