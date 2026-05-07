using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class CodeTemplateService : ICodeTemplateService
    {
        private readonly IUnitOfWork _uow;

        public CodeTemplateService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public string CreateCodetemplate(List<CodeTemplate> codeTemplates)
        {
            if (codeTemplates == null || codeTemplates.Count == 0)
            {
                return string.Empty;
            }

            foreach (var codeTemplate in codeTemplates)
            {
                if (codeTemplate == null || codeTemplate.OrgId <= 0)
                {
                    continue;
                }

                var existingCodeTemplate = _uow.GenericRepository<CodeTemplate>().Table()
                    .FirstOrDefault(x => x.IsDeleted == false
                                      && x.EntityNo == codeTemplate.EntityNo
                                      && x.OrgId == codeTemplate.OrgId
                                      && (codeTemplate.IsMaster || x.BranchId == codeTemplate.BranchId));

                if (existingCodeTemplate != null)
                {
                    existingCodeTemplate.Name = codeTemplate.Name;
                    existingCodeTemplate.NoOfDigit = codeTemplate.NoOfDigit;
                    existingCodeTemplate.StartValue = codeTemplate.StartValue;
                    existingCodeTemplate.Prefix = codeTemplate.Prefix;
                    existingCodeTemplate.CurrentValue = codeTemplate.CurrentValue;
                    existingCodeTemplate.Suffix = codeTemplate.Suffix;
                    existingCodeTemplate.BranchId = codeTemplate.BranchId;
                    existingCodeTemplate.IsMaster = codeTemplate.IsMaster;
                    existingCodeTemplate.IsDateMonthYearWise = codeTemplate.IsDateMonthYearWise;
                    existingCodeTemplate.IsActive = codeTemplate.IsActive;
                    existingCodeTemplate.UpdatedBy = codeTemplate.UpdatedBy;
                    existingCodeTemplate.UpdatedDate = DateTime.Now;

                    _uow.GenericRepository<CodeTemplate>().Update(existingCodeTemplate);
                }
                else
                {
                    var entity = new CodeTemplate
                    {
                        EntityNo = codeTemplate.EntityNo,
                        Name = codeTemplate.Name,
                        NoOfDigit = codeTemplate.NoOfDigit,
                        StartValue = codeTemplate.StartValue,
                        Prefix = codeTemplate.Prefix,
                        CurrentValue = codeTemplate.CurrentValue,
                        Suffix = codeTemplate.Suffix,
                        BranchId = codeTemplate.BranchId,
                        IsMaster = codeTemplate.IsMaster,
                        IsDateMonthYearWise = codeTemplate.IsDateMonthYearWise,
                        OrgId = codeTemplate.OrgId,
                        IsActive = codeTemplate.IsActive,
                        IsDeleted = false,
                        CreatedBy = codeTemplate.CreatedBy,
                        CreatedDate = DateTime.Now
                    };

                    _uow.GenericRepository<CodeTemplate>().Insert(entity);
                }
            }

            _uow.Save();

            return "Success";
        }

        public IEnumerable<Object> GetAllCodeTemplate(int OrgId, int BranchId, bool IsMaster)
        {
            var orgResult = _uow.GenericRepository<CodeTemplate>().Table()
                .Where(x => x.IsDeleted == false && x.EntityNo != 2 && x.OrgId == OrgId
                         && x.IsMaster == IsMaster
                         && (IsMaster || x.BranchId == BranchId))
                .Select(x => new
                {
                    x.Id,
                    x.EntityNo,
                    x.Name,
                    x.NoOfDigit,
                    x.StartValue,
                    x.Prefix,
                    x.CurrentValue,
                    x.Suffix,
                    x.BranchId,
                    x.IsMaster,
                    x.IsDateMonthYearWise,
                    x.OrgId,
                    x.IsActive,
                })
                .ToList();

            if (orgResult.Count > 0)
            {
                return orgResult;
            }

            var defaultResult = _uow.GenericRepository<CodeTemplate>().Table()
                .Where(x => x.IsDeleted == false && x.EntityNo != 2 && x.OrgId == 0
                         && x.IsMaster == IsMaster
                         && (IsMaster || x.BranchId == BranchId))
                .Select(x => new
                {
                    x.Id,
                    x.EntityNo,
                    x.Name,
                    x.NoOfDigit,
                    x.StartValue,
                    x.Prefix,
                    x.CurrentValue,
                    x.Suffix,
                    x.BranchId,
                    x.IsMaster,
                    x.IsDateMonthYearWise,
                    x.OrgId,
                    x.IsActive,
                })
                .ToList();

            return defaultResult;
        }

        public string GetLatestCode(int EntityNo, int OrgId, int BranchId)
        {
            var query = _uow.GenericRepository<CodeTemplate>().Table()
                .Where(x => x.IsDeleted == false
                         && x.IsActive == true
                         && x.EntityNo == EntityNo);

            CodeTemplate selectedTemplate = null;

            if (EntityNo == 2)
            {
                selectedTemplate = query
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();
            }
            else
            {
                selectedTemplate = query
                    .Where(x => x.OrgId == OrgId
                             && (x.IsMaster || x.BranchId == BranchId))
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();

                if (selectedTemplate == null)
                {
                    selectedTemplate = query
                        .Where(x => x.OrgId == 0
                                 && (x.IsMaster || x.BranchId == BranchId))
                        .OrderByDescending(x => x.Id)
                        .FirstOrDefault();
                }
            }

            if (selectedTemplate == null)
            {
                return "";
            }

            return BuildLatestCode(selectedTemplate);
        }

        private static string BuildLatestCode(CodeTemplate codeTemplate)
        {
            string prefix = (codeTemplate.Prefix ?? string.Empty).ToUpper();
            string suffix = (codeTemplate.Suffix ?? string.Empty).ToUpper();
            string startValue = codeTemplate.StartValue.ToString();
            int nextValue = codeTemplate.CurrentValue + 1;
            int balanceDigits = codeTemplate.NoOfDigit - startValue.Length;

            string numericPart = balanceDigits > 0
                ? startValue + nextValue.ToString().PadLeft(balanceDigits, '0')
                : startValue + nextValue.ToString();

            if (codeTemplate.IsDateMonthYearWise)
            {
                return DateTime.Now.ToString("ddMMyyyy") + "-" + prefix + numericPart + suffix;
            }

            return prefix + numericPart + suffix;
        }
    }
}
