using Logging_CachingApplication.Common.Interfaces;
using MediatR;


namespace Logging_CachingApplication.Product.Commands.DeleteProduct
{
    public class DeleteHandlerCommand : IRequestHandler<DeleteProductCommand, Unit>
    {

        private readonly IProductRepository _repository;
        private readonly IAppDbContext _context;
        private readonly IRedisService _redis;

        public DeleteHandlerCommand(IProductRepository repository, IAppDbContext context, IRedisService redis)
        {
            _repository = repository;
            _context = context;
            _redis = redis;
        }
        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
           var product = await _repository.GetByIdAsync(request.Id);
            if(product == null)
                throw new KeyNotFoundException($"Product with id {request.Id} not found.");
            _repository.Delete(product);
            await _context.SaveChangesAsync(cancellationToken);

            // invalidate cache
            await _redis.DeleteAsync($"products:{product.Id}");
            await _redis.DeleteAsync("products:all");
            return Unit.Value;
        }
    }
}