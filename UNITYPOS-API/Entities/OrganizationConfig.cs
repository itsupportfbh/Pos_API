namespace UNITYPOS_API.Entities
{
    public class OrganizationConfig:CommonClass
    {
        public int Id { get; set; }
        public string? Image { get; set; }
        public string? ThemeColor { get; set; }
        public int FontSize { get; set; }
    }
}
