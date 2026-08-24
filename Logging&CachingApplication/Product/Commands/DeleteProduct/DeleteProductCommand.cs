using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logging_CachingApplication.Product.Commands.DeleteProduct
{
    public sealed record DeleteProductCommand(
        int Id
   ) : IRequest<Unit>;
}
