using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Workiom.Common.DTOs
{
    public class PaginationQueryDTO
    {
        [DefaultValue(1)]
        [Range(0, double.MaxValue, ErrorMessage = "Page can be minimum 1")]
        public int Page { get; set; }

        [DefaultValue(10)]
        [Range(0, double.MaxValue, ErrorMessage = "Size can be minimum 1")]
        public int Size { get; set; }
    }
}
