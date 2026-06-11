using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class DualDisplayService : IDualDisplayService
    {
        private readonly IUnitOfWork _uow;

        public DualDisplayService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public IEnumerable<object> GetAll(int orgId)
        {
            var result =
                (from profile in _uow.GenericRepository<DualDisplayProfile>().Table()
                 join branch in _uow.GenericRepository<Branch>().Table()
                    on profile.BranchId equals branch.Id
                 join counter in _uow.GenericRepository<Counter>().Table()
                    on profile.CounterId equals counter.Id
                 join organization in _uow.GenericRepository<Organization>().Table()
                    on profile.OrgId equals organization.Id
                 where profile.IsDeleted == false
                       && (orgId == 0 || profile.OrgId == orgId)
                 orderby profile.Id descending
                 select new
                 {
                     Id = profile.Id,
                     ProfileCode = profile.ProfileCode,
                     ProfileName = profile.ProfileName,
                     OrgId = profile.OrgId,
                     OrgName = organization.Name,
                     BranchId = profile.BranchId,
                     BranchName = branch.Name,
                     CounterId = profile.CounterId,
                     CounterName = counter.Name,
                     ThemeName = profile.ThemeName,
                     HeaderTitle = profile.HeaderTitle,
                     WelcomeMessage = profile.WelcomeMessage,
                     IdleMessage = profile.IdleMessage,
                     IsActive = profile.IsActive,
                     IsDeleted = profile.IsDeleted,
                     CreatedBy = profile.CreatedBy,
                     CreatedDate = profile.CreatedDate,
                     UpdatedBy = profile.UpdatedBy,
                     UpdatedDate = profile.UpdatedDate
                 }).ToList();

            return result;
        }

        public object? GetActiveProfile(int orgId, int branchId, int counterId)
        {
            var profiles =
                from profile in _uow.GenericRepository<DualDisplayProfile>().Table()
                join branch in _uow.GenericRepository<Branch>().Table()
                    on profile.BranchId equals branch.Id
                join counter in _uow.GenericRepository<Counter>().Table()
                    on profile.CounterId equals counter.Id
                join organization in _uow.GenericRepository<Organization>().Table()
                    on profile.OrgId equals organization.Id
                where profile.IsDeleted == false
                      && profile.IsActive == true
                      && (orgId == 0 || profile.OrgId == orgId)
                      && (branchId == 0 || profile.BranchId == branchId)
                orderby (counterId > 0 && profile.CounterId == counterId) descending,
                        profile.UpdatedDate descending,
                        profile.CreatedDate descending,
                        profile.Id descending
                select new
                {
                    Id = profile.Id,
                    ProfileCode = profile.ProfileCode,
                    ProfileName = profile.ProfileName,
                    OrgId = profile.OrgId,
                    OrgName = organization.Name,
                    BranchId = profile.BranchId,
                    BranchName = branch.Name,
                    CounterId = profile.CounterId,
                    CounterName = counter.Name,
                    ThemeName = profile.ThemeName,
                    HeaderTitle = profile.HeaderTitle,
                    WelcomeMessage = profile.WelcomeMessage,
                    IdleMessage = profile.IdleMessage,
                    IsActive = profile.IsActive,
                    IsDeleted = profile.IsDeleted,
                    CreatedBy = profile.CreatedBy,
                    CreatedDate = profile.CreatedDate,
                    UpdatedBy = profile.UpdatedBy,
                    UpdatedDate = profile.UpdatedDate
                };

            return profiles.FirstOrDefault();
        }

        public DualDisplayProfile? GetById(int id)
        {
            return _uow.GenericRepository<DualDisplayProfile>()
                .Table()
                .FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
        }

        public string Create(DualDisplayProfile profile)
        {
            var duplicateCode = _uow.GenericRepository<DualDisplayProfile>().Table()
                .Count(x =>
                    x.OrgId == profile.OrgId &&
                    x.IsDeleted == false &&
                    x.ProfileCode.Trim().ToLower() == profile.ProfileCode.Trim().ToLower());

            if (duplicateCode > 0)
            {
                return "AlreadyExists";
            }

            var duplicateCounter = _uow.GenericRepository<DualDisplayProfile>().Table()
                .Count(x =>
                    x.OrgId == profile.OrgId &&
                    x.BranchId == profile.BranchId &&
                    x.CounterId == profile.CounterId &&
                    x.IsDeleted == false);

            if (duplicateCounter > 0)
            {
                return "CounterAlreadyMapped";
            }

            var entity = new DualDisplayProfile
            {
                ProfileCode = profile.ProfileCode,
                ProfileName = profile.ProfileName,
                OrgId = profile.OrgId,
                BranchId = profile.BranchId,
                CounterId = profile.CounterId,
                ThemeName = profile.ThemeName,
                HeaderTitle = profile.HeaderTitle,
                WelcomeMessage = profile.WelcomeMessage,
                IdleMessage = profile.IdleMessage,
                IsActive = profile.IsActive ?? true,
                IsDeleted = false,
                CreatedBy = profile.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<DualDisplayProfile>().Insert(entity);
            _uow.Save();

            if (entity.IsActive == true)
            {
                DeactivateOtherProfiles(entity.Id, entity.OrgId, entity.BranchId, entity.CounterId);
            }

            return Convert.ToString(entity.Id);
        }

        public string Update(DualDisplayProfile profile)
        {
            var duplicateCode = _uow.GenericRepository<DualDisplayProfile>().Table()
                .Count(x =>
                    x.Id != profile.Id &&
                    x.OrgId == profile.OrgId &&
                    x.IsDeleted == false &&
                    x.ProfileCode.Trim().ToLower() == profile.ProfileCode.Trim().ToLower());

            if (duplicateCode > 0)
            {
                return "AlreadyExists";
            }

            var duplicateCounter = _uow.GenericRepository<DualDisplayProfile>().Table()
                .Count(x =>
                    x.Id != profile.Id &&
                    x.OrgId == profile.OrgId &&
                    x.BranchId == profile.BranchId &&
                    x.CounterId == profile.CounterId &&
                    x.IsDeleted == false);

            if (duplicateCounter > 0)
            {
                return "CounterAlreadyMapped";
            }

            var existingProfile = _uow.GenericRepository<DualDisplayProfile>().Table()
                .FirstOrDefault(x => x.Id == profile.Id && x.IsDeleted == false);

            if (existingProfile == null)
            {
                return "";
            }

            existingProfile.ProfileCode = profile.ProfileCode;
            existingProfile.ProfileName = profile.ProfileName;
            existingProfile.OrgId = profile.OrgId;
            existingProfile.BranchId = profile.BranchId;
            existingProfile.CounterId = profile.CounterId;
            existingProfile.ThemeName = profile.ThemeName;
            existingProfile.HeaderTitle = profile.HeaderTitle;
            existingProfile.WelcomeMessage = profile.WelcomeMessage;
            existingProfile.IdleMessage = profile.IdleMessage;
            existingProfile.IsActive = profile.IsActive ?? true;
            existingProfile.IsDeleted = false;
            existingProfile.UpdatedBy = profile.UpdatedBy;
            existingProfile.UpdatedDate = DateTime.Now;

            _uow.GenericRepository<DualDisplayProfile>().Update(existingProfile);
            _uow.Save();

            if (existingProfile.IsActive == true)
            {
                DeactivateOtherProfiles(existingProfile.Id, existingProfile.OrgId, existingProfile.BranchId, existingProfile.CounterId);
            }

            return Convert.ToString(existingProfile.Id);
        }

        public string DeleteById(int id)
        {
            var result = _uow.GenericRepository<DualDisplayProfile>().Table()
                .FirstOrDefault(x => x.Id == id);

            if (result != null)
            {
                result.IsDeleted = true;
                result.UpdatedDate = DateTime.Now;
                _uow.GenericRepository<DualDisplayProfile>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }

        public string ActiveInActive(int id, bool isActive)
        {
            var result = _uow.GenericRepository<DualDisplayProfile>().Table()
                .FirstOrDefault(x => x.Id == id);

            if (result != null)
            {
                result.IsActive = isActive;
                result.UpdatedDate = DateTime.Now;
                _uow.GenericRepository<DualDisplayProfile>().Update(result);
                _uow.Save();

                if (isActive)
                {
                    DeactivateOtherProfiles(result.Id, result.OrgId, result.BranchId, result.CounterId);
                }
            }

            return Convert.ToString(result?.Id ?? 0);
        }

        private void DeactivateOtherProfiles(int currentId, int orgId, int branchId, int counterId)
        {
            var otherProfiles = _uow.GenericRepository<DualDisplayProfile>().Table()
                .Where(x =>
                    x.Id != currentId &&
                    x.OrgId == orgId &&
                    x.BranchId == branchId &&
                    x.CounterId == counterId &&
                    x.IsDeleted == false &&
                    x.IsActive == true)
                .ToList();

            if (!otherProfiles.Any())
            {
                return;
            }

            foreach (var profile in otherProfiles)
            {
                profile.IsActive = false;
                profile.UpdatedDate = DateTime.Now;
                _uow.GenericRepository<DualDisplayProfile>().Update(profile);
            }

            _uow.Save();
        }
    }
}
