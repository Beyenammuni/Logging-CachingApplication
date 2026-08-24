using Logging_CachingApplication.Common.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Logging_CachingApplication.Product.Queries.GetProducts
{
    public sealed class GetProductsHandlerQuery : IRequestHandler<GetProductsQuery, IEnumerable<Logging_CachingDomain.Models.Product>>
    {
        private readonly IProductRepository _repository;
        private readonly IRedisService _redis;

        public GetProductsHandlerQuery(IProductRepository repository, IRedisService redis)
        {
            _repository = repository;
            _redis = redis;
        }

        public async Task<IEnumerable<Logging_CachingDomain.Models.Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "products:all";
            var cached = await _redis.GetAsync<IEnumerable<Logging_CachingDomain.Models.Product>>(cacheKey);
            if (cached != null)
                return cached;

            var products = await _repository.GetAllAsync();
            await _redis.SetAsync(cacheKey, products, TimeSpan.FromMinutes(30));
            return products;
        }
    }
}
