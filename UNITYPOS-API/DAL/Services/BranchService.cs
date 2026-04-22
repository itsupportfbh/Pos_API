using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Security.Cryptography;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.ViewModel;

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
                     where b.IsDeleted==false &&b.OrgId==orgid
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
                      where  b.IsDeleted == false && b.Id==id
                      select new
                      {
                          id = b.Id,
                          name = b.Name,
                          code = b.Code,
                          isactive = b.IsActive,
                          Phone=b.Phone,
                          Email=b.Email,
                          ContactPerson = b.ContactPerson,
                          ContactMobileNo = b.ContactMobileNo,
                          ContactEmail = b.ContactEmail,
                          Address1 = b.Address1,
                          Address2 = b.Address2,
                          City = b.City,
                          State = b.State,
                          PostalCode = b.PostalCode,
                          Country = b.Country,
                          Remarks = b.Remarks,
                          OrgId = b.OrgId,
                          IsActive = b.IsActive,
                          IsDeleted = b.IsDeleted,
                          CreatedBy = b.CreatedBy,
                          CreatedDate = b.CreatedDate,
                          UpdatedBy = b.UpdatedBy,
                          UpdatedDate = b.UpdatedDate

                      }).ToList();


            return result;
        }


        public string Create(Branch branch)
        {
            int check = _uow.GenericRepository<Branch>().Table()
                .Count(b => b.Name.Trim().ToLower() == branch.Name.Trim().ToLower()
                         && b.OrgId == branch.OrgId
                         && b.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

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

        public string Update(Branch branch)
        {
            int check = _uow.GenericRepository<Branch>().Table()
                .Count(b => (b.Name.Trim().ToLower() == branch.Name.Trim().ToLower()
                          || b.Code.Trim().ToLower() == branch.Code.Trim().ToLower())
                         && b.Id != branch.Id
                         && b.OrgId == branch.OrgId
                         && b.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var existingBranch = _uow.GenericRepository<Branch>().Table()
                .FirstOrDefault(x => x.Id == branch.Id
                                  && x.OrgId == branch.OrgId
                                  && x.IsDeleted == false);

            if (existingBranch != null)
            {
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
                existingBranch.IsActive = true;
                existingBranch.IsDeleted = false;
                existingBranch.UpdatedBy = branch.UpdatedBy;
                existingBranch.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<Branch>().Update(existingBranch);
                _uow.Save();

                return Convert.ToString(existingBranch.Id);
            }

            return "0";
        }
        public string DeleteById(int id)
        {
            var result = _uow.GenericRepository<Branch>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {
               
                result.IsDeleted = true;
                _uow.GenericRepository<Branch>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);


        }

        public string ActiveInActive(int id,bool isActive)
        {
            var result = _uow.GenericRepository<Branch>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {
                
                result.IsActive = isActive;
                _uow.GenericRepository<Branch>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);


        }

    }
}
