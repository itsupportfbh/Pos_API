using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class OrderHoldItemsService:IOrderHoldItemsService
    {
        private readonly IUnitOfWork _uow;
        public OrderHoldItemsService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public string Create(OrderHoldItems item)
        {
            var entity = new OrderHoldItems
            {
                Orderid = item.Orderid,
                Menuitemid = item.Menuitemid,
                Itemname = item.Itemname,
                Quantity = item.Quantity,
                Unitprice = item.Unitprice,
                Totalprice = item.Quantity * item.Unitprice,
                DiscountAmount = item.DiscountAmount ?? 0,
                TaxAmount = item.TaxAmount ?? 0,
                Modifierdetails = item.Modifierdetails,
                Itemstatus =  item.Itemstatus,
                Notes = item.Notes,
                OrgId = item.OrgId,

                IsDeleted = false,
                CreatedBy = item.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<OrderHoldItems>().Insert(entity);
            _uow.Save();

            return Convert.ToString(entity.Itemid);
        }

        public string Update(OrderHoldItems item)
        {
            var existingItem = _uow.GenericRepository<OrderHoldItems>().Table()
                .FirstOrDefault(x => x.Itemid == item.Itemid && x.IsDeleted == false);

            if (existingItem == null)
            {
                return "";
            }

            existingItem.Orderid = item.Orderid;
            existingItem.Menuitemid = item.Menuitemid;
            existingItem.Itemname = item.Itemname;
            existingItem.Quantity = item.Quantity;
            existingItem.Unitprice = item.Unitprice;
            existingItem.Totalprice = item.Quantity * item.Unitprice;
            existingItem.DiscountAmount = item.DiscountAmount ?? 0;
            existingItem.TaxAmount = item.TaxAmount ?? 0;
            existingItem.Modifierdetails = item.Modifierdetails;
            existingItem.Itemstatus =  item.Itemstatus;
            existingItem.Notes = item.Notes;
            existingItem.OrgId = item.OrgId;

            existingItem.IsDeleted = false;
            existingItem.UpdatedBy = item.UpdatedBy;
            existingItem.UpdatedDate = DateTime.Now;

            _uow.GenericRepository<OrderHoldItems>().Update(existingItem);
            _uow.Save();

            return Convert.ToString(existingItem.Itemid);
        }

        public IEnumerable<object> GetAll(int orgid)
        {
            var result = (from item in _uow.GenericRepository<OrderHoldItems>().Table()

                          join oh in _uow.GenericRepository<OrdersHold>().Table()
                          on item.Orderid equals oh.OrderId

                          join o in _uow.GenericRepository<Organization>().Table()
                          on item.OrgId equals o.Id

                          where item.IsDeleted == false
                             && oh.IsDeleted == false
                             && (orgid == 0 || item.OrgId == 0 || item.OrgId == orgid)

                          select new
                          {
                              Itemid = item.Itemid,
                              Orderid = item.Orderid,
                              Ordernumber = oh.Ordernumber,

                              Menuitemid = item.Menuitemid,
                              Itemname = item.Itemname,
                              Quantity = item.Quantity,
                              Unitprice = item.Unitprice,
                              Totalprice = item.Totalprice,
                              DiscountAmount = item.DiscountAmount,
                              TaxAmount = item.TaxAmount,
                              Modifierdetails = item.Modifierdetails,
                              Itemstatus = item.Itemstatus,
                              Notes = item.Notes,

                              OrganizationId = item.OrgId,
                              OrganizationName = o.Name
                          })
                          .ToList();

            return result;
        }

        public IEnumerable<object> GetByOrderId(long orderId)
        {
            var result = _uow.GenericRepository<OrderHoldItems>().Table()
                .Where(x => x.Orderid == orderId && x.IsDeleted == false)
                .Select(x => new
                {
                    x.Itemid,
                    x.Orderid,
                    x.Menuitemid,
                    x.Itemname,
                    x.Quantity,
                    x.Unitprice,
                    x.Totalprice,
                    x.DiscountAmount,
                    x.TaxAmount,
                    x.Modifierdetails,
                    x.Itemstatus,
                    x.Notes,
                    x.OrgId
                })
                .ToList();

            return result;
        }

        public OrderHoldItems GetById(long itemId)
        {
            var result = _uow.GenericRepository<OrderHoldItems>()
                .Table()
                .FirstOrDefault(x => x.Itemid == itemId && x.IsDeleted == false);

            return result;
        }

        public string Delete(long itemId)
        {
            var result = _uow.GenericRepository<OrderHoldItems>()
                .Table()
                .FirstOrDefault(x => x.Itemid == itemId);

            if (result != null)
            {
                result.IsDeleted = true;
                result.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<OrderHoldItems>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Itemid ?? 0);
        }

    }
}
