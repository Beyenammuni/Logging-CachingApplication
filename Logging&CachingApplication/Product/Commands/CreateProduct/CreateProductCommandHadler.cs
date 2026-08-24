using Logging_CachingApplication.Common.Interfaces;
using MediatR;
using Logging_CachingDomain.Models;
using System;

namespace Logging_CachingApplication.Product.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IProductRepository _repository;
        private readonly IAppDbContext _context;
        private readonly IRedisService _redis;

        public CreateProductCommandHandler(IProductRepository repository, IAppDbContext context, IRedisService redis)
        {
            _repository = repository;
            _context = context;
            _redis = redis;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Product name cannot be null or empty.");

            var product = new Logging_CachingDomain.Models.Product
            {
                Name = request.Name,
                Description = request.Description,
                Quentity = request.Quatity,
                Price = request.Price,
            };

            await _repository.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            // Invalidate cached product list
            await _redis.DeleteAsync("products:all");

            return product.Id;
        }
    }
}
