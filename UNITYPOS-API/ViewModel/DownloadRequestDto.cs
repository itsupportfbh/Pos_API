namespace UNITYPOS_API.ViewModel
{
    public class DownloadRequestDto
    {
        public string TableName { get; set; }
        public DateTime? LastSyncDate { get; set; }
        public long LastDownloadId { get; set; }
    }
}
