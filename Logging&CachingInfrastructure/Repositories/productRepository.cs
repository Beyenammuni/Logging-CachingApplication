using Logging_CachingApplication.Common.Interfaces;
using Logging_CachingDomain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logging_CachingInfrastructure.Repositories
{
    public class productRepository : IProductRepository
    {
        private readonly IAppDbContext _context;

        public productRepository(IAppDbContext context)
        {
            _context = context;
        }

        public async Task Add(Product entity)
        {
            await _context.products.AddAsync(entity);
        }

        public void Delete(Product entity)
        {
            _context.products.Remove(entity);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
           return await _context.products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int? id)
        {
           return await _context.products.FindAsync(id);
        }

        public void Update(Product entity)
        {
            // Attach if not tracked and mark modified
            var tracked = _context.products.Local.FirstOrDefault(x => x.Id == entity.Id);
            if (tracked == null)
            {
                _context.products.Attach(entity);
            }
            _context.products.Update(entity);
        }
    }
}
