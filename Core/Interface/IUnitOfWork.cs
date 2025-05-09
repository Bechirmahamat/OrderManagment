namespace Core.Interface
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        IOrderItemRepository OrderItems { get; }
        ICategoryRepository Categories { get; }
        ICompanyRespository Companies { get; }
        Task<bool> SaveChangesAsync();
    }
}
