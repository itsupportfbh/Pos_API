using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface ISyncService
    {
        Task<object> DownloadAsync(
        DownloadRequestDto request);

        Task<bool> UploadAsync(
            UploadRequestDto request);
    }
}
