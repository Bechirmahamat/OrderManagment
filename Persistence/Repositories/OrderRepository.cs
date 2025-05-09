using Core.Interface;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context, ILogger<GenericRepository<Order>> logger) : base(context, logger)
        {
        }
    }
}
