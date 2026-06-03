using Microsoft.EntityFrameworkCore;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly IUnitOfWork _uow;

        public TranslationService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public IEnumerable<object> GetTranslations(string? languageCode, string? fallbackLanguageCode = "en-IN")
        {
            var requestedLanguageCode = NormalizeLanguageCode(languageCode, "en-IN");
            var defaultLanguageCode = NormalizeLanguageCode(fallbackLanguageCode, "en-IN");

            var translationMasters = _uow.GenericRepository<TranslationMaster>()
                .Table()
                .Where(x => x.IsActive && x.IsDeleted == false)
                .AsNoTracking()
                .ToList();

            var translationDetails = _uow.GenericRepository<TranslationDetail>()
                .Table()
                .Where(x =>
                    x.IsActive &&
                    x.IsDeleted == false &&
                    (x.LanguageCode == requestedLanguageCode || x.LanguageCode == defaultLanguageCode))
                .AsNoTracking()
                .ToList();

            var result = translationMasters
                .Select(master =>
                {
                    var translatedText = translationDetails
                        .FirstOrDefault(detail =>
                            detail.TranslationId == master.Id &&
                            detail.LanguageCode == requestedLanguageCode)
                        ?.TranslatedText;

                    if (string.IsNullOrWhiteSpace(translatedText))
                    {
                        translatedText = translationDetails
                            .FirstOrDefault(detail =>
                                detail.TranslationId == master.Id &&
                                detail.LanguageCode == defaultLanguageCode)
                            ?.TranslatedText;
                    }

                    return (object)new
                    {
                        Key = master.TranslationKey,
                        Value = string.IsNullOrWhiteSpace(translatedText) ? master.DefaultText : translatedText
                    };
                })
                .ToList();

            return result;
        }

        private static string NormalizeLanguageCode(string? languageCode, string fallbackLanguageCode)
        {
            var normalizedLanguageCode = string.IsNullOrWhiteSpace(languageCode)
                ? fallbackLanguageCode
                : languageCode.Trim();

            return normalizedLanguageCode;
        }
    }
}
