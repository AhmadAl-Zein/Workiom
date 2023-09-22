using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Workiom.Services.DTOs
{
    public class GetContactsFilterDTO
    {
        [DefaultValue("")]
        public string? Name { get; set; }
        [MinLength(24)]
        public string? CompanyId { get; set; }
        public Dictionary<string, string>? ExtraFields { get; set; }
    }
}
