using UNITYPOS_API.Entities;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IOrderService
    {
        string Create(Orders order);
        string Update(Orders order);
     //   IEnumerable<object> GetAll(int orgid);
        IEnumerable<object> GetAllOrderDetails(long orderId, long branchId);
        Orders? GetById(long orderId);
        string Delete(long orderId);

        public string GenerateOrderNumber(int orgId, int branchId);
    }
}
