using Newtonsoft.Json;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.DAL.Services
{
    public class SyncService : ISyncService
    {
        private readonly IUnitOfWork _uow;

        public SyncService(
            IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<object> DownloadAsync(
            DownloadRequestDto request)
        {
            switch (request.TableName)
            { 
                case "FoodMenu":
                    return await DownloadFoodMenu(
                        request);

                //case "Supplier":
                //    return await DownloadSupplier(
                //        request);

                default:
                    return null;
            }
        }

        public async Task<bool> UploadAsync(
            UploadRequestDto request)
        {
            switch (request.TableName)
            {
                case "Product":
                    return await UploadFoodMenu(
                        request);

                //case "Customer":
                //    return await UploadCustomer(
                //        request);

                default:
                    return false;
            }
        }

        private async Task<List<FoodMenu>>
DownloadFoodMenu(
    DownloadRequestDto request)
        {
            var repo =
                _uow.GenericRepository<FoodMenu>();

            return repo
                .GetAll()
                .Where(x =>
                    x.Id > request.LastDownloadId)
                .OrderBy(x => x.Id)
                .ToList();
        }

        private async Task<bool>
UploadFoodMenu(
    UploadRequestDto request)
        {
            var products =
                JsonConvert.DeserializeObject
                <List<FoodMenu>>
                (
                    request.Data.ToString()
                );

            foreach (var item in products)
            {
                var existing =
                    _uow
                    .GenericRepository<FoodMenu>()
                    .GetAll()
                    .FirstOrDefault(x =>
                        x.Id == item.Id);

                if (existing == null)
                {
                    await _uow
                        .GenericRepository<FoodMenu>()
                        .InsertAsync(item);
                }
                else
                {
                    existing.Name = item.Name;
                    existing.Price = item.Price;
                }
            }

            await _uow.SaveAsync();

            return true;
        }
    }
}
