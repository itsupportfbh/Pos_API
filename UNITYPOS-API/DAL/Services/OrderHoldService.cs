using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class OrderHoldService:IOrderHoldService

    {


        private readonly IUnitOfWork _uow;
        public OrderHoldService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }
        //public string Create(OrdersHold ordershold)
        //{
        //    int check = _uow.GenericRepository<OrdersHold>().Table()
        //        .Count(o => o.Ordernumber.ToLower() == ordershold.Ordernumber.ToLower()
        //                 && o.OrgId == ordershold.OrgId
        //                 && o.IsDeleted == false);

        //    if (check > 0)
        //    {
        //        return "AlreadyExists";
        //    }

        //    var entity = new OrdersHold
        //    {
        //        Ordernumber = ordershold.Ordernumber,
        //        Tableid = ordershold.Tableid,
        //        Ordertype = ordershold.Ordertype,
        //        Orderstatus = string.IsNullOrWhiteSpace(ordershold.Orderstatus)
        //                        ? "OPEN"
        //                        : ordershold.Orderstatus,

        //        Guestcount = ordershold.Guestcount ?? 1,
        //        SubtotalAmount = ordershold.SubtotalAmount ?? 0,
        //        TaxAmount = ordershold.TaxAmount ?? 0,
        //        DiscountAmount = ordershold.DiscountAmount ?? 0,
        //        TotalAmount = ordershold.TotalAmount ?? 0,
        //        Shiftid = ordershold.Shiftid,
        //        OrgId = ordershold.OrgId,


        //        IsDeleted = false,
        //        CreatedBy = ordershold.CreatedBy,
        //        CreatedDate = DateTime.Now
        //    };

        //    _uow.GenericRepository<OrdersHold>().Insert(entity);
        //    _uow.Save();

        //    return Convert.ToString(entity.OrderId);
        //}


        public string Create(OrdersHold ordershold)
        {
            int check = _uow.GenericRepository<OrdersHold>().Table()
                .Count(o => o.Ordernumber.ToLower() == ordershold.Ordernumber.ToLower()
                         && o.OrgId == ordershold.OrgId
                         && o.BranchId == ordershold.BranchId
                         && o.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var entity = new OrdersHold
            {
                Ordernumber = ordershold.Ordernumber,
                Tableid = ordershold.Tableid,
                Ordertype = ordershold.Ordertype,
                Orderstatus = string.IsNullOrWhiteSpace(ordershold.Orderstatus)
                                ? "OPEN"
                                : ordershold.Orderstatus,

                Itemcount = ordershold.Itemcount ?? 1,
                SubtotalAmount = ordershold.SubtotalAmount ?? 0,
                TaxAmount = ordershold.TaxAmount ?? 0,
                DiscountAmount = ordershold.DiscountAmount ?? 0,
                TotalAmount = ordershold.TotalAmount ?? 0,
                Shiftid = ordershold.Shiftid,
                OrgId = ordershold.OrgId,
                BranchId= ordershold.BranchId,
                IsDeleted = false,
                CreatedBy = ordershold.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<OrdersHold>().Insert(entity);
           _uow.Save();

            if (ordershold.Items != null && ordershold.Items.Any())
            {
                foreach (var item in ordershold.Items)
                {
                    var orderItem = new OrderHoldItems
                    {
                        Orderid = entity.OrderId,
                        Menuitemid = item.Menuitemid,
                        ComboMenuItemId=item.ComboMenuItemId,
                        Itemname = item.Itemname,
                        Quantity = item.Quantity,
                        Unitprice = item.Unitprice,
                        Totalprice = item.Quantity * item.Unitprice,
                        DiscountAmount = item.DiscountAmount ?? 0,
                        TaxAmount = item.TaxAmount ?? 0,
                        Modifierdetails = item.Modifierdetails,
                        Itemstatus = string.IsNullOrWhiteSpace(item.Itemstatus)
                                        ? "OPEN"
                                        : item.Itemstatus,
                        Notes = item.Notes,
                        OrgId = item.OrgId > 0 ? item.OrgId : ordershold.OrgId,

                        IsDeleted = false,
                        CreatedBy = ordershold.CreatedBy,
                        CreatedDate = DateTime.Now
                    };

                    _uow.GenericRepository<OrderHoldItems>().Insert(orderItem);
                }

               
            }
            _uow.Save();
            return Convert.ToString(entity.OrderId);
        }




        public string Update(OrdersHold ordershold)
        {
            int check = _uow.GenericRepository<OrdersHold>().Table()
                .Count(o => o.Ordernumber.ToLower() == ordershold.Ordernumber.ToLower()
                         && o.OrderId != ordershold.OrderId
                         && o.OrgId == ordershold.OrgId
                         && o.BranchId == ordershold.BranchId
                         && o.IsDeleted == false);

            if (check > 0)
                return "AlreadyExists";

            var existingOrder = _uow.GenericRepository<OrdersHold>().Table()
                .FirstOrDefault(x => x.OrderId == ordershold.OrderId
                                  && x.IsDeleted == false);

            if (existingOrder == null)
                return "";

            existingOrder.Ordernumber = ordershold.Ordernumber;
            existingOrder.Tableid = ordershold.Tableid;
            existingOrder.Ordertype = ordershold.Ordertype;
            existingOrder.Orderstatus = string.IsNullOrWhiteSpace(ordershold.Orderstatus)
                ? "Hold"
                : ordershold.Orderstatus;

            existingOrder.Itemcount = ordershold.Itemcount ?? 1;
            existingOrder.SubtotalAmount = ordershold.SubtotalAmount ?? 0;
            existingOrder.TaxAmount = ordershold.TaxAmount ?? 0;
            existingOrder.DiscountAmount = ordershold.DiscountAmount ?? 0;
            existingOrder.TotalAmount = ordershold.TotalAmount ?? 0;
            existingOrder.Shiftid = ordershold.Shiftid;
            existingOrder.OrgId = ordershold.OrgId;
            existingOrder.BranchId = ordershold.BranchId;
            existingOrder.IsDeleted = false;
            existingOrder.UpdatedBy = ordershold.UpdatedBy;
            existingOrder.UpdatedDate = DateTime.Now;

            _uow.GenericRepository<OrdersHold>().Update(existingOrder);

            var existingItems = _uow.GenericRepository<OrderHoldItems>().Table()
                .Where(x => x.Orderid == existingOrder.OrderId)
                .ToList();

            var incomingItems = ordershold.Items ?? new List<OrderHoldItems>();

            foreach (var item in incomingItems)
            {
                var oldItem = existingItems.FirstOrDefault(x =>
                    x.Menuitemid == item.Menuitemid &&
                    (x.ComboMenuItemId ?? 0) == (item.ComboMenuItemId ?? 0) &&
                    x.IsDeleted == false);

                if (oldItem != null)
                {
                    oldItem.ComboMenuItemId = item.ComboMenuItemId;
                    oldItem.Itemname = item.Itemname;
                    oldItem.Quantity = item.Quantity;
                    oldItem.Unitprice = item.Unitprice;
                    oldItem.Totalprice = item.Totalprice > 0
                        ? item.Totalprice
                        : item.Quantity * item.Unitprice;

                    oldItem.DiscountAmount = item.DiscountAmount ?? 0;
                    oldItem.TaxAmount = item.TaxAmount ?? 0;
                    oldItem.Modifierdetails = item.Modifierdetails;
                    oldItem.Itemstatus = string.IsNullOrWhiteSpace(item.Itemstatus)
                        ? "Hold"
                        : item.Itemstatus;

                    oldItem.Notes = item.Notes;
                    oldItem.OrgId = item.OrgId > 0 ? item.OrgId : existingOrder.OrgId;
                    oldItem.IsDeleted = false;
                    oldItem.UpdatedBy = ordershold.UpdatedBy;
                    oldItem.UpdatedDate = DateTime.Now;

                    _uow.GenericRepository<OrderHoldItems>().Update(oldItem);
                }
                else
                {
                    var newItem = new OrderHoldItems
                    {
                        Orderid = existingOrder.OrderId,
                        Menuitemid = item.Menuitemid,
                        ComboMenuItemId = item.ComboMenuItemId,
                        Itemname = item.Itemname,
                        Quantity = item.Quantity,
                        Unitprice = item.Unitprice,
                        Totalprice = item.Totalprice > 0
                            ? item.Totalprice
                            : item.Quantity * item.Unitprice,

                        DiscountAmount = item.DiscountAmount ?? 0,
                        TaxAmount = item.TaxAmount ?? 0,
                        Modifierdetails = item.Modifierdetails,
                        Itemstatus = string.IsNullOrWhiteSpace(item.Itemstatus)
                            ? "Hold"
                            : item.Itemstatus,

                        Notes = item.Notes,
                        OrgId = item.OrgId > 0 ? item.OrgId : existingOrder.OrgId,
                        IsDeleted = false,
                        CreatedBy = ordershold.UpdatedBy,
                        CreatedDate = DateTime.Now
                    };

                    _uow.GenericRepository<OrderHoldItems>().Insert(newItem);
                }
            }

            // ✅ Soft delete removed menu items
            var incomingKeys = incomingItems
                .Select(x => new
                {
                    x.Menuitemid,
                    ComboMenuItemId = x.ComboMenuItemId ?? 0
                })
                .ToList();

            var removedItems = existingItems
                .Where(old => old.IsDeleted == false &&
                    !incomingKeys.Any(newItem =>
                        newItem.Menuitemid == old.Menuitemid &&
                        newItem.ComboMenuItemId == (old.ComboMenuItemId ?? 0)))
                .ToList();

            foreach (var removed in removedItems)
            {
                removed.IsDeleted = true;
                removed.UpdatedBy = ordershold.UpdatedBy;
                removed.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<OrderHoldItems>().Update(removed);
            }

            _uow.Save();

            return Convert.ToString(existingOrder.OrderId);
        }
        public IEnumerable<object> GetAll(int orgid)
        {
            var result = (from oh in _uow.GenericRepository<OrdersHold>().Table()
                          join o in _uow.GenericRepository<Organization>().Table()
                          on oh.OrgId equals o.Id

                          where oh.IsDeleted != true
                             && (orgid == 0 || oh.OrgId == orgid)

                          select new
                          {
                              OrderId = oh.OrderId,
                              Ordernumber = oh.Ordernumber,
                              Tableid = oh.Tableid,
                              Ordertype = oh.Ordertype,
                              Orderstatus = oh.Orderstatus,

                              Itemcount = oh.Itemcount,

                              SubtotalAmount = oh.SubtotalAmount,
                              TaxAmount = oh.TaxAmount,
                              DiscountAmount = oh.DiscountAmount,
                              TotalAmount = oh.TotalAmount,

                              Shiftid = oh.Shiftid,

                              OrganizationId = oh.OrgId,
                              OrganizationName = o.Name
                          })
                          .ToList();

            return result;
        }


        public IEnumerable<object> GetAllHoldorderDetails(long orderId)
        {
            var result = (from oh in _uow.GenericRepository<OrdersHold>().Table()
                          join o in _uow.GenericRepository<Organization>().Table()
                          on oh.OrgId equals o.Id

                          where oh.IsDeleted != true
                             && oh.OrderId == orderId

                          select new
                          {
                              OrderId = oh.OrderId,
                              Ordernumber = oh.Ordernumber,
                              Tableid = oh.Tableid,
                              Ordertype = oh.Ordertype,
                              Orderstatus = oh.Orderstatus,
                              Itemcount = oh.Itemcount,

                              SubtotalAmount = oh.SubtotalAmount,
                              TaxAmount = oh.TaxAmount,
                              DiscountAmount = oh.DiscountAmount,
                              TotalAmount = oh.TotalAmount,

                              Shiftid = oh.Shiftid,

                              OrganizationId = oh.OrgId,
                              OrganizationName = o.Name,

                              Items = _uow.GenericRepository<OrderHoldItems>().Table()
                                  .Where(x => x.Orderid == oh.OrderId
                                           && x.IsDeleted != true)
                                  .Select(x => new
                                  {
                                      Itemid = x.Itemid,
                                      Orderid = x.Orderid,

                                      Menuitemid = x.Menuitemid,
                                      ComboMenuItemId = x.ComboMenuItemId,

                                      Itemname = x.Itemname,
                                      Quantity = x.Quantity,
                                      Unitprice = x.Unitprice,
                                      Totalprice = x.Totalprice,

                                      DiscountAmount = x.DiscountAmount,
                                      TaxAmount = x.TaxAmount,

                                      Modifierdetails = x.Modifierdetails,
                                      Itemstatus = x.Itemstatus,
                                      Notes = x.Notes,

                                      OrgId = x.OrgId,
                                      CreatedBy = x.CreatedBy,
                                      CreatedDate = x.CreatedDate,
                                      UpdatedBy = x.UpdatedBy,
                                      UpdatedDate = x.UpdatedDate
                                  })
                                  .ToList()
                          })
                          .ToList();

            return result;
        }
        public OrdersHold GetById(long orderId)
        {
            var result = _uow.GenericRepository<OrdersHold>()
                       .Table()
                       .Where(x => x.OrderId == orderId
                       && x.IsDeleted == false)
                       .FirstOrDefault();

            return result;
        }


        public string Delete(long orderId)
        {
            var result = _uow.GenericRepository<OrdersHold>()
                            .Table()
                            .Where(x => x.OrderId == orderId)
                            .FirstOrDefault();

            if (result != null)
            {
                result.IsDeleted = true;

                _uow.GenericRepository<OrdersHold>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.OrderId ?? 0);
        }


        //public string ActiveInActive(long orderId, bool isActive)
        //{
        //    var result = _uow.GenericRepository<OrdersHold>()
        //                    .Table()
        //                    .Where(x => x.OrderId == orderId)
        //                    .FirstOrDefault();

        //    if (result != null)
        //    {
        //        result.IsActive = isActive;

        //        _uow.GenericRepository<OrdersHold>().Update(result);
        //        _uow.Save();
        //    }

        //    return Convert.ToString(result?.OrderId ?? 0);
        //}

    }
}
