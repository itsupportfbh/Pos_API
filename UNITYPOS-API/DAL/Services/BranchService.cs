using System.Security.Cryptography;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities;

namespace UNITYPOS_API.DAL.Services
{
    public class BranchService:IBranchService
    {

        private readonly IUnitOfWork _uow;
        public BranchService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public IEnumerable<Object> GetAllBranch( int orgid)
        {
            IEnumerable<Object> result = null;

            result=( from b in _uow.GenericRepository<Branch>().Table()
                     where b.IsActive== true&&b.IsDeleted==false &&b.OrgId==orgid
                     select new
                     {
                         id = b.Id,
                         name=b.Name,
                         code=b.Code,
                         isactive=b.IsActive,
                     }).ToList();


            return result;
        }
        public IEnumerable<Object> GetBranchbyId(int id)
        {
            IEnumerable<Object> result = null;

            result = (from b in _uow.GenericRepository<Branch>().Table()
                      where b.IsActive == true && b.IsDeleted == false && b.Id==id
                      select new
                      {
                          id = b.Id,
                          name = b.Name,
                          code = b.Code,
                          isactive = b.IsActive,
                      }).ToList();


            return result;
        }
         


        public string Create(Branch branch)
        {
            if (branch == null)
                throw new ArgumentNullException(nameof(branch));

            var existingBranch = _uow.GenericRepository<Branch>().Table()
                .FirstOrDefault(x =>
                    x.OrgId == branch.OrgId &&
                    x.IsDeleted == false &&
                    (x.Code.Trim().ToLower() == branch.Code.Trim().ToLower()
                     || x.Name.Trim().ToLower() == branch.Name.Trim().ToLower()));

            if (existingBranch != null)
              return "AlreadyExists ";

            var entity = new Branch
            {

                Code = branch.Code,
                Name = branch.Name,
                Phone = branch.Phone,
                Email = branch.Email,
                ContactPerson = branch.ContactPerson,
                ContactMobileNo = branch.ContactMobileNo,
                ContactEmail = branch.ContactEmail,
                Address1 = branch.Address1,
                Address2 = branch.Address2,
                City = branch.City,
                State = branch.State,
                PostalCode = branch.PostalCode,
                Country = branch.Country,
                Remarks = branch.Remarks,

                OrgId = branch.OrgId,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = branch.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<Branch>().Insert(entity);
            _uow.Save();

            return Convert.ToString(entity.Id);
        }


        public Branch Update(Branch branch)
        {
            if (branch == null)
                throw new ArgumentNullException(nameof(branch));

            var existingBranch = _uow.GenericRepository<Branch>().Table()
                .FirstOrDefault(x => x.Id == branch.Id && x.IsDeleted == false);

            if (existingBranch == null)
                throw new Exception("Branch not found.");

            var duplicateBranch = _uow.GenericRepository<Branch>().Table()
                .FirstOrDefault(x =>
                    x.Id != branch.Id &&
                    x.OrgId == branch.OrgId &&
                    x.IsDeleted == false &&
                    (x.Code.Trim().ToLower() == branch.Code.Trim().ToLower() ||
                     x.Name.Trim().ToLower() == branch.Name.Trim().ToLower()));

            if (duplicateBranch != null)
                throw new Exception("Branch code or name already exists for this organization.");

            existingBranch.Code = branch.Code;
            existingBranch.Name = branch.Name;
            existingBranch.Phone = branch.Phone;
            existingBranch.Email = branch.Email;
            existingBranch.ContactPerson = branch.ContactPerson;
            existingBranch.ContactMobileNo = branch.ContactMobileNo;
            existingBranch.ContactEmail = branch.ContactEmail;
            existingBranch.Address1 = branch.Address1;
            existingBranch.Address2 = branch.Address2;
            existingBranch.City = branch.City;
            existingBranch.State = branch.State;
            existingBranch.PostalCode = branch.PostalCode;
            existingBranch.Country = branch.Country;
            existingBranch.Remarks = branch.Remarks;
            existingBranch.OrgId = branch.OrgId;

            existingBranch.UpdatedBy = branch.UpdatedBy;
            existingBranch.UpdatedDate = DateTime.Now;

            _uow.GenericRepository<Branch>().Update(existingBranch);
            _uow.Save();

            return existingBranch;
        }

        public Branch DeleteById(int id)
        {
            var result = _uow.GenericRepository<Branch>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {
               
                result.IsDeleted = true;
                _uow.GenericRepository<Branch>().Update(result);
                _uow.Save();
            }

            return new Branch();


        }

        public Branch ActiveInActive(int id,bool isActive)
        {
            var result = _uow.GenericRepository<Branch>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {
                
                result.IsActive = isActive;
                _uow.GenericRepository<Branch>().Update(result);
                _uow.Save();
            }

            return new Branch();


        }

    }
}
