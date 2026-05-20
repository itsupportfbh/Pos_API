namespace UNITYPOS_API.Entities
{
    public class FileUploadResult
    {
        public string Url { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
    }
}
