using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Workiom.Services.DTOs
{
    public class GetCompaniesFilterDTO
    {
        [DefaultValue("")]
        public string? Name { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "From Number Of Employees can be minimum 1")]
        public int? FromNumberOfEmployees { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "To Number Of Employees can be minimum 1")]
        public int? ToNumberOfEmployees { get; set; }
        public Dictionary<string, string>? ExtraFields { get; set; }
    }
}
