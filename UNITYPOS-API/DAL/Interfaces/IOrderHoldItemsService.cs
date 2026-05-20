using UNITYPOS_API.Entities;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IOrderHoldItemsService
    {
        public string Create(OrderHoldItems item);
        public string Update(OrderHoldItems item);
        public IEnumerable<object> GetAll(int orgid);
        public IEnumerable<object> GetByOrderId(long orderId);
        public OrderHoldItems GetById(long itemId);
        public string Delete(long itemId);
    }
}
