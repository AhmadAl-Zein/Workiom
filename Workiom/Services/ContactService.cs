using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using Workiom.Common.DTOs;
using Workiom.Config;
using Workiom.Models;
using Workiom.Services.DTOs;

namespace Workiom.Services
{
    public class ContactService : IContactService
    {
        private readonly IMongoCollection<Contact> _contactCollection;

        public ContactService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _contactCollection = database.GetCollection<Contact>("contacts");
        }

        public async Task Create(AddContactDTO input)
        {
            var checkName = await _contactCollection.Find(company => company.Name == input.Name).FirstOrDefaultAsync();
            if (checkName is not null) throw new Exception($"Name {input.Name} is already exists");

            await _contactCollection.InsertOneAsync(new Contact
            {
                Name = input.Name,
                CompanyIds = input.CompanyIds,
                ExtraFields = input.ExtraFields
            });
        }

        public async Task<Contact> GetById(string id)
        {
            var contact = await _contactCollection.Find(company => company.Id == id).FirstOrDefaultAsync();
            if (contact is null) throw new Exception($"Contact with id {id} not found");
            return contact;
        }

        public async Task<List<Contact>> Get() => await _contactCollection.Find(contact => true).ToListAsync();

        public async Task<List<Contact>> GetWithPagination(GetContactsFilterDTO input, PaginationQueryDTO pagination)
        {
            var filterBuilder = Builders<Contact>.Filter;
            var filters = new List<FilterDefinition<Contact>>();

            if (!string.IsNullOrEmpty(input.Name))
            {
                var nameFilter = filterBuilder.Regex(contact => contact.Name, new BsonRegularExpression($".*{input.Name}.*", "i"));
                filters.Add(nameFilter);
            }

            if (!string.IsNullOrEmpty(input.CompanyId))
            {
                var companyIdFilter = filterBuilder.AnyEq(contact => contact.CompanyIds, input.CompanyId);
                filters.Add(companyIdFilter);
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

            return combinedFilter != null ? _contactCollection.Find(combinedFilter).Skip(pagination.Size * (pagination.Page - 1)).Limit(pagination.Size).ToList() :
                                            _contactCollection.Find(contact => true).Skip(pagination.Size * (pagination.Page - 1)).Limit(pagination.Size).ToList();
        }

        public async Task Update(string id, UpdateContactDTO input)
        {
            var contact = await GetById(id);
            var filter = Builders<Contact>.Filter.Eq(contact => contact.Id, id);
            var update = Builders<Contact>.Update
                .Set(contact => contact.Name, input.Name ?? contact.Name)
                .Set(contact => contact.CompanyIds, input.CompanyIds ?? contact.CompanyIds);

            if (input.ExtraFields != null)
            {
                update = update.Set(contact => contact.ExtraFields, input.ExtraFields);
            }

            var result = _contactCollection.UpdateOne(filter, update);

            if (result.ModifiedCount == 0)
            {
                throw new Exception($"Contact with id {id} not updated");
            }
        }

        public async Task Delete(string id)
        {
            var contact = await GetById(id);
            await _contactCollection.DeleteOneAsync(contact => contact.Id == id);
        }
    }
}
