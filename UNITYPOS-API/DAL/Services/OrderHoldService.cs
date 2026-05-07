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
        public string Create(OrdersHold ordershold)
        {
            int check = _uow.GenericRepository<OrdersHold>().Table()
                .Count(o => o.Ordernumber.ToLower() == ordershold.Ordernumber.ToLower()
                         && o.OrgId == ordershold.OrgId
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

                Guestcount = ordershold.Guestcount ?? 1,
                SubtotalAmount = ordershold.SubtotalAmount ?? 0,
                TaxAmount = ordershold.TaxAmount ?? 0,
                DiscountAmount = ordershold.DiscountAmount ?? 0,
                TotalAmount = ordershold.TotalAmount ?? 0,
                Shiftid = ordershold.Shiftid,
                OrgId = ordershold.OrgId,

              
                IsDeleted = false,
                CreatedBy = ordershold.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<OrdersHold>().Insert(entity);
            _uow.Save();

            return Convert.ToString(entity.OrderId);
        }




        public string Update(OrdersHold ordershold)
        {
            int check = _uow.GenericRepository<OrdersHold>().Table()
                .Count(o => o.Ordernumber.ToLower() == ordershold.Ordernumber.ToLower()
                         && o.OrderId != ordershold.OrderId
                         && o.OrgId == ordershold.OrgId
                         && o.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var existingOrder = _uow.GenericRepository<OrdersHold>().Table()
                .FirstOrDefault(x => x.IsDeleted == false
                                  && x.OrderId == ordershold.OrderId);

            if (existingOrder == null)
            {
                return "";
            }

            existingOrder.Ordernumber = ordershold.Ordernumber;
            existingOrder.Tableid = ordershold.Tableid;
            existingOrder.Ordertype = ordershold.Ordertype;
            existingOrder.Orderstatus = string.IsNullOrWhiteSpace(ordershold.Orderstatus)
                                        ? "OPEN"
                                        : ordershold.Orderstatus;

            existingOrder.Guestcount = ordershold.Guestcount ?? 1;
            existingOrder.SubtotalAmount = ordershold.SubtotalAmount ?? 0;
            existingOrder.TaxAmount = ordershold.TaxAmount ?? 0;
            existingOrder.DiscountAmount = ordershold.DiscountAmount ?? 0;
            existingOrder.TotalAmount = ordershold.TotalAmount ?? 0;
            existingOrder.Shiftid = ordershold.Shiftid;
            existingOrder.OrgId = ordershold.OrgId;

           
            existingOrder.IsDeleted = false;
            existingOrder.UpdatedBy = ordershold.UpdatedBy;
            existingOrder.UpdatedDate = DateTime.Now;

            _uow.GenericRepository<OrdersHold>().Update(existingOrder);
            _uow.Save();

            return Convert.ToString(existingOrder.OrderId);
        }

        public IEnumerable<object> GetAll(int orgid)
        {
            var result = (from oh in _uow.GenericRepository<OrdersHold>().Table()

                          join o in _uow.GenericRepository<Organization>().Table()
                          on oh.OrgId equals o.Id

                          where oh.IsDeleted == false
                          && (orgid == 0 || oh.OrgId == 0 || oh.OrgId == orgid)

                          select new
                          {
                              OrderId = oh.OrderId,
                              Ordernumber = oh.Ordernumber,
                              Tableid = oh.Tableid,
                              Ordertype = oh.Ordertype,
                              Orderstatus = oh.Orderstatus,

                              Guestcount = oh.Guestcount,

                              SubtotalAmount = oh.SubtotalAmount,
                              TaxAmount = oh.TaxAmount,
                              DiscountAmount = oh.DiscountAmount,
                              TotalAmount = oh.TotalAmount,

                              Shiftid = oh.Shiftid,

                              OrganizationId = oh.OrgId,
                              OrganizationName = o.Name,

                             // IsActive = oh.IsActive
                          })
                          .ToList();

            return result;
        }


        public IEnumerable<object> GetAllHoldorderDetails(int orgid)
        {
            var result = (from oh in _uow.GenericRepository<OrdersHold>().Table()

                          join o in _uow.GenericRepository<Organization>().Table()
                          on oh.OrgId equals o.Id

                          join item in _uow.GenericRepository<OrderHoldItems>().Table()
                          on oh.OrderId equals item.Orderid into itemGroup

                          where oh.IsDeleted == false
                          && (orgid == 0 || oh.OrgId == 0 || oh.OrgId == orgid)

                          select new
                          {
                              OrderId = oh.OrderId,
                              Ordernumber = oh.Ordernumber,
                              Tableid = oh.Tableid,
                              Ordertype = oh.Ordertype,
                              Orderstatus = oh.Orderstatus,

                              Guestcount = oh.Guestcount,

                              SubtotalAmount = oh.SubtotalAmount,
                              TaxAmount = oh.TaxAmount,
                              DiscountAmount = oh.DiscountAmount,
                              TotalAmount = oh.TotalAmount,

                              Shiftid = oh.Shiftid,

                              OrganizationId = oh.OrgId,
                              OrganizationName = o.Name,

                              Items = itemGroup
                                      .Where(x => x.IsDeleted == false)
                                      .Select(x => new
                                      {
                                          Itemid = x.Itemid,
                                          Menuitemid = x.Menuitemid,
                                          Itemname = x.Itemname,
                                          Quantity = x.Quantity,
                                          Unitprice = x.Unitprice,
                                          Totalprice = x.Totalprice,
                                          DiscountAmount = x.DiscountAmount,
                                          TaxAmount = x.TaxAmount,
                                          Modifierdetails = x.Modifierdetails,
                                          Itemstatus = x.Itemstatus,
                                          Notes = x.Notes
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
