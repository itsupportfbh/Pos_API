using Microsoft.EntityFrameworkCore;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.DAL.Services
{
    public class OrganizationService : IOrganizationservice
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
                CreatedDate = DateTime.UtcNow
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

            var entity = new Organization
            {
                Id = organizationDTO.Id,
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
                CreatedDate = DateTime.UtcNow
            };

            _uow.GenericRepository<Organization>().Update(entity);
            _uow.Save();

            return Convert.ToString(entity.Id);
        }

        public IEnumerable<Object> GetAllOrganization()
        {
            IEnumerable<Object> result = null;

            result = (from o in _uow.GenericRepository<Organization>().Table()
                      where o.IsActive == true && o.IsDeleted == false
                      select new
                      {
                          Id = o.Id,
                          Code = o.Code,
                          Name = o.Name,
                          IsActive = o.IsActive,
                      })
                         .ToList();

            return result;
        }

        public Organization GetById(int Id)
        {
            var result = _uow.GenericRepository<Organization>()
                       .Table()
                       .Where(x => x.Id == Id && x.IsActive == true && x.IsDeleted == false)
                       .OrderBy(x => x.Id)
                       .FirstOrDefault();

            return result;
        }

        public string DeleteById(int Id)
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
