namespace UNITYPOS_API.Entities.Master
{
    public class DualDisplayProfile : CommonClass
    {
        public int Id { get; set; }
        public string ProfileCode { get; set; } = string.Empty;
        public string ProfileName { get; set; } = string.Empty;
        public int OrgId { get; set; }
        public int BranchId { get; set; }
        public int CounterId { get; set; }
        public string? ThemeName { get; set; }
        public string? HeaderTitle { get; set; }
        public string? WelcomeMessage { get; set; }
        public string? IdleMessage { get; set; }
    }
}
