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
    public class UserRoleMappingService : IUserRoleMappingService
    {

        private readonly IUnitOfWork _uow;
        public UserRoleMappingService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public string Create(List<UserRoleMapping> userRoleMapping)
        {
            if (userRoleMapping == null || userRoleMapping.Count == 0)
            {
                return string.Empty;
            }

            _uow.GenericRepository<UserRoleMapping>().InsertRange(userRoleMapping);
            _uow.Save();

            return "Success";
        }

        public string Update(List<UserRoleMapping> userRoleMapping)
        {
            if (userRoleMapping == null || userRoleMapping.Count == 0)
            {
                return string.Empty;
            }

            var userId = userRoleMapping.First().UserId;
            var updatedBy = userRoleMapping.First().UpdatedBy;

            var existingMappings = _uow.GenericRepository<UserRoleMapping>()
                .Table()
                .Where(x => x.UserId == userId && x.IsDeleted == false)
                .ToList();

            foreach (var existingMapping in existingMappings)
            {
                existingMapping.IsDeleted = true;
                existingMapping.UpdatedBy = updatedBy;
                existingMapping.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<UserRoleMapping>().Update(existingMapping);
            }

            _uow.Save();

            foreach (var mapping in userRoleMapping)
            {
                mapping.IsDeleted = false;
                mapping.IsActive = true;
                mapping.UpdatedBy = updatedBy;
                mapping.UpdatedDate = DateTime.Now;

                var existingRecord = _uow.GenericRepository<UserRoleMapping>()
                    .Table()
                    .FirstOrDefault(x => x.Id == mapping.Id && x.UserId == userId);

                if (existingRecord != null)
                {
                    existingRecord.RoleId = mapping.RoleId;
                    existingRecord.IsDeleted = false;
                    existingRecord.IsActive = true;
                    existingRecord.UpdatedBy = mapping.UpdatedBy;
                    existingRecord.UpdatedDate = DateTime.Now;

                    _uow.GenericRepository<UserRoleMapping>().Update(existingRecord);
                }
                else
                {
                    _uow.GenericRepository<UserRoleMapping>().Insert(mapping);
                }
            }

            _uow.Save();

            return "Success";
        }

    }
}
