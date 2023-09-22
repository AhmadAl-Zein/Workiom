namespace Workiom.Services.DTOs
{
    public class UpdateContactDTO
    {
        public string? Name { get; set; }
        public List<string>? CompanyIds { get; set; }
        public Dictionary<string, string>? ExtraFields { get; set; }
    }
}
