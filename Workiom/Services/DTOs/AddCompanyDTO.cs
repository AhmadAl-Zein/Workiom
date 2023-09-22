using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Workiom.Services.DTOs
{
    public class AddCompanyDTO
    {
        [Required]
        public string Name { get; set; }
        public int NumberOfEmployees { get; set; }
        public Dictionary<string, string>? ExtraFields { get; set; }
    }
}
