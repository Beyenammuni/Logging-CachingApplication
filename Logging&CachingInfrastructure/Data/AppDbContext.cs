using Logging_CachingApplication.Common.Interfaces;
using Logging_CachingDomain.Models;
using Logging_CachingInfrastructure.Data.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Logging_CachingInfrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> dbContext) : DbContext(options: dbContext), IAppDbContext
    {
        public DbSet<Product> products => Set<Product>();
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfigurationsFromAssembly(
           typeof(ProductConfiguration).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
