using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Xml.Linq;
using Workiom.Common.DTOs;
using Workiom.Config;
using Workiom.Models;
using Workiom.Services.DTOs;

namespace Workiom.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IMongoCollection<Company> _companyCollection;

        public CompanyService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _companyCollection = database.GetCollection<Company>("companies");
        }

        public async Task Create(AddCompanyDTO input)
        {
            var checkName = await _companyCollection.Find(company => company.Name == input.Name).FirstOrDefaultAsync();
            if (checkName is not null) throw new Exception($"Name {input.Name} is already exists");

            await _companyCollection.InsertOneAsync(new Company
            {
                Name = input.Name,
                NumberOfEmployees = input.NumberOfEmployees,
                ExtraFields = input.ExtraFields
            });
        }

        public async Task<Company> GetById(string id)
        {
            var company = await _companyCollection.Find(company => company.Id == id).FirstOrDefaultAsync();
            if (company is null) throw new Exception($"Company with id {id} not found");
            return company;
        }

        public async Task<List<Company>> Get() => await _companyCollection.Find(company => true).ToListAsync();
        public async Task<List<Company>> GetWithPagination(GetCompaniesFilterDTO input, PaginationQueryDTO pagination)
        {
            var filterBuilder = Builders<Company>.Filter;
            var filters = new List<FilterDefinition<Company>>();

            if (!string.IsNullOrEmpty(input.Name))
            {
                var nameFilter = filterBuilder.Regex(company => company.Name, new BsonRegularExpression($".*{input.Name}.*", "i"));
                filters.Add(nameFilter);
            }

            if (input.FromNumberOfEmployees.HasValue)
            {
                var minEmployeesFilter = filterBuilder.Gte(company => company.NumberOfEmployees, input.FromNumberOfEmployees.Value);
                filters.Add(minEmployeesFilter);
            }

            if (input.ToNumberOfEmployees.HasValue)
            {
                var maxEmployeesFilter = filterBuilder.Lte(company => company.NumberOfEmployees, input.ToNumberOfEmployees.Value);
                filters.Add(maxEmployeesFilter);
            }

            if (input.ExtraFields != null)
            {
                foreach (var extraField in input.ExtraFields)
                {
                    var extraFieldFilter = filterBuilder.Eq($"extraFields.{extraField.Key}", extraField.Value);
                    filters.Add(extraFieldFilter);
                }
            }

            var combinedFilter = filters.Count() == 0 ? null : filterBuilder.And(filters);

            return combinedFilter != null ? _companyCollection.Find(combinedFilter).Skip(pagination.Size * (pagination.Page - 1)).Limit(pagination.Size).ToList() : 
                                            _companyCollection.Find(company => true).Skip(pagination.Size * (pagination.Page - 1)).Limit(pagination.Size).ToList();
        }

        public async Task Update(string id, UpdateCompanyDTO input)
        {
            var company = await GetById(id);
            var filter = Builders<Company>.Filter.Eq(company => company.Id, id);
            var update = Builders<Company>.Update
                .Set(company => company.Name, input.Name ?? company.Name)
                .Set(company => company.NumberOfEmployees, input.NumberOfEmployees ?? company.NumberOfEmployees);

            if (input.ExtraFields != null)
            {
                update = update.Set(company => company.ExtraFields, input.ExtraFields);
            }

            var result = _companyCollection.UpdateOne(filter, update);

            if (result.ModifiedCount == 0)
            {
                throw new Exception($"Company with id {id} not updated");
            }
        }

        public async Task Delete(string id)
        {
            var company = await GetById(id);
            await _companyCollection.DeleteOneAsync(company => company.Id == id);
        }
    }
}
