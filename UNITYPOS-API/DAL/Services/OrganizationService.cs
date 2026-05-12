using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Metrics;
using System.Numerics;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.DAL.Services
{
    public class OrganizationService : IOrganizationService
    {

        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        private readonly ICodeTemplateService _codeTemplateService;

        public OrganizationService(IUnitOfWork uow, IConfiguration configuration, ICodeTemplateService codeTemplateService)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _codeTemplateService = codeTemplateService;
        }

        public string Create(Organization Organization)
        {
            int check = _uow.GenericRepository<Organization>().Table()
                .Count(o => o.Name.ToLower() == Organization.Name.ToLower()
                         && o.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            string Code = _codeTemplateService.GetLatestCode(Organization.EntityNo, 0, 0);

            var entity = new Organization
            {
                Code = Code,
                Name = Organization.Name,
                GSTNo = Organization.GSTNo,
                RegistrationNo = Organization.RegistrationNo,
                Phone = Organization.Phone,
                Email = Organization.Email,
                Website = Organization.Website,
                ContactPerson = Organization.ContactPerson,
                ContactMobileNo = Organization.ContactMobileNo,
                ContactEmail = Organization.ContactEmail,
                Address1 = Organization.Address1,
                Address2 = Organization.Address2,
                City = Organization.City,
                State = Organization.State,
                PostalCode = Organization.PostalCode,
                Country = Organization.Country,
                Remarks = Organization.Remarks,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = Organization.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<Organization>().Insert(entity);

            var codeTemplate = _uow.GenericRepository<CodeTemplate>().Table()
                               .FirstOrDefault(x => x.EntityNo == Organization.EntityNo
                                                 && x.OrgId == 0
                                                 && x.BranchId == 0
                                                 && x.IsMaster == true);

            if (codeTemplate != null)
            {
                codeTemplate.CurrentValue = codeTemplate.CurrentValue + 1;
                _uow.GenericRepository<CodeTemplate>().Update(codeTemplate);
            }

            _uow.Save();

            return Convert.ToString(entity.Id);
        }

        public string Update(Organization Organization)
        {
            int check = _uow.GenericRepository<Organization>().Table()
                .Count(o => o.Name.ToLower() == Organization.Name.ToLower() && o.Id != Organization.Id
                         && o.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var ExistingOrg = _uow.GenericRepository<Organization>().Table().Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == Organization.Id).FirstOrDefault();

            if (ExistingOrg != null)
            {
                ExistingOrg.Code = Organization.Code;
                ExistingOrg.Name = Organization.Name;
                ExistingOrg.GSTNo = Organization.GSTNo;
                ExistingOrg.RegistrationNo = Organization.RegistrationNo;
                ExistingOrg.Phone = Organization.Phone;
                ExistingOrg.Email = Organization.Email;
                ExistingOrg.Website = Organization.Website;
                ExistingOrg.ContactPerson = Organization.ContactPerson;
                ExistingOrg.ContactMobileNo = Organization.ContactMobileNo;
                ExistingOrg.ContactEmail = Organization.ContactEmail;
                ExistingOrg.Address1 = Organization.Address1;
                ExistingOrg.Address2 = Organization.Address2;
                ExistingOrg.City = Organization.City;
                ExistingOrg.State = Organization.State;
                ExistingOrg.PostalCode = Organization.PostalCode;
                ExistingOrg.Country = Organization.Country;
                ExistingOrg.Remarks = Organization.Remarks;
                ExistingOrg.IsActive = true;
                ExistingOrg.UpdatedBy = Organization.UpdatedBy;
                ExistingOrg.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<Organization>().Update(ExistingOrg);
                _uow.Save();
            }
            else
            {
                return "";
            }

            return Convert.ToString(ExistingOrg.Id);
        }

        public IEnumerable<Object> GetAllOrganization()
        {
            string fileUploadPathView = _configuration["AppSettings:FileUploadPathView"] ?? string.Empty;

            IEnumerable<Object> result = null;

            result = (from o in _uow.GenericRepository<Organization>().Table()
                      join oc in _uow.GenericRepository<OrganizationConfig>().Table().Where(x => x.IsActive == true && x.IsDeleted == false).AsNoTracking() on o.Id equals oc.OrgId into ocGroup
                      from oc in ocGroup.DefaultIfEmpty()

                      where o.IsDeleted == false
                      select new
                      {
                          o.Id,
                          o.Code,
                          o.Name,
                          o.Website,
                          o.Email,
                          o.Phone,
                          o.IsActive,
                          Image = fileUploadPathView + "Organization/" + oc.Image
                      })
                         .ToList();

            return result;
        }

        public Organization GetById(int Id)
        {
            var result = _uow.GenericRepository<Organization>()
                       .Table()
                       .Where(x => x.Id == Id && x.IsActive == true && x.IsDeleted == false)
                       .FirstOrDefault();

            return result;
        }

        public string Delete(int Id)
        {
            var result = _uow.GenericRepository<Organization>().Table().Where(x => x.Id == Id).FirstOrDefault();
            if (result != null)
            {
                result.IsDeleted = true;
                _uow.GenericRepository<Organization>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }


        public string ActiveInActive(int Id, bool IsActive)
        {
            var result = _uow.GenericRepository<Organization>().Table().Where(x => x.Id == Id).FirstOrDefault();
            if (result != null)
            {
                result.IsActive = IsActive;
                _uow.GenericRepository<Organization>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }

        public string CreateUpdateOrganizationConfig(OrganizationConfig model)
        {
            var repo = _uow.GenericRepository<OrganizationConfig>();

            var existing = repo.Table()
                .FirstOrDefault(o => o.OrgId == model.OrgId && o.IsDeleted == false);

            if (existing != null)
            {
                existing.Image = model.Image;
                existing.ThemeColor = model.ThemeColor;
                existing.FontSize = model.FontSize;
                existing.IsActive = true;
                existing.UpdatedBy = model.UpdatedBy;
                existing.UpdatedDate = DateTime.Now;

                repo.Update(existing);
                _uow.Save();

                return Convert.ToString(existing.Id);
            }
            else
            {
                var entity = new OrganizationConfig
                {
                    OrgId = model.OrgId,
                    Image = model.Image,
                    ThemeColor = model.ThemeColor,
                    FontSize = model.FontSize,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = model.CreatedBy,
                    CreatedDate = DateTime.Now
                };

                repo.Insert(entity);
                _uow.Save();

                return Convert.ToString(entity.Id);
            }
        }

        public OrganizationConfig GetOrganizationConfigByOrgId(int OrgId)
        {
            string fileUploadPathView = _configuration["AppSettings:FileUploadPathView"] ?? string.Empty;

            var result = _uow.GenericRepository<OrganizationConfig>()
                       .Table()
                       .Where(x => x.OrgId == OrgId && x.IsActive == true && x.IsDeleted == false)
                       .FirstOrDefault();

            if (result != null && !string.IsNullOrWhiteSpace(result.Image))
            {
                result.Image = fileUploadPathView + "Organization/" + result.Image;
            }

            return result;
        }

    }
}
