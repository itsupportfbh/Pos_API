using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface ICommonService
    {
        public IEnumerable<Object> GetCountry();
        public IEnumerable<Object> GetStateByCountryId(int CountryId);
        public IEnumerable<Object> GetCityByStateId(int StateId);

    }
}
