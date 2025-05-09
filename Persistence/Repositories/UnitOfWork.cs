using Core.Interface;
using Microsoft.Extensions.Logging;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IProductRepository Products { get; }

        public IOrderRepository Orders { get; }

        public IOrderItemRepository OrderItems { get; }

        public ICategoryRepository Categories { get; }

        public ICompanyRespository Companies { get; }



        private readonly AppDbContext context;
        public UnitOfWork(AppDbContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            Products = new ProductRepository(context, loggerFactory.CreateLogger<ProductRepository>());
            Orders = new OrderRepository(context, loggerFactory.CreateLogger<OrderRepository>());
            OrderItems = new OrderItemRepository(context, loggerFactory.CreateLogger<OrderItemRepository>());
            Categories = new CategoryRepository(context, loggerFactory.CreateLogger<CategoryRepository>());
            Companies = new CompanyRepository(context, loggerFactory.CreateLogger<CompanyRepository>());

        }
        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
