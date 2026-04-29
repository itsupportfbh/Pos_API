using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.ViewModel;
using static System.Net.Mime.MediaTypeNames;

namespace UNITYPOS_API.DAL.Services
{
    public class UserMasterService : IUserMasterService
    {

        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        private readonly IUserBranchMappingService _userBranchMappingService;
        private readonly IUserRoleMappingService _userRoleMappingService;

        public UserMasterService(
            IUnitOfWork uow,
            IConfiguration configuration,
            IUserBranchMappingService userBranchMappingService,
            IUserRoleMappingService userRoleMappingService)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _configuration = configuration;
            _userBranchMappingService = userBranchMappingService ?? throw new ArgumentNullException(nameof(userBranchMappingService));
            _userRoleMappingService = userRoleMappingService ?? throw new ArgumentNullException(nameof(userRoleMappingService));
        }

        public string Create(CreateUserMaster userMaster)
        {
            int checkEmail = _uow.GenericRepository<UserMaster>().Table()
                                .Count(o => o.Email.ToLower() == userMaster.Email.ToLower() && o.OrgId == userMaster.OrgId
                                 && o.IsDeleted == false);

            int checkUser = _uow.GenericRepository<UserMaster>().Table()
                            .Count(o => o.EmpCode.ToLower() == userMaster.EmpCode.ToLower() && o.OrgId == userMaster.OrgId
                             && o.IsDeleted == false);

            if (checkEmail > 0)
            {
                return "AlreadyEmail";
            }

            if (checkUser > 0)
            {
                return "AlreadyExists";
            }


            var entity = new UserMaster
            {
                Code = userMaster.Code,
                Name = userMaster.Name,
                Remarks = userMaster.Remarks,
                IsAdmin = userMaster.IsAdmin,
                Email = userMaster.Email,
                Password = userMaster.Password ?? "",
                ContactNo = userMaster.ContactNo,
                EmpCode = userMaster.EmpCode,
                OrgId = userMaster.OrgId,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = userMaster.CreatedBy,
                CreatedDate = DateTime.Now,
                Image = userMaster.Image,
                Gender = userMaster.Gender,
                DOB = userMaster.DOB,
                Age = userMaster.Age,
                Address1 = userMaster.Address1,
                Address2 = userMaster.Address2,
                Country = userMaster.Country,
                State = userMaster.State,
                City = userMaster.City,
                PostalCode = userMaster.PostalCode,
            };

            try
            {
                _uow.BeginTransaction();

                _uow.GenericRepository<UserMaster>().Insert(entity);
                _uow.Save();

                if (entity.Id.HasValue && userMaster.UserBranchMapping != null && userMaster.UserBranchMapping.Count > 0)
                {
                    foreach (var mapping in userMaster.UserBranchMapping)
                    {
                        mapping.UserId = entity.Id.Value;
                    }

                    _userBranchMappingService.Create(userMaster.UserBranchMapping);
                }

                if (entity.Id.HasValue && userMaster.UserRoleMapping != null && userMaster.UserRoleMapping.Count > 0)
                {
                    foreach (var mapping in userMaster.UserRoleMapping)
                    {
                        mapping.UserId = entity.Id.Value;
                    }

                    _userRoleMappingService.Create(userMaster.UserRoleMapping);
                }

                _uow.Commit();
            }
            catch
            {
                _uow.Rollback();
                throw;
            }

            return Convert.ToString(entity.Id);
        }

        public string Update(CreateUserMaster userMaster)
        {
            int checkEmail = _uow.GenericRepository<UserMaster>().Table()
                             .Count(o => o.Email.ToLower() == userMaster.Email.ToLower() && o.Id != userMaster.Id && o.OrgId == userMaster.OrgId
                              && o.IsDeleted == false);

            int checkUser = _uow.GenericRepository<UserMaster>().Table()
                            .Count(o => o.EmpCode.ToLower() == userMaster.EmpCode.ToLower() && o.Id != userMaster.Id && o.OrgId == userMaster.OrgId
                             && o.IsDeleted == false);

            if (checkEmail > 0)
            {
                return "AlreadyEmail";
            }

            if (checkUser > 0)
            {
                return "AlreadyExists";
            }

            var ExistingUser = _uow.GenericRepository<UserMaster>().Table().Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == userMaster.Id).FirstOrDefault();

            if (ExistingUser != null)
            {
                try
                {
                    _uow.BeginTransaction();

                    ExistingUser.Code = userMaster.Code;
                    ExistingUser.Name = userMaster.Name;
                    ExistingUser.Remarks = userMaster.Remarks;
                    ExistingUser.IsAdmin = userMaster.IsAdmin;
                    ExistingUser.Email = userMaster.Email;
                    ExistingUser.Password = userMaster.Password ?? "";
                    ExistingUser.ContactNo = userMaster.ContactNo;
                    ExistingUser.EmpCode = userMaster.EmpCode;
                    ExistingUser.OrgId = userMaster.OrgId;
                    ExistingUser.IsActive = true;
                    ExistingUser.UpdatedBy = userMaster.UpdatedBy;
                    ExistingUser.UpdatedDate = DateTime.Now;
                    ExistingUser.Image = string.IsNullOrWhiteSpace(userMaster.Image)
                        ? ExistingUser.Image
                        : userMaster.Image;
                    ExistingUser.Gender = userMaster.Gender;
                    ExistingUser.DOB = userMaster.DOB;
                    ExistingUser.Age = userMaster.Age;
                    ExistingUser.Address1 = userMaster.Address1;
                    ExistingUser.Address2 = userMaster.Address2;
                    ExistingUser.Country = userMaster.Country;
                    ExistingUser.State = userMaster.State;
                    ExistingUser.City = userMaster.City;
                    ExistingUser.PostalCode = userMaster.PostalCode;

                    _uow.GenericRepository<UserMaster>().Update(ExistingUser);
                    _uow.Save();

                    if (userMaster.UserBranchMapping != null && userMaster.UserBranchMapping.Count > 0 && ExistingUser.Id.HasValue)
                    {
                        foreach (var mapping in userMaster.UserBranchMapping)
                        {
                            mapping.UserId = ExistingUser.Id.Value;
                            mapping.UpdatedBy = userMaster.UpdatedBy;
                            mapping.UpdatedDate = DateTime.Now;
                        }

                        _userBranchMappingService.Update(userMaster.UserBranchMapping);
                    }

                    if (userMaster.UserRoleMapping != null && userMaster.UserRoleMapping.Count > 0 && ExistingUser.Id.HasValue)
                    {
                        foreach (var mapping in userMaster.UserRoleMapping)
                        {
                            mapping.UserId = ExistingUser.Id.Value;
                            mapping.UpdatedBy = userMaster.UpdatedBy;
                            mapping.UpdatedDate = DateTime.Now;
                        }

                        _userRoleMappingService.Update(userMaster.UserRoleMapping);
                    }

                    _uow.Commit();
                }
                catch
                {
                    _uow.Rollback();
                    throw;
                }
            }
            else
            {
                return "";
            }

            return Convert.ToString(ExistingUser.Id);
        }

        public IEnumerable<Object> GetAllUsers(int OrgId)
        {
            IEnumerable<Object> result = null;
            string fileUploadPathView = _configuration["AppSettings:FileUploadPathView"] ?? string.Empty;

            result = (from u in _uow.GenericRepository<UserMaster>().Table()
                      where u.IsDeleted == false && (OrgId == 0 || u.OrgId == OrgId)
                      select new
                      {
                          u.Id,
                          u.Code,
                          u.Name,
                          u.Remarks,
                          u.IsAdmin,
                          u.Email,
                          u.Password,
                          u.ContactNo,
                          u.EmpCode,
                          u.OrgId,
                          u.IsActive,
                          u.IsDeleted,
                          u.CreatedBy,
                          u.CreatedDate,
                          Image = string.IsNullOrWhiteSpace(u.Image)
                              ? u.Image
                              : fileUploadPathView + "User/" + u.Image,
                          u.Gender,
                          u.DOB,
                          u.Age,
                          u.Address1,
                          u.Address2,
                          u.Country,
                          u.State,
                          u.City,
                          u.PostalCode,
                      })
                         .ToList();

            return result;
        }

        public UserMaster GetById(int Id)
        {
            string fileUploadPathView = _configuration["AppSettings:FileUploadPathView"] ?? string.Empty;

            var result = _uow.GenericRepository<UserMaster>()
                       .Table()
                       .Where(x => x.Id == Id && x.IsActive == true && x.IsDeleted == false)
                       .FirstOrDefault();

            if (result != null && !string.IsNullOrWhiteSpace(result.Image))
            {
                result.Image = fileUploadPathView + "User/" + result.Image;
            }

            return result;
        }

        public string Delete(int Id)
        {
            var result = _uow.GenericRepository<UserMaster>().Table().Where(x => x.Id == Id).FirstOrDefault();
            if (result != null)
            {
                result.IsDeleted = true;
                _uow.GenericRepository<UserMaster>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }


        public string ActiveInActive(int Id, bool IsActive)
        {
            var result = _uow.GenericRepository<UserMaster>().Table().Where(x => x.Id == Id).FirstOrDefault();
            if (result != null)
            {
                result.IsActive = IsActive;
                _uow.GenericRepository<UserMaster>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }

    }
}
