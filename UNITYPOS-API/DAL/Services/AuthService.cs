using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly ITokenService _tokenService;

        public AuthService(IUnitOfWork uow, ITokenService tokenService)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _tokenService = tokenService;
        }
        public Object Login(string Email, string Password)
        {
           Object result = null;

            var user = _uow.GenericRepository<UserMaster>().Table().FirstOrDefault(u => (u.Email == Email || u.EmpCode == Email) && u.Password == Password && u.IsActive == true && u.IsDeleted == false);

            if (user != null)
            {
                var tokenResult = _tokenService.GenerateToken(user);

                int UserId = (int)(from um in _uow.GenericRepository<UserMaster>().Table()
                              where (um.Email == Email || um.EmpCode == Email) && um.Password == Password && um.IsActive == true && um.IsDeleted == false
                              select um.Id).FirstOrDefault();

                int OrgId = (int)(from um in _uow.GenericRepository<UserMaster>().Table()
                             where (um.Email == Email || um.EmpCode == Email) && um.Password == Password && um.IsActive == true && um.IsDeleted == false
                             select um.OrgId).FirstOrDefault();

      var UserDetails = (
                  from ur in _uow.GenericRepository<UserMaster>().Table()
               
                  join um in _uow.GenericRepository<UserRoleMapping>().Table()
                      on ur.Id equals um.UserId
               
                  join rm in _uow.GenericRepository<RoleMaster>().Table()
                      on um.RoleId equals rm.Id
               
                  join org in _uow.GenericRepository<Organization>().Table()
                      on ur.OrgId equals org.Id
               
                  where ur.IsDeleted == false
                      && ur.IsActive == true
                      && um.IsDeleted == false
                      && rm.IsDeleted == false
                      && rm.IsActive == true
                      && org.IsDeleted == false
                      && ur.Id == UserId
                      && um.UserId == UserId
                      && rm.OrgId == OrgId
               
                  select new
                  {
                      ur.IsAdmin,
                      UserId = ur.Id,
                      UserCode = ur.Code,
                      ur.EmpCode,
                      UserName = ur.Name,
                      RoleId = rm.Id,
                      RoleName = rm.Name,
                      ur.OrgId,
                      OrgCode = org.Code,
                      OrgName = org.Name,
                      BranchId = _uow.GenericRepository<UserBranchMapping>().Table()
                                      .Where(bm => bm.IsDeleted == false && bm.UserId == UserId)
                                      .Select(bm => (int?)bm.BranchId)
                                      .FirstOrDefault(),
               
                      BranchName = (
                          from br in _uow.GenericRepository<Branch>().Table()
                          join bm in _uow.GenericRepository<UserBranchMapping>().Table()
                              on br.Id equals bm.BranchId
                          where br.IsActive == true
                              && br.IsDeleted == false
                              && br.OrgId == OrgId
                              && bm.IsDeleted == false
                              && bm.UserId == UserId
                          select br.Name
                      ).FirstOrDefault()
                  }
                ).ToList();

                result = new
                {
                    UserDetails,
                    Status = true,
                    Token = tokenResult.Token,
                    Expiration = tokenResult.Expiration,
                };

            }
            else
            {
                return new
                {
                    Status = false,
                    Message = "Invalid email or password"
                };
            }

            return result;
        }
    }
}
