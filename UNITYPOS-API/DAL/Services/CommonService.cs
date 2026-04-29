using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities;
using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.DAL.Services
{
    public class CommonService : ICommonService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;

        public CommonService(IUnitOfWork uow, IConfiguration configuration)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _configuration = configuration;
        }

        public IEnumerable<Object> GetCountry()
        {
            IEnumerable<Object> result = null;

            result = (from c in _uow.GenericRepository<CountryMaster>().Table()
                      select new
                      {
                          c.Id,
                          c.Name,
                          c.Iso3,
                          c.Iso2,
                          c.NumericCode,
                          c.Phonecode,
                          c.Capital,
                          c.Currency,
                          c.CurrencyName,
                          c.CurrencySymbol,
                          c.Tld,
                          c.Native,
                          c.Population,
                          c.Gdp,
                          c.Region,
                          c.RegionId,
                          c.Subregion,
                          c.SubregionId,
                          c.Nationality,
                          c.AreaSqKm,
                          c.Latitude,
                          c.Longitude,
                          c.Emoji,
                          c.WikiDataId
                      }).AsNoTracking()
                         .ToList();

            return result;
        }

        public IEnumerable<Object> GetStateByCountryId(int CountryId)
        {
            IEnumerable<Object> result = null;

            result = (from x in _uow.GenericRepository<StateMaster>()
                      .Table()
                      .Where(x => x.CountryId == CountryId)
                      select new
                      {
                          x.Id,
                          x.Name,
                          x.CountryId,
                          x.CountryCode,
                          x.CountryName,
                          x.Iso2,
                          x.Iso3166_2,
                          x.FipsCode,
                          x.Type,
                          x.Level,
                          x.ParentId,
                          x.Native,
                          x.Latitude,
                          x.Longitude,
                          x.Timezone,
                          x.WikiDataId,
                          x.Population
                      }).AsNoTracking()
                      .ToList();
            return result;
        }

        public IEnumerable<Object> GetCityByStateId(int StateId)
        {
            IEnumerable<Object> result = null;

            result = (from x in _uow.GenericRepository<CityMaster>()
                      .Table()
                      .Where(x => x.StateId == StateId)
                      select new
                      {
                          x.Id,
                          x.Name,
                          x.StateId,
                          x.CountryId,
                          x.Latitude,
                          x.Longitude,
                          x.Timezone

                      }).AsNoTracking()
                      .ToList();

            return result;
        }

        public async Task<FileUploadResult> FileUpload(IFormFile postedFile, string folderName)
        {
            if (postedFile == null || postedFile.Length == 0)
            {
                throw new Exception("Please upload file.");
            }

            int maxContentLength = 1024 * 1024 * 20;
            IList<string> allowedFileExtensions = new List<string>
            {
                ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tif", ".tiff", ".webp", ".svg", ".ico",
                ".doc", ".docx", ".pdf", ".txt", ".rtf", ".csv", ".xls", ".xlsx", ".ppt", ".pptx", ".odt", ".md", ".html", ".htm", ".xml", ".json", ".ps", ".epub",
                ".mp3", ".wav", ".ogg", ".flac", ".aac", ".wma", ".m4a", ".amr", ".mid", ".midi",
                ".mp4", ".mov", ".avi", ".mkv", ".wmv", ".flv", ".webm", ".m4v", ".mpeg", ".mpg", ".3gp", ".3g2"
            };

            var ext = Path.GetExtension(postedFile.FileName);
            var fileExtension = ext.ToLower();

            if (!allowedFileExtensions.Contains(fileExtension))
            {
                throw new Exception("Please upload valid file.");
            }

            if (postedFile.Length > maxContentLength)
            {
                throw new Exception("Please upload a file up to 20 MB.");
            }

            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string uploadPathView = _configuration["AppSettings:FileUploadPathView"] ?? string.Empty;
            string fileName = Path.GetFileName(postedFile.FileName);
            string extension = Path.GetExtension(fileName);
            string uploadPathName = extension.Substring(1).ToUpper() + timeStamp + extension;

            string startupPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "FileUpload", folderName);
            if (!Directory.Exists(startupPath))
            {
                Directory.CreateDirectory(startupPath);
            }

            var filePath = Path.Combine(startupPath, uploadPathName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await postedFile.CopyToAsync(stream);
            }

            var url = folderName + "/" + uploadPathName;

            return new FileUploadResult
            {
                Url = url,
                FileName = uploadPathName,
                FileType = folderName,
                FullPath = uploadPathView + url,
            };
        }

        public IEnumerable<Object> GetBranchByUserId(int Userid)
        {
            IEnumerable<Object> result = null;

            result = (from x in _uow.GenericRepository<UserBranchMapping>().Table()
                      join u in _uow.GenericRepository<Branch>().Table()
                        on x.BranchId equals u.Id
                      where x.UserId == Userid
                      select new
                      {
                          x.Id,
                          u.Name
                      }).ToList();

            return result;
        }
    }
}
