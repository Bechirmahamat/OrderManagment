using Core.Interface;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context, ILogger<GenericRepository<Product>> logger) : base(context, logger)
        {
        }
    }
}
