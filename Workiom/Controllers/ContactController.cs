using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Workiom.Common.DTOs;
using Workiom.Models;
using Workiom.Services;
using Workiom.Services.DTOs;

namespace Workiom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ContactService _contactService;

        public ContactController(ContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpPost]
        public async Task<ResponseDTO> AddContact(AddContactDTO body)
        {
            await _contactService.Create(body);
            return new ResponseDTO()
            {
                Success = true,
                Data = "Contact added successfully"
            };
        }

        [HttpGet]
        public async Task<ResponseDTO> GetContacts()
        {
            var contacts = await _contactService.Get();
            return new ResponseDTO
            {
                Success = true,
                Data = contacts
            };
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ResponseDTO> GetContactById(string id)
        {
            var contact = await _contactService.GetById(id);
            return new ResponseDTO
            {
                Success = true,
                Data = contact
            };
        }

        [HttpPost("get-with-pagination")]
        public async Task<ResponseDTO> GetContactsWithPagination([FromQuery] PaginationQueryDTO pagination, GetContactsFilterDTO body)
        {
            var contacts = await _contactService.GetWithPagination(body, pagination);
            return new ResponseDTO
            {
                Success = true,
                Data = contacts
            };
        }

        [HttpPut("{id:length(24)}")]
        public async Task<ResponseDTO> UpdateContact(string id, [FromBody] UpdateContactDTO body)
        {
            await _contactService.Update(id, body);
            return new ResponseDTO
            {
                Success = true,
                Data = "Contact updated successfully"
            };
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<ResponseDTO> DeleteCompany(string id)
        {
            await _contactService.Delete(id);
            return new ResponseDTO
            {
                Success = true,
                Data = "Contact deleted successfully"
            };
        }
    }
}
