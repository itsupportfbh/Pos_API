using Microsoft.AspNetCore.Http;
using UNITYPOS_API.Entities;
using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface ICommonService
    {
        public IEnumerable<Object> GetCountry();
        public IEnumerable<Object> GetStateByCountryId(int CountryId);
        public IEnumerable<Object> GetCityByStateId(int StateId);
        public Task<FileUploadResult> FileUpload(IFormFile postedFile, string folderName);
        public IEnumerable<Object> GetBranchByUserId(int StateId);
    }
}
