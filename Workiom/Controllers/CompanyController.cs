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
    public class CompanyController : ControllerBase
    {
        private readonly CompanyService _companyService;

        public CompanyController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost]
        public async Task<ResponseDTO> AddCompany(AddCompanyDTO body)
        {
            await _companyService.Create(body);
            return new ResponseDTO()
            {
                Success = true,
                Data = "Company added successfully"
            };
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ResponseDTO> GetCompanyById(string id)
        {
            var company = await _companyService.GetById(id);
            return new ResponseDTO
            {
                Success = true,
                Data = company
            };
        }

        [HttpGet]
        public async Task<ResponseDTO> GetCompanies()
        {
            var companies = await _companyService.Get();
            return new ResponseDTO
            {
                Success = true,
                Data = companies
            };
        }

        [HttpPost("get-with-pagination")]
        public async Task<ResponseDTO> GetCompaniesWithPagination([FromQuery] PaginationQueryDTO pagination, GetCompaniesFilterDTO body)
        {
            var companies = await _companyService.GetWithPagination(body, pagination);
            return new ResponseDTO
            {
                Success = true,
                Data = companies
            };
        }

        [HttpPut("{id:length(24)}")]
        public async Task<ResponseDTO> UpdateCompany(string id, [FromBody] UpdateCompanyDTO body)
        {
            await _companyService.Update(id, body);
            return new ResponseDTO
            {
                Success = true,
                Data = "Company updated successfully"
            };
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<ResponseDTO> DeleteCompany(string id)
        {
            await _companyService.Delete(id);
            return new ResponseDTO
            {
                Success = true,
                Data = "Company deleted successfully"
            };
        }
    }
}
