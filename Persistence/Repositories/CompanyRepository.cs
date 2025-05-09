using Core.Interface;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRespository
    {
        public CompanyRepository(AppDbContext context, ILogger<GenericRepository<Company>> logger) : base(context, logger)
        {
        }
    }

}
