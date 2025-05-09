using Core.Interface;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class CategoryRepository : GenericRepository<ProductCategory>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context, ILogger<GenericRepository<ProductCategory>> logger) : base(context, logger)
        {
        }
    }
}
