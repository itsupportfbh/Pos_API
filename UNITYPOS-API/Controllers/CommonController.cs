using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.DAL.Services;
using UNITYPOS_API.Entities;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CommonController : ControllerBase
    {

        private readonly ICommonService _commonService;
        private readonly ILogger<CommonController> _logger;

        public CommonController(
            ICommonService commonService,
            ILogger<CommonController> logger)
        {
            _commonService = commonService;
            _logger = logger;
        }

        [HttpGet]
        public string GetCountry()
        {
            string result = null;

            result = JsonConvert.SerializeObject(_commonService.GetCountry());

            return Common.Utility.GetResult(result);
        }

        [HttpGet]
        public string GetStateByCountryId(int CountryId)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_commonService.GetStateByCountryId(CountryId));
            return Common.Utility.GetResult(result);
        }


        [HttpGet]
        public string GetCityByStateId(int StateId)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_commonService.GetCityByStateId(StateId));
            return Common.Utility.GetResult(result);
        }



        [HttpPost]
        public async Task<IActionResult> FileUpload(String FolderName)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Request;

                foreach (var file in httpRequest.Form.Files)
                {
                    var result = await _commonService.FileUpload(file, FolderName);
                    return Created("", result);
                }
                var res = "Please upload an image or voice recording.";
                dict.Add("error", res);
                return NotFound(dict);
            }
            catch (Exception ex)
            {
                var res = "Some error occurred.";
                dict.Add("error", res);
                Exception objErr = ex.GetBaseException();
                _logger.LogError($"Error: {objErr}, MethodName=PostUserImage");
                return NotFound(dict);
            }
        }

        [HttpGet]
        public string GetBranchByUserId(int UserId)
        {
            string result = null;

            result = JsonConvert.SerializeObject(_commonService.GetBranchByUserId(UserId));
            return Common.Utility.GetResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromForm] SendEmailRequest request)
        {
            try
            {
                byte[]? fileBytes = null;
                string? fileName = null;
                string? contentType = null;

                if (request.Attachment != null && request.Attachment.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await request.Attachment.CopyToAsync(memoryStream);
                    fileBytes = memoryStream.ToArray();
                    fileName = request.Attachment.FileName;
                    contentType = request.Attachment.ContentType;
                }

                await _commonService.SendEmail(
                    request.ToEmail ?? string.Empty,
                    request.CcEmail,
                    request.Subject ?? string.Empty,
                    request.Body ?? string.Empty,
                    fileBytes,
                    fileName,
                    contentType);

                return Ok(new
                {
                    Message = "Mail sent successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending email.");
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

    }
}
