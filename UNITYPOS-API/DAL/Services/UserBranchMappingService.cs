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
    public class UserBranchMappingService : IUserBranchMappingService
    {

        private readonly IUnitOfWork _uow;
        public UserBranchMappingService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public string Create(List<UserBranchMapping> userBranchMapping)
        {
            if (userBranchMapping == null || userBranchMapping.Count == 0)
            {
                return string.Empty;
            }

            _uow.GenericRepository<UserBranchMapping>().InsertRange(userBranchMapping);
            _uow.Save();

            return "Success";

        }

        public string Update(List<UserBranchMapping> userBranchMapping)
        {
            if (userBranchMapping == null || userBranchMapping.Count == 0)
            {
                return string.Empty;
            }

            var userId = userBranchMapping.First().UserId;
            var updatedBy = userBranchMapping.First().UpdatedBy;

            var existingMappings = _uow.GenericRepository<UserBranchMapping>()
                .Table()
                .Where(x => x.UserId == userId && x.IsDeleted == false)
                .ToList();

            foreach (var existingMapping in existingMappings)
            {
                existingMapping.IsDeleted = true;
                existingMapping.UpdatedBy = updatedBy;
                existingMapping.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<UserBranchMapping>().Update(existingMapping);
            }

            _uow.Save();

            foreach (var mapping in userBranchMapping)
            {
                mapping.IsDeleted = false;
                mapping.IsActive = true;
                mapping.UpdatedBy = updatedBy;
                mapping.UpdatedDate = DateTime.Now;

                var existingRecord = _uow.GenericRepository<UserBranchMapping>()
                    .Table()
                    .FirstOrDefault(x => x.BranchId == mapping.BranchId && x.UserId == userId);

                if (existingRecord != null)
                {
                    existingRecord.BranchId = mapping.BranchId;
                    existingRecord.IsDeleted = false;
                    existingRecord.IsActive = true;
                    existingRecord.UpdatedBy = mapping.UpdatedBy;
                    existingRecord.UpdatedDate = DateTime.Now;

                    _uow.GenericRepository<UserBranchMapping>().Update(existingRecord);
                }
                else
                {
                    _uow.GenericRepository<UserBranchMapping>().Insert(mapping);
                }
            }

            _uow.Save();

            return "Success";
        }

        public IEnumerable<Object> GetByUserId(int UserId)
        {
            IEnumerable<Object> result = null;

            result = (from u in _uow.GenericRepository<UserBranchMapping>().Table()
                      where u.IsDeleted == false && u.UserId == UserId
                      select new
                      {
                          u.Id,
                          u.UserId,
                          u.BranchId,
                          u.IsActive,
                          u.IsDeleted,
                          u.CreatedBy,
                          u.CreatedDate,
                      })
                         .ToList();

            return result;
        }

    }
}
