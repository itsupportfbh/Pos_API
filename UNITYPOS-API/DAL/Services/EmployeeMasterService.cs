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
                       && (branchId == "0" || (e.BranchId.HasValue && BranchIds.Contains(e.BranchId.Value)))

                 select new
                 {
                     Id = e.Id,
                     OrganizationName = o.Name,
                     Code = e.Code,
                     Name = e.Name,
                     MobileNo = e.MobileNo,
                     EmailId = e.EmailId,
                     DesignationId = e.DesignationId ?? 0,
                     DepartmentId = e.DepartmentId ?? 0,
                     DateOfJoining = e.DateOfJoining,       
                     BranchId = e.BranchId ?? 0,
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
            var employeeCode = employeeMaster.Code?.Trim() ?? string.Empty;
            var employeeName = employeeMaster.Name?.Trim() ?? string.Empty;

            int codeCheck = _uow.GenericRepository<EmployeeMaster>().Table()
                .Count(c => c.Code.Trim().ToLower() == employeeCode.ToLower());

            if (codeCheck > 0)
            {
                return "AlreadyExists";
            }

            int check = _uow.GenericRepository<EmployeeMaster>().Table()
                .Count(c => c.Name.Trim().ToLower() == employeeName.ToLower()
                         && c.OrgId == employeeMaster.OrgId
                         && c.BranchId == employeeMaster.BranchId
                         && c.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var entity = new EmployeeMaster
            {
                Code = employeeCode,
                Name = employeeName,
                MobileNo = employeeMaster.MobileNo,
                EmailId = employeeMaster.EmailId,
                DesignationId = employeeMaster.DesignationId,
                DepartmentId = employeeMaster.DepartmentId,
                DateOfJoining = employeeMaster.DateOfJoining,
                BranchId = employeeMaster.BranchId,
                Gender = employeeMaster.Gender,
                AddressLine1 = employeeMaster.AddressLine1,
                IdProofNo = employeeMaster.IdProofNo,
                Remarks = employeeMaster.Remarks,
                OrgId = employeeMaster.OrgId,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 1,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<EmployeeMaster>().Insert(entity);
            _uow.Save();

            return Convert.ToString(entity.Id);
        }


        public string Update(EmployeeMaster employeeMaster)
        {
            var employeeCode = employeeMaster.Code?.Trim() ?? string.Empty;
            var employeeName = employeeMaster.Name?.Trim() ?? string.Empty;

            int codeCheck = _uow.GenericRepository<EmployeeMaster>().Table()
                .Count(x => x.Code.Trim().ToLower() == employeeCode.ToLower()
                         && x.Id != employeeMaster.Id);

            if (codeCheck > 0)
            {
                return "AlreadyExists";
            }

            int check = _uow.GenericRepository<EmployeeMaster>().Table()
                .Count(x => x.Name.Trim().ToLower() == employeeName.ToLower() &&
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
                existingEmployee.Code = employeeCode;
                existingEmployee.Name = employeeName;
                existingEmployee.MobileNo = employeeMaster.MobileNo;
                existingEmployee.EmailId = employeeMaster.EmailId;
                existingEmployee.DesignationId = employeeMaster.DesignationId;
                existingEmployee.DepartmentId = employeeMaster.DepartmentId;
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
    


