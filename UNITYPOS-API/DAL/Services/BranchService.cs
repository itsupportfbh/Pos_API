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
                .Count(o => o.Name.ToLower() == branch.Name.ToLower() && o.Id != branch.Id
       && o.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var ExistingBranch = _uow.GenericRepository<Branch>().Table().Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == branch.Id && x.OrgId==branch.OrgId).FirstOrDefault();

            if (ExistingBranch != null)
            {
                ExistingBranch.Code = branch.Code;
                ExistingBranch.Name = branch.Name;
                ExistingBranch.Phone = branch.Phone;
                ExistingBranch.Email = branch.Email;
                ExistingBranch.ContactPerson = branch.ContactPerson;
                ExistingBranch.ContactMobileNo = branch.ContactMobileNo;
                ExistingBranch.ContactEmail = branch.ContactEmail;
                ExistingBranch.Address1 = branch.Address1;
                ExistingBranch.Address2 = branch.Address2;
                ExistingBranch.City = branch.City;
                ExistingBranch.State = branch.State;
                ExistingBranch.PostalCode = branch.PostalCode;
                ExistingBranch.Country = branch.Country;
                ExistingBranch.Remarks = branch.Remarks;
                ExistingBranch.OrgId = branch.OrgId;
                ExistingBranch.IsActive = true;
                ExistingBranch.IsDeleted = false;

                ExistingBranch.UpdatedBy = branch.UpdatedBy;
                ExistingBranch.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<Branch>().Update(ExistingBranch);
                _uow.Save();

                return Convert.ToString(ExistingBranch.Id);
            }
            else
            {
                return "0";
            }
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
