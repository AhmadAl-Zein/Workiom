using MongoDB.Bson;

namespace Workiom.Services.DTOs
{
    public class UpdateCompanyDTO
    {
        public string? Name { get; set; }
        public int? NumberOfEmployees { get; set; }
        public Dictionary<string, string>? ExtraFields { get; set; }
    }
}
