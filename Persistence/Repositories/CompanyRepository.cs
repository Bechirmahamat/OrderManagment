using Core.Interface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRespository
    {
        public CompanyRepository(AppDbContext context, ILogger<CompanyRepository> logger) : base(context, logger)
        {
        }

        public async Task<Company?> GetCompanyByManagerId(string managerId)
        {

            try
            {
                return await dbSet.Where(x => x.ManagerId == managerId).FirstAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving entity");
                return null;
            }
        }

    }

}
