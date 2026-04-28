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

        public string Create(Organization Organization)
        {
            int check = _uow.GenericRepository<Organization>().Table()
                .Count(o => o.Name.ToLower() == Organization.Name.ToLower()
                         && o.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var entity = new Organization
            {
                Code = Organization.Code,
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
                CreatedBy = Organization.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<Organization>().Insert(entity);
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
