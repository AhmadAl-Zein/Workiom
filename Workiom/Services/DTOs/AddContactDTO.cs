using System.ComponentModel.DataAnnotations;

namespace Workiom.Services.DTOs
{
    public class AddContactDTO
    {
        [Required]
        public string Name { get; set; }
        public List<string> CompanyIds { get; set; }
        public Dictionary<string, string>? ExtraFields { get; set; }
    }
}
