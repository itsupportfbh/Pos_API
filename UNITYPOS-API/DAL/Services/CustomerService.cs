using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class CustomerService: ICustomerService
    {



        private readonly IUnitOfWork _uow;
        public CustomerService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }


        public IEnumerable<object> GetAllCustomer(int orgid)
        {
            var result =
                (from b in _uow.GenericRepository<CustomerMaster>().Table()
                 where b.IsDeleted == false
                       && b.OrgId == orgid
                 select new
                 {
                     id = b.Id,
                     code = b.Code,
                     name = b.Name,
                     mobileNo = b.MobileNo,
                     emailId = b.EmailId,
                     addressLine1 = b.AddressLine1,
                     cityId = b.CityId,
                     stateId = b.StateId,
                     countryId = b.CountryId,
                     pincode = b.Pincode,
                     dateOfBirth = b.DateOfBirth,
                     gender = b.Gender,
                     memberNo = b.MemberNo,
                     openingBalance = b.OpeningBalance,
                     isMember = b.IsMember,
                     remarks = b.Remarks,
                     orgId = b.OrgId,
                     isActive = b.IsActive
                 }).ToList();

            return result;
        }

        public CustomerMaster GetCustomerbyId(int Id)
        {
            var result = _uow.GenericRepository<CustomerMaster>()
                       .Table()
                       .Where(x => x.Id == Id && x.IsActive == true && x.IsDeleted == false)
                       .FirstOrDefault();

            return result;
        }

        public string Create(CustomerMaster customer)
        {
            int check = _uow.GenericRepository<CustomerMaster>().Table()
                .Count(c => c.MobileNo.Trim().ToLower() == customer.MobileNo.Trim()
                         && c.OrgId == customer.OrgId
                         && c.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var entity = new CustomerMaster
            {
                Code = customer.Code,
                Name = customer.Name,
                MobileNo = customer.MobileNo,
                EmailId = customer.EmailId,
                AddressLine1 = customer.AddressLine1,
                CityId = customer.CityId,
                StateId = customer.StateId,
                CountryId = customer.CountryId,
                Pincode = customer.Pincode,
                DateOfBirth = customer.DateOfBirth,
                Gender = customer.Gender,
                MemberNo = customer.MemberNo,
                OpeningBalance = customer.OpeningBalance,
                IsMember = customer.IsMember,
                Remarks = customer.Remarks,
                OrgId = customer.OrgId,

                IsActive = true,
                IsDeleted = false,
                CreatedBy = customer.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<CustomerMaster>().Insert(entity);
            _uow.Save();

            return Convert.ToString(entity.Id);

        }

        public string Update(CustomerMaster customer)
        {
            int check = _uow.GenericRepository<CustomerMaster>().Table()
                .Count(c => c.MobileNo.Trim().ToLower() == customer.MobileNo.Trim()
                         && c.Id != customer.Id
                         && c.OrgId == customer.OrgId
                         && c.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var existingCustomer = _uow.GenericRepository<CustomerMaster>().Table()
                .FirstOrDefault(x => x.Id == customer.Id
                                  && x.OrgId == customer.OrgId
                                  && x.IsDeleted == false);

            if (existingCustomer == null)
            {
                return "";
            }

            existingCustomer.Code = customer.Code;
            existingCustomer.Name = customer.Name;
            existingCustomer.MobileNo = customer.MobileNo;
            existingCustomer.EmailId = customer.EmailId;
            existingCustomer.AddressLine1 = customer.AddressLine1;
            existingCustomer.CityId = customer.CityId;
            existingCustomer.StateId = customer.StateId;
            existingCustomer.CountryId = customer.CountryId;
            existingCustomer.Pincode = customer.Pincode;
            existingCustomer.DateOfBirth = customer.DateOfBirth;
            existingCustomer.Gender = customer.Gender;
            existingCustomer.MemberNo = customer.MemberNo;
            existingCustomer.OpeningBalance = customer.OpeningBalance;
            existingCustomer.IsMember = customer.IsMember;
            existingCustomer.Remarks = customer.Remarks;
            existingCustomer.OrgId = customer.OrgId;

            existingCustomer.IsActive = true;
            existingCustomer.IsDeleted = false;
            existingCustomer.UpdatedBy = customer.UpdatedBy;
            existingCustomer.UpdatedDate = DateTime.Now;

            _uow.GenericRepository<CustomerMaster>().Update(existingCustomer);
            _uow.Save();

            return Convert.ToString(existingCustomer.Id);
        }

        public string DeleteById(int id)
        {
            var result = _uow.GenericRepository<CustomerMaster>().Table()
                .FirstOrDefault(x => x.Id == id);

            if (result != null)
            {
                result.IsDeleted = true;

                _uow.GenericRepository<CustomerMaster>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }

        public string ActiveInActive(int id, bool isActive)
        {
            var result = _uow.GenericRepository<CustomerMaster>().Table()
                .FirstOrDefault(x => x.Id == id);

            if (result != null)
            {
                result.IsActive = isActive;

                _uow.GenericRepository<CustomerMaster>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }
    }
}
