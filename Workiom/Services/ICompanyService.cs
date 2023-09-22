using Workiom.Common.DTOs;
using Workiom.Models;
using Workiom.Services.DTOs;

namespace Workiom.Services
{
    public interface ICompanyService
    {
        Task Create(AddCompanyDTO input);
        Task<Company> GetById(string id);
        Task<List<Company>> Get();
        Task<List<Company>> GetWithPagination(GetCompaniesFilterDTO input, PaginationQueryDTO pagination);
        Task Update(string id, UpdateCompanyDTO input);
        Task Delete(string id);
    }
}
