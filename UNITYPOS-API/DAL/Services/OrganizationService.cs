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
        public OrganizationService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public string Create(OrganizationDTO organizationDTO)
        {
            int check = _uow.GenericRepository<Organization>().Table()
                .Count(o => o.Name.ToLower() == organizationDTO.Name.ToLower()
                         && o.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var entity = new Organization
            {
                Code = organizationDTO.Code,
                Name = organizationDTO.Name,
                GSTNo = organizationDTO.GSTNo,
                RegistrationNo = organizationDTO.RegistrationNo,
                Phone = organizationDTO.Phone,
                Email = organizationDTO.Email,
                Website = organizationDTO.Website,
                ContactPerson = organizationDTO.ContactPerson,
                ContactMobileNo = organizationDTO.ContactMobileNo,
                ContactEmail = organizationDTO.ContactEmail,
                Address1 = organizationDTO.Address1,
                Address2 = organizationDTO.Address2,
                City = organizationDTO.City,
                State = organizationDTO.State,
                PostalCode = organizationDTO.PostalCode,
                Country = organizationDTO.Country,
                Remarks = organizationDTO.Remarks,
                IsActive = true,
                CreatedBy = organizationDTO.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<Organization>().Insert(entity);
            _uow.Save();

            return Convert.ToString(entity.Id);
        }

        public string Update(OrganizationDTO organizationDTO)
        {
            int check = _uow.GenericRepository<Organization>().Table()
                .Count(o => o.Name.ToLower() == organizationDTO.Name.ToLower() && o.Id != organizationDTO.Id
                         && o.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var ExistingOrg = _uow.GenericRepository<Organization>().Table().Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == organizationDTO.Id).FirstOrDefault();

            if (ExistingOrg != null)
            {
                ExistingOrg.Code = organizationDTO.Code;
                ExistingOrg.Name = organizationDTO.Name;
                ExistingOrg.GSTNo = organizationDTO.GSTNo;
                ExistingOrg.RegistrationNo = organizationDTO.RegistrationNo;
                ExistingOrg.Phone = organizationDTO.Phone;
                ExistingOrg.Email = organizationDTO.Email;
                ExistingOrg.Website = organizationDTO.Website;
                ExistingOrg.ContactPerson = organizationDTO.ContactPerson;
                ExistingOrg.ContactMobileNo = organizationDTO.ContactMobileNo;
                ExistingOrg.ContactEmail = organizationDTO.ContactEmail;
                ExistingOrg.Address1 = organizationDTO.Address1;
                ExistingOrg.Address2 = organizationDTO.Address2;
                ExistingOrg.City = organizationDTO.City;
                ExistingOrg.State = organizationDTO.State;
                ExistingOrg.PostalCode = organizationDTO.PostalCode;
                ExistingOrg.Country = organizationDTO.Country;
                ExistingOrg.Remarks = organizationDTO.Remarks;
                ExistingOrg.IsActive = true;
                ExistingOrg.UpdatedBy = organizationDTO.UpdatedBy;
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
            IEnumerable<Object> result = null;

            result = (from o in _uow.GenericRepository<Organization>().Table()
                      where o.IsDeleted == false
                      select new
                      {
                          o.Id,
                          o.Code,
                          o.Name,
                          o.Website,
                          o.IsActive,
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

    }
}
