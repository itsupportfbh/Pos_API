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


        public List<Organization> GetAllOrganization()
        {
            return _uow.GenericRepository<Organization>()
                
                       .Table()
                       .Where(x=>x.IsActive==true)
                       .OrderBy(x => x.Id)
                       .ToList();

        }
        public List<OrganizationDTO> GetAllOrganizationDTO()
        {
            var result = (from a in _uow.GenericRepository<Organization>().Table().AsNoTracking()
                          select new
                          {
                             a

                          }).AsEnumerable().Select(xx => new OrganizationDTO
                          {
                              Id = xx.a.Id,
                              Code = xx.a.Code,
                              Name = xx.a.Name,
                              GSTNo = xx.a.GSTNo,
                              RegistrationNo = xx.a.RegistrationNo,
                              Phone = xx.a.Phone,
                              Email = xx.a.Email,
                              Website = xx.a.Website,
                              ContactPerson = xx. a.ContactPerson,
                              ContactMobileNo = xx.a.ContactMobileNo,
                              ContactEmail = xx. a.ContactEmail,
                              Address1 = xx. a.Address1,
                              Address2 = xx.a.Address2,
                              City = xx. a.City,
                              State = xx.a.State,
                              PostalCode = xx.a.PostalCode,
                              Country = xx. a.Country,
                              Image = xx.a.Image,
                              ThemeColor = xx.a.ThemeColor,
                              Remarks = xx.a.Remarks,
                              IsActive = xx. a.IsActive

                          }).OrderBy(x => x.Id).ToList();
            return result;


        }
       
        public Organization GetOrganizationById(int Id)
        {
            var result = _uow.GenericRepository<Organization>()
                       .Table()
                       .Where(x => x.Id == Id&& x.IsActive==true)
                       .OrderBy(x => x.Id)
                       .FirstOrDefault();
            return result;
        }

        public Organization AddUpdateOrganization(OrganizationDTO organizationDTO)
        {
            var result = _uow.GenericRepository<Organization>().Table().Where(x => x.Id == organizationDTO.Id && x.IsActive==true).FirstOrDefault();

            if (result == null)
            {
                result = new Organization();

                result.Id = organizationDTO.Id;
                result.Code = organizationDTO.Code;
                result.Name = organizationDTO.Name;

                result.GSTNo = organizationDTO.GSTNo;
                result.RegistrationNo = organizationDTO.RegistrationNo;

                result.Phone = organizationDTO.Phone;
                result.Email = organizationDTO.Email;
                result.Website = organizationDTO.Website;

                result.ContactPerson = organizationDTO.ContactPerson;
                result.ContactMobileNo = organizationDTO.ContactMobileNo;
                result.ContactEmail = organizationDTO.ContactEmail;

                result.Address1 = organizationDTO.Address1;
                result.Address2 = organizationDTO.Address2;

                result.City = organizationDTO.City;
                result.State = organizationDTO.State;
                result.PostalCode = organizationDTO.PostalCode;
                result.Country = organizationDTO.Country;

                result.Image = organizationDTO.Image;
                result.ThemeColor = organizationDTO.ThemeColor;
                result.Remarks = organizationDTO.Remarks;

                result.IsActive = organizationDTO.IsActive;

                result.CreatedBy = 1; // pass logged-in user
                result.CreatedDate = DateTime.Now;

                result.IsDeleted = false;

                _uow.GenericRepository<Organization>().Insert(result);


            }
            else
            {

                result.Code = organizationDTO.Code;
                result.Name = organizationDTO.Name;

                result.GSTNo = organizationDTO.GSTNo;
                result.RegistrationNo = organizationDTO.RegistrationNo;

                result.Phone = organizationDTO.Phone;
                result.Email = organizationDTO.Email;
                result.Website = organizationDTO.Website;

                result.ContactPerson = organizationDTO.ContactPerson;
                result.ContactMobileNo = organizationDTO.ContactMobileNo;
                result.ContactEmail = organizationDTO.ContactEmail;

                result.Address1 = organizationDTO.Address1;
                result.Address2 = organizationDTO.Address2;

                result.City = organizationDTO.City;
                result.State = organizationDTO.State;
                result.PostalCode = organizationDTO.PostalCode;
                result.Country = organizationDTO.Country;

                result.Image = organizationDTO.Image;
                result.ThemeColor = organizationDTO.ThemeColor;
                result.Remarks = organizationDTO.Remarks;

                result.IsActive = organizationDTO.IsActive;

                result.CreatedBy = 1; // pass logged-in user
                result.CreatedDate = DateTime.Now;

                result.IsDeleted = false;
                _uow.GenericRepository<Organization>().Update(result);

            }
            _uow.Save();
            return result;

        }


        public Organization DeleteOrganizationById(int id)
        {
            var result = _uow.GenericRepository<Organization>().Table().Where(x => x.Id== id).FirstOrDefault();
            if (result != null)
            {
                result.IsActive = false;
                result.IsDeleted = true;
                _uow.GenericRepository<Organization>().Update(result);
                _uow.Save();
            }

            return new Organization();


        }





    }
}
