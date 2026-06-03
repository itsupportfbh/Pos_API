namespace UNITYPOS_API.Entities.Master
{
    public class TranslationMaster
    {
        public int Id { get; set; }
        public string TranslationKey { get; set; } = string.Empty;
        public string? Module { get; set; }
        public string DefaultText { get; set; } = string.Empty;
        public string? Remarks { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
