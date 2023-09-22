using Workiom.Common.DTOs;
using Workiom.Models;
using Workiom.Services.DTOs;

namespace Workiom.Services
{
    public interface IContactService
    {
        Task Create(AddContactDTO input);
        Task<Contact> GetById(string id);
        Task<List<Contact>> Get();
        Task<List<Contact>> GetWithPagination(GetContactsFilterDTO input, PaginationQueryDTO pagination);
        Task Update(string id, UpdateContactDTO input);
        Task Delete(string id);
    }
}
