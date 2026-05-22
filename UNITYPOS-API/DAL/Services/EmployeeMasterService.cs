using Azure.Core;
using UNITYPOS_API.Controllers;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.Entities;

namespace UNITYPOS_API.DAL.Services
{
    public class EmployeeMasterService : IEmployeeMasterService
    {
        private readonly IUnitOfWork _uow;

        public EmployeeMasterService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public IEnumerable<object> GetAllEmployee(int orgId, string branchId)
        {
            var BranchIds = branchId.Split(',').Select(int.Parse).ToList();

            var result =
                (from e in _uow.GenericRepository<EmployeeMaster>().Table()

                 join b in _uow.GenericRepository<Branch>().Table()
                    on e.BranchId equals b.Id

                 join o in _uow.GenericRepository<Organization>().Table()
                    on e.OrgId equals o.Id

                 where e.IsDeleted == false
                       && (orgId == 0 || e.OrgId == orgId)
                       && (BranchIds.Contains((int)e.BranchId) || branchId == "0")

                 select new
                 {
                     Id = e.Id,
                     OrganizationName = o.Name,
                     Code = e.Code,
                     Name = e.Name,
                     MobileNo = e.MobileNo,
                     EmailId = e.EmailId,
                     Designation = e.Designation,
                     Department = e.Department,
                     DateOfJoining = e.DateOfJoining,
                     BranchId = e.BranchId,
                     BranchName = b.Name,
                     Gender = e.Gender,
                     AddressLine1 = e.AddressLine1,
                     IdProofNo = e.IdProofNo,
                     Remarks = e.Remarks,
                     OrgId = e.OrgId,
                     IsActive = e.IsActive,
                     IsDeleted = e.IsDeleted
                 }).ToList();

            return result;
        }

        public EmployeeMaster GetEmployeebyId(int Id)
        {
            var result = _uow.GenericRepository<EmployeeMaster>()
                      .Table()
                      .Where(x => x.Id == Id
                               && x.IsActive == true
                               && x.IsDeleted == false)
                      .FirstOrDefault();

            return result;
        }



        public string Create(EmployeeMaster employeeMaster)
        {
            int check = _uow.GenericRepository<EmployeeMaster>().Table()
                .Count(c => c.Name.ToLower() == employeeMaster.Name.ToLower()
                         && c.OrgId == employeeMaster.OrgId
                         && c.BranchId == employeeMaster.BranchId
                         && c.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var entity = new EmployeeMaster
            {
                Code = employeeMaster.Code,
                Name = employeeMaster.Name,
                MobileNo = employeeMaster.MobileNo,
                EmailId = employeeMaster.EmailId,
                Designation = employeeMaster.Designation,
                Department = employeeMaster.Department,
                DateOfJoining = employeeMaster.DateOfJoining,
                BranchId = employeeMaster.BranchId,
                Gender = employeeMaster.Gender,
                AddressLine1 = employeeMaster.AddressLine1,
                IdProofNo = employeeMaster.IdProofNo,
                Remarks = employeeMaster.Remarks,
                OrgId = employeeMaster.OrgId,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = employeeMaster.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<EmployeeMaster>().Insert(entity);
            _uow.Save();

            return Convert.ToString(entity.Id);
        }


        public string Update(EmployeeMaster employeeMaster)
        {
            int check = _uow.GenericRepository<EmployeeMaster>().Table()
                .Count(x => x.Name.Trim().ToLower() == employeeMaster.Name.Trim().ToLower() &&
                    x.Id != employeeMaster.Id &&
                    x.OrgId == employeeMaster.OrgId &&
                    x.BranchId == employeeMaster.BranchId &&
                    x.IsDeleted == false);
                     

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var existingEmployee = _uow.GenericRepository<EmployeeMaster>().Table()
                .FirstOrDefault(x =>
                    x.Id == employeeMaster.Id &&
                    x.IsDeleted == false);

            if (existingEmployee != null)
            {
                existingEmployee.Code = employeeMaster.Code;
                existingEmployee.Name = employeeMaster.Name;
                existingEmployee.MobileNo = employeeMaster.MobileNo;
                existingEmployee.EmailId = employeeMaster.EmailId;
                existingEmployee.Designation = employeeMaster.Designation;
                existingEmployee.Department = employeeMaster.Department;
                existingEmployee.DateOfJoining = employeeMaster.DateOfJoining;
                existingEmployee.BranchId = employeeMaster.BranchId;
                existingEmployee.Gender = employeeMaster.Gender;
                existingEmployee.AddressLine1 = employeeMaster.AddressLine1;
                existingEmployee.IdProofNo = employeeMaster.IdProofNo;
                existingEmployee.Remarks = employeeMaster.Remarks;

                existingEmployee.UpdatedBy = employeeMaster.UpdatedBy;
                existingEmployee.UpdatedDate = DateTime.Now;

                _uow.Save();
            }
            else
            {
                return "";
            }

            return Convert.ToString(existingEmployee.Id);
        }


        public string DeleteById(int id)
        {
            var result = _uow.GenericRepository<EmployeeMaster>()
                .Table()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (result != null)
            {
                result.IsDeleted = true;

                _uow.GenericRepository<EmployeeMaster>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }

        public string ActiveInActive(int id, bool isActive)
        {
            var result = _uow.GenericRepository<EmployeeMaster>()
                .Table()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (result != null)
            {
                result.IsActive = isActive;

                _uow.GenericRepository<EmployeeMaster>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }

        public object Create(EmployeeController employee)
        {
            throw new NotImplementedException();
        }

        public object Update(EmployeeController employee)
        {
            throw new NotImplementedException();
        }

       
    }
}
    


