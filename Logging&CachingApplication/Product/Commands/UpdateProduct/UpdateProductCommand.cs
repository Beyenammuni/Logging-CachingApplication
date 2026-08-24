using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logging_CachingApplication.Product.Commands.UpdateProduct
{
    public sealed record UpdateProductCommand(
        int Id,
        string Name,
        string? Description,
        decimal Price,
        int? Quatity
   ) : IRequest<Unit>;
}
