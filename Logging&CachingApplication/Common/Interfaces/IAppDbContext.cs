using Logging_CachingDomain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logging_CachingApplication.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Logging_CachingDomain.Models.Product> products { get;  }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
