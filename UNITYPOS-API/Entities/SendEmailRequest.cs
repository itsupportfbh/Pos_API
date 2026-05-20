namespace UNITYPOS_API.Entities
{
    public class SendEmailRequest
    {
        public string? ToEmail { get; set; }
        public string? CcEmail { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public IFormFile? Attachment { get; set; }
    }
}
