using UNITYPOS_API.Entities;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IOrderService
    {
        
        IEnumerable<object> GetAllOrderDetails(int orderId, int branchId);
        Orders? GetById(int orderId);
        string Delete(int orderId);
        public Task<string> Create(Orders orders);
        public Task<string> Update(Orders orders);
        public string StatusChange(Orders order);
        public string KitchenItemStatusChange(Orderitems orderItem);
        public IEnumerable<Object> GetAllDiningTable(int orgid, int branchid);
        public IEnumerable<object> GetTopSixMenuAndComboMenu(int orgid, int branchid);
        public Task<IEnumerable<object>> GetAllMenuAndComboMenu(int orgid, int branchid, int? categoryId, int? subCategoryId, string? searchKey);
    }
}
