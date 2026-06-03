using System.Net.NetworkInformation;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class AuthService : IAuthService
    {
        private sealed class OrganizationLocaleInfo
        {
            public string OrgCode { get; set; } = string.Empty;
            public string OrgName { get; set; } = string.Empty;
            public int? OrgCountry { get; set; }
            public int? OrgState { get; set; }
            public int? OrgCity { get; set; }
            public string? OrgLanguageCode { get; set; }
            public string? OrgCurrencyCode { get; set; }
            public string? OrgCurrencyName { get; set; }
            public string? OrgCurrencySymbol { get; set; }
            public string? OrgTimezone { get; set; }
        }

        private sealed class BranchLocaleInfo
        {
            public int? BranchId { get; set; }
            public string? BranchName { get; set; }
            public string? BranchLanguageCode { get; set; }
            public int? BranchCountry { get; set; }
            public int? BranchState { get; set; }
            public int? BranchCity { get; set; }
            public string? BranchCurrencyCode { get; set; }
            public string? BranchCurrencyName { get; set; }
            public string? BranchCurrencySymbol { get; set; }
            public string? BranchTimezone { get; set; }
        }

        private sealed class LoginUserDetail
        {
            public bool? IsAdmin { get; set; }
            public int? UserId { get; set; }
            public string? UserCode { get; set; }
            public string? EmpCode { get; set; }
            public string? UserName { get; set; }
            public int RoleId { get; set; }
            public string? RoleName { get; set; }
            public int? OrgId { get; set; }
            public string? Image { get; set; }
            public string? OrgCode { get; set; }
            public string? OrgName { get; set; }
            public int? OrgCountry { get; set; }
            public int? OrgState { get; set; }
            public int? OrgCity { get; set; }
            public string? OrgLanguageCode { get; set; }
            public string? OrgCurrencyCode { get; set; }
            public string? OrgCurrencyName { get; set; }
            public string? OrgCurrencySymbol { get; set; }
            public string? OrgTimezone { get; set; }
            public int? BranchId { get; set; }
            public string? BranchName { get; set; }
            public string? BranchLanguageCode { get; set; }
            public int? BranchCountry { get; set; }
            public int? BranchState { get; set; }
            public int? BranchCity { get; set; }
            public string? BranchCurrencyCode { get; set; }
            public string? BranchCurrencyName { get; set; }
            public string? BranchCurrencySymbol { get; set; }
            public string? BranchTimezone { get; set; }
            public string? LanguageCode { get; set; }
            public string? CurrencyCode { get; set; }
            public string? CurrencyName { get; set; }
            public string? CurrencySymbol { get; set; }
            public string? Timezone { get; set; }
        }

        private readonly IUnitOfWork _uow;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork uow, ITokenService tokenService, IConfiguration configuration)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _tokenService = tokenService;
            _configuration = configuration;
        }
        public Object Login(string Email, string Password)
        {
            string fileUploadPathView = _configuration["AppSettings:FileUploadPathView"] ?? string.Empty;

            Object result = null;

            var user = _uow.GenericRepository<UserMaster>().Table()
                .FirstOrDefault(u => (u.Email == Email || u.EmpCode == Email) && u.IsActive == true && u.IsDeleted == false);

            bool isPasswordValid = false;

            if (user != null && !string.IsNullOrWhiteSpace(user.Password))
            {
                if (user.Password.StartsWith("$2"))
                {
                    isPasswordValid = BCrypt.Net.BCrypt.Verify(Password, user.Password);
                }
                else
                {
                    isPasswordValid = user.Password == Password;

                    if (isPasswordValid)
                    {
                        user.Password = BCrypt.Net.BCrypt.HashPassword(Password);
                        _uow.GenericRepository<UserMaster>().Update(user);
                        _uow.Save();
                    }
                }
            }

            if (user != null && isPasswordValid)
            {
                var tokenResult = _tokenService.GenerateToken(user);
                int UserId = user.Id ?? 0;
                int OrgId = user.OrgId ?? 0;

                var UserDetails = BuildUserDetails(UserId, OrgId, fileUploadPathView);

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

        public Object Login(int Pin)
        {
            string fileUploadPathView = _configuration["AppSettings:FileUploadPathView"] ?? string.Empty;

            Object result = null;

            var user = _uow.GenericRepository<UserMaster>().Table()
                .FirstOrDefault(u => (u.PinNo == Pin) && u.IsActive == true && u.IsDeleted == false);
        
            if (user != null)
            {
                var tokenResult = _tokenService.GenerateToken(user);
                int UserId = user.Id ?? 0;
                int OrgId = user.OrgId ?? 0;

                var UserDetails = BuildUserDetails(UserId, OrgId, fileUploadPathView);

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

        private List<LoginUserDetail> BuildUserDetails(int userId, int orgId, string fileUploadPathView)
        {
            var organizationLocale = GetOrganizationLocaleInfo(orgId);
            var branchLocale = GetBranchLocaleInfo(userId, orgId);
            var resolvedLanguageCode = FirstNonEmpty(branchLocale.BranchLanguageCode, organizationLocale.OrgLanguageCode);
            var resolvedCurrencyCode = FirstNonEmpty(branchLocale.BranchCurrencyCode, organizationLocale.OrgCurrencyCode);
            var resolvedCurrencyName = FirstNonEmpty(branchLocale.BranchCurrencyName, organizationLocale.OrgCurrencyName);
            var resolvedCurrencySymbol = FirstNonEmpty(branchLocale.BranchCurrencySymbol, organizationLocale.OrgCurrencySymbol);
            var resolvedTimezone = FirstNonEmpty(branchLocale.BranchTimezone, organizationLocale.OrgTimezone);

            return (
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
                         && ur.Id == userId
                         && um.UserId == userId
                         && rm.OrgId == orgId
                     select new LoginUserDetail
                     {
                         IsAdmin = ur.IsAdmin,
                         UserId = ur.Id,
                         UserCode = ur.Code,
                         EmpCode = ur.EmpCode,
                         UserName = ur.Name,
                         RoleId = rm.Id,
                         RoleName = rm.Name,
                         OrgId = ur.OrgId,
                         Image = fileUploadPathView + "User/" + ur.Image,
                         OrgCode = organizationLocale.OrgCode,
                         OrgName = organizationLocale.OrgName,
                         OrgCountry = organizationLocale.OrgCountry,
                         OrgState = organizationLocale.OrgState,
                         OrgCity = organizationLocale.OrgCity,
                         OrgLanguageCode = organizationLocale.OrgLanguageCode,
                         OrgCurrencyCode = organizationLocale.OrgCurrencyCode,
                         OrgCurrencyName = organizationLocale.OrgCurrencyName,
                         OrgCurrencySymbol = organizationLocale.OrgCurrencySymbol,
                         OrgTimezone = organizationLocale.OrgTimezone,
                         BranchId = branchLocale.BranchId,
                         BranchName = branchLocale.BranchName,
                         BranchLanguageCode = branchLocale.BranchLanguageCode,
                         BranchCountry = branchLocale.BranchCountry,
                         BranchState = branchLocale.BranchState,
                         BranchCity = branchLocale.BranchCity,
                         BranchCurrencyCode = branchLocale.BranchCurrencyCode,
                         BranchCurrencyName = branchLocale.BranchCurrencyName,
                         BranchCurrencySymbol = branchLocale.BranchCurrencySymbol,
                         BranchTimezone = branchLocale.BranchTimezone,
                         LanguageCode = resolvedLanguageCode,
                         CurrencyCode = resolvedCurrencyCode,
                         CurrencyName = resolvedCurrencyName,
                         CurrencySymbol = resolvedCurrencySymbol,
                         Timezone = resolvedTimezone
                     }
                   ).ToList();
        }

        private OrganizationLocaleInfo GetOrganizationLocaleInfo(int orgId)
        {
            return (
                from org in _uow.GenericRepository<Organization>().Table()
                join country in _uow.GenericRepository<CountryMaster>().Table()
                    on org.Country equals country.Id into countryJoin
                from country in countryJoin.DefaultIfEmpty()
                join city in _uow.GenericRepository<CityMaster>().Table()
                    on org.City equals city.Id into cityJoin
                from city in cityJoin.DefaultIfEmpty()
                join state in _uow.GenericRepository<StateMaster>().Table()
                    on org.State equals state.Id into stateJoin
                from state in stateJoin.DefaultIfEmpty()
                where org.Id == orgId
                    && org.IsDeleted == false
                select new OrganizationLocaleInfo
                {
                    OrgCode = org.Code,
                    OrgName = org.Name,
                    OrgCountry = org.Country,
                    OrgState = org.State,
                    OrgCity = org.City,
                    OrgLanguageCode = NormalizeText(org.LanguageCode),
                    OrgCurrencyCode = country != null ? country.Currency : null,
                    OrgCurrencyName = country != null ? country.CurrencyName : null,
                    OrgCurrencySymbol = country != null ? country.CurrencySymbol : null,
                    OrgTimezone = NormalizeText(city != null && !string.IsNullOrWhiteSpace(city.Timezone)
                        ? city.Timezone
                        : state != null ? state.Timezone : null)
                }
            ).FirstOrDefault() ?? new OrganizationLocaleInfo();
        }

        private BranchLocaleInfo GetBranchLocaleInfo(int userId, int orgId)
        {
            return (
                from bm in _uow.GenericRepository<UserBranchMapping>().Table()
                join br in _uow.GenericRepository<Branch>().Table()
                    on bm.BranchId equals br.Id
                join country in _uow.GenericRepository<CountryMaster>().Table()
                    on br.Country equals country.Id into countryJoin
                from country in countryJoin.DefaultIfEmpty()
                join city in _uow.GenericRepository<CityMaster>().Table()
                    on br.City equals city.Id into cityJoin
                from city in cityJoin.DefaultIfEmpty()
                join state in _uow.GenericRepository<StateMaster>().Table()
                    on br.State equals state.Id into stateJoin
                from state in stateJoin.DefaultIfEmpty()
                where bm.UserId == userId
                    && bm.IsDeleted == false
                    && br.IsActive == true
                    && br.IsDeleted == false
                    && br.OrgId == orgId
                orderby bm.Id, br.Id
                select new BranchLocaleInfo
                {
                    BranchId = br.Id,
                    BranchName = br.Name,
                    BranchLanguageCode = NormalizeText(br.LanguageCode),
                    BranchCountry = br.Country,
                    BranchState = br.State,
                    BranchCity = br.City,
                    BranchCurrencyCode = country != null ? country.Currency : null,
                    BranchCurrencyName = country != null ? country.CurrencyName : null,
                    BranchCurrencySymbol = country != null ? country.CurrencySymbol : null,
                    BranchTimezone = NormalizeText(city != null && !string.IsNullOrWhiteSpace(city.Timezone)
                        ? city.Timezone
                        : state != null ? state.Timezone : null)
                }
            ).FirstOrDefault() ?? new BranchLocaleInfo();
        }

        private static string? NormalizeText(string? value)
        {
            var normalized = (value ?? string.Empty).Trim();
            return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
        }

        private static string? FirstNonEmpty(params string?[] values)
        {
            foreach (var value in values)
            {
                var normalized = NormalizeText(value);
                if (!string.IsNullOrWhiteSpace(normalized))
                {
                    return normalized;
                }
            }

            return null;
        }
    }
}
