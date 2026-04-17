using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities;

namespace UNITYPOS_API.DAL.Services
{
    public class CounterService:ICounterService
    {
        private readonly IUnitOfWork _uow;
        public CounterService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        



    }
}
