using System.ComponentModel.DataAnnotations.Schema;

namespace UNITYPOS_API.Entities.Master
{
    public class OrganizationConfig : CommonClass
    {
        public int Id { get; set; }
        public string? Image { get; set; }
        public string? ThemeColor { get; set; }
        public int FontSize { get; set; }
        public int OrgId { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
