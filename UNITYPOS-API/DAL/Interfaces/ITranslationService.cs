namespace UNITYPOS_API.DAL.Interfaces
{
    public interface ITranslationService
    {
        IEnumerable<object> GetTranslations(string? languageCode, string? fallbackLanguageCode = "en-IN");
    }
}
