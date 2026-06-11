using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class SyncController : ControllerBase
    {
        private readonly ISyncService
        _syncService;

        public SyncController(
            ISyncService syncService)
        {
            _syncService = syncService;
        }

        [HttpPost]
        public async Task<IActionResult>
        Download(
            DownloadRequestDto request)
        {
            var result =
                await _syncService
                .DownloadAsync(request);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult>
        Upload(
            UploadRequestDto request)
        {
            var result =
                await _syncService
                .UploadAsync(request);

            return Ok(result);
        }
    }
}
