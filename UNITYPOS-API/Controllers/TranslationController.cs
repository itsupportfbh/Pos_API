using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UNITYPOS_API.DAL.Interfaces;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class TranslationController : ControllerBase
    {
        private readonly ITranslationService _translationService;

        public TranslationController(ITranslationService translationService)
        {
            _translationService = translationService;
        }

        [HttpGet]
        public string GetTranslations(string? LanguageCode, string? FallbackLanguageCode = "en-IN")
        {
            var result = JsonConvert.SerializeObject(_translationService.GetTranslations(LanguageCode, FallbackLanguageCode));
            return Common.Utility.GetResult(result);
        }
    }
}
