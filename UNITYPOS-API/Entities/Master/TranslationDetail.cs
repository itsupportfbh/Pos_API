namespace UNITYPOS_API.Entities.Master
{
    public class TranslationDetail
    {
        public int Id { get; set; }
        public int TranslationId { get; set; }
        public string LanguageCode { get; set; } = string.Empty;
        public string TranslatedText { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
