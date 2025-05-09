namespace Core.Interface
{
    public interface IGenericRepository<T>
    {
        Task<T?> GetById(Guid id);
        Task<bool> Create(T entity);
        Task<IEnumerable<T>> GetAll();
        Task<bool> Update(T entity);
        Task<bool> Delete(Guid id);
    }
}
