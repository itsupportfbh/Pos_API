using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface ICodeTemplateService
    {
        public String CreateCodetemplate(List<CodeTemplate> codeTemplates);
        public IEnumerable<Object> GetAllCodeTemplate(int OrgId, int BranchId, bool IsMaster);
        public string GetLatestCode(int EntityNo, int OrgId, int BranchId);
    }
}
