using Core.Interface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Data;

namespace Persistence.Repositories
{

    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext context;
        protected readonly ILogger<GenericRepository<T>> logger;
        protected readonly DbSet<T> dbSet;

        public GenericRepository(AppDbContext context, ILogger<GenericRepository<T>> logger)
        {
            this.context = context;
            this.logger = logger;
            dbSet = context.Set<T>();
        }

        public virtual async Task<bool> Create(T entity)
        {
            try
            {
                var data = await dbSet.AddAsync(entity);
                return data.State == EntityState.Added;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating entity");
                return false;
            }
        }

        public virtual async Task<bool> Delete(Guid id)
        {
            try
            {
                var data = await dbSet.FindAsync(id);
                if (data == null)
                {
                    logger.LogWarning("Entity not found");
                    return false;
                }
                dbSet.Remove(data);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting entity");
                return false;
            }
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                return await dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving entities");
                return null;
            }
        }

        public virtual async Task<T?> GetById(Guid id)
        {
            try
            {
                return await dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving entity");
                return null;
            }
        }

        public async Task<List<T>> GetByIds(IEnumerable<Guid> ids)
        {
            return await dbSet
                .Where(e => ids.Contains(e.Id))
                .ToListAsync();
        }

        public virtual Task<bool> Update(T entity)
        {
            try
            {
                var data = dbSet.Update(entity);
                return Task.FromResult(data.State == EntityState.Modified);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating entity");
                return Task.FromResult(false);
            }
        }
    }
}
