using Logging_CachingApplication.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Logging_CachingApplication.Product.Queries.GetProductById
{
    public sealed class GetProductByIdHandlerQuery : IRequestHandler<GetProductByIdQuery, GetProductByIdResponse>
    {
        private readonly IProductRepository _repository;
        private readonly IRedisService _redis;

        public GetProductByIdHandlerQuery(IProductRepository repository, IRedisService redis)
        {
            _repository = repository;
            _redis = redis;
        }

        public async Task<GetProductByIdResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            // Try cache first
            var cacheKey = $"products:{request.Id}";
            var cached = await _redis.GetAsync<GetProductByIdResponse>(cacheKey);
            if (cached != null)
                return cached;

            var product = await _repository.GetByIdAsync(request.Id);
            if (product == null)
                throw new KeyNotFoundException($"Product with id {request.Id} not found.");

            var response = new GetProductByIdResponse(product.Id, product.Name, product.Description ?? string.Empty, product.Price, product.Quentity);

            // cache the result
            await _redis.SetAsync(cacheKey, response, TimeSpan.FromMinutes(30));

            return response;
        }
    }
}
