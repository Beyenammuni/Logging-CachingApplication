using System;
using System.Collections.Generic;
using System.Text;
using Logging_CachingDomain.Models;

namespace Logging_CachingApplication.Common.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Logging_CachingDomain.Models.Product>> GetAllAsync();
        Task<Logging_CachingDomain.Models.Product> GetByIdAsync(int? id);
        Task Add(Logging_CachingDomain.Models.Product entity);
        void Update(Logging_CachingDomain.Models.Product entity);
        void Delete(Logging_CachingDomain.Models.Product entity);
    }
}
