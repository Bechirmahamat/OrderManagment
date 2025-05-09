using Core.Interface;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(AppDbContext context, ILogger<GenericRepository<OrderItem>> logger) : base(context, logger)
        {
        }
    }
}
