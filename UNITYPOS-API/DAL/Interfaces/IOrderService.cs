using UNITYPOS_API.Entities;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IOrderService
    {
        
        IEnumerable<object> GetAllOrderDetails(long orderId, long branchId);
        Orders? GetById(long orderId);
        string Delete(long orderId);

        public string GenerateOrderNumber(int orgId, int branchId);


        public Task<string> Create(Orders orders);
        public Task<string> Update(Orders orders);
        public string StatusChange(Orders order);
        public string KitchenItemStatusChange(Orderitems orderItem);
    }
}
