using Domain.Entities;

namespace Core.Interface
{
    public interface ICompanyRespository : IGenericRepository<Company>
    {
        Task<Company?> GetCompanyByManagerId(string managerId);

    }
}
