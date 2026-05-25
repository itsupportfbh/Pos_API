using UNITYPOS_API.Entities;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IOrderHoldService
    {
       // public string Create(OrdersHold ordershold);
       // public string Update(OrdersHold ordershold);
        public OrdersHold GetById(long orderId);
        public IEnumerable<object> GetAll(int orgid);
        public string Delete(long orderId);
        public string Deletepermanantly(long orderId);
        public Task<string> Create(OrdersHold ordershold);
        public Task<string> Update(OrdersHold ordershold);
        public IEnumerable<object> GetAllHoldorderDetails(long orderId);
    }
}
