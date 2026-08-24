using Logging_CachingApplication.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Logging_CachingApplication.Product.Commands.UpdateProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IProductRepository _repository;
        private readonly IAppDbContext _context;
        private readonly IRedisService _redis;

        public UpdateProductHandler(IProductRepository repository, IAppDbContext context, IRedisService redis)
        {
            _repository = repository;
            _context = context;
            _redis = redis;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);
            if (product == null)
                throw new KeyNotFoundException($"Product with id {request.Id} not found.");

            product.Name = request.Name;
            product.Description = request.Description;
        
            product.Quentity = request.Quatity;
            product.Price = request.Price;

            _repository.Update(product);
            await _context.SaveChangesAsync(cancellationToken);

            // Invalidate cache entries related to products
            await _redis.DeleteAsync($"products:{product.Id}");
            await _redis.DeleteAsync("products:all");

            return Unit.Value;
        }
    }
}
