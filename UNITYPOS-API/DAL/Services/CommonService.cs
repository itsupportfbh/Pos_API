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
    public class CommonService : ICommonService
    {

        private readonly IUnitOfWork _uow;
        public CommonService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
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
                      .Where(x => x.StateId == StateId )
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

    }
}
