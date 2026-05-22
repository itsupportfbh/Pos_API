using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using UNITYPOS_API.Common;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.Context;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class OrderService : IOrderService
    {

       
            private readonly IUnitOfWork _uow;
        private readonly POSContext _context;
        public OrderService(IUnitOfWork uow, POSContext poscontext)
            {
                _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _context = poscontext;
        }



        public IEnumerable<object> GetAllOrderDetails(long orgId, long branchId)
        {
            int org = Convert.ToInt32(orgId);
            int branch = Convert.ToInt32(branchId);

            var orderList = _uow.GenericRepository<Orders>().Table()
                .Where(x => x.IsDeleted == false
                         && x.OrgId == org
                         && x.BranchId == branch)
                .OrderByDescending(x => x.Orderid)
                .ToList();

            var orderIds = orderList.Select(x => x.Orderid).ToList();

            var itemList = _uow.GenericRepository<Orderitems>().Table()
                .Where(x => x.IsDeleted == false
                         && x.OrgId == org
                         && orderIds.Contains(x.Orderid))
                .ToList();

            var result = orderList.Select(ord => new
            {
                OrderId = ord.Orderid,
                OrderNumber = ord.OrderNumber,
                TableId = ord.TableId,
                OrderType = ord.OrderType,
                OrderStatus = ord.OrderStatus,

                ItemCount = ord.ItemCount ?? 0,
                
                SubtotalAmount = ord.SubtotalAmount ?? 0,
                TaxAmount = ord.TaxAmount ?? 0,
                DiscountAmount = ord.DiscountAmount ?? 0,
                TotalAmount = ord.TotalAmount ?? 0,
                CustomerName=ord.CustomerName,
                ContactNumber=ord.ContactNumber,
                ShiftId = ord.ShiftId,
                OrganizationId = ord.OrgId,
                BranchId = ord.BranchId,

                Items = itemList
                    .Where(item => item.Orderid == ord.Orderid)
                    .Select(item => new
                    {
                        Itemid = item.Itemid,
                        Orderid = item.Orderid,

                        Menuitemid = item.Menuitemid,
                        ComboMenuItemId = item.ComboMenuItemId,

                        Itemname = item.Itemname,

                        Quantity = item.Quantity,
                        Unitprice = item.Unitprice,
                        Totalprice = item.Totalprice,

                        DiscountAmount = item.DiscountAmount ?? 0,
                        TaxAmount = item.TaxAmount ?? 0,

                        Modifierdetails = item.Modifierdetails,
                        Itemstatus = item.Itemstatus,
                        Notes = item.Notes,

                        OrgId = item.OrgId
                    })
                    .ToList()
            }).ToList();

            return result;
        }
        public Orders? GetById(long orderId)
            {
                var result = _uow.GenericRepository<Orders>()
                    .Table()
                    .FirstOrDefault(x => x.Orderid== orderId
                                      && x.IsDeleted == false);

                return result;
            }

        public string Delete(long orderId)
            {
                var result = _uow.GenericRepository<Orders>()
                    .Table()
                    .FirstOrDefault(x => x.Orderid == orderId);

                if (result != null)
                {
                    result.IsDeleted = true;

                    _uow.GenericRepository<Orders>().Update(result);
                    _uow.Save();
                }

                return Convert.ToString(result?.Orderid ?? 0);
            }


        //public string Create(Orders order)
        //{
        //    try
        //    {
        //        if (order == null)
        //            return "InvalidOrder";

        //        if (order.Items == null || !order.Items.Any())
        //            return "NoItemsFound";

        //        string newOrderNo = !string.IsNullOrWhiteSpace(order.OrderNumber) ? order.OrderNumber : GenerateOrderNumber(order.OrgId, order.BranchId);

        //        var entity = new Orders
        //        {
        //            // OrderNumber = newOrderNo,
        //            OrderNumber = order.OrderNumber,
        //            TableId = order.TableId,
        //            OrderType = order.OrderType,
        //            OrderStatus = string.IsNullOrWhiteSpace(order.OrderStatus)
        //                ? "In Kitchen"
        //                : order.OrderStatus,

        //            ItemCount = order.Items.Count,

        //            SubtotalAmount = order.SubtotalAmount ?? 0,
        //            TaxAmount = order.TaxAmount ?? 0,
        //            DiscountAmount = order.DiscountAmount ?? 0,
        //            TotalAmount = order.TotalAmount ?? 0,
        //            CustomerName = order.CustomerName,
        //            ContactNumber = order.ContactNumber,
        //            ShiftId = order.ShiftId,
        //            OrgId = order.OrgId,
        //            BranchId = order.BranchId,

        //            IsDeleted = false,
        //            CreatedBy = order.CreatedBy,
        //            CreatedDate = DateTime.Now
        //        };

        //        _uow.GenericRepository<Orders>().Insert(entity);
        //        _uow.Save();

        //        foreach (var item in order.Items)
        //        {
        //            var orderItem = new Orderitems
        //            {
        //                Orderid = entity.Orderid,
        //                Menuitemid = item.Menuitemid,
        //                ComboMenuItemId = item.ComboMenuItemId,
        //                Itemname = item.Itemname,

        //                Quantity = item.Quantity,
        //                Unitprice = item.Unitprice,
        //                Totalprice = item.Totalprice > 0
        //                    ? item.Totalprice
        //                    : item.Quantity * item.Unitprice,

        //                DiscountAmount = item.DiscountAmount ?? 0,
        //                TaxAmount = item.TaxAmount ?? 0,
        //                Modifierdetails = item.Modifierdetails,

        //                Itemstatus = "In Kitchen",
        //                Notes = item.Notes,

        //                OrgId = item.Itemid > 0 ? item.OrgId : order.OrgId,

        //                IsDeleted = false,
        //                CreatedBy = order.CreatedBy,
        //                CreatedDate = DateTime.Now
        //            };

        //            _uow.GenericRepository<Orderitems>().Insert(orderItem);
        //        }

        //        _uow.Save();

        //        DeleteHoldOrderPermanently(entity);

        //        return entity.Orderid.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Database save failed: " + ex.Message;
        //    }
        //}
        //private void DeleteHoldOrderPermanently(Orders order)
        //{
        //    var holdOrder = _uow.GenericRepository<OrdersHold>().Table()
        //        .FirstOrDefault(x => x.Ordernumber == order.OrderNumber
        //                          && x.OrgId == order.OrgId
        //                          && x.BranchId == order.BranchId
        //                          && x.IsDeleted == false);

        //    if (holdOrder == null)
        //        return;

        //    var holdItems = _uow.GenericRepository<OrderHoldItems>().Table()
        //        .Where(x => x.Orderid == holdOrder.OrderId
        //                 && x.OrgId == order.OrgId)
        //        .ToList();

        //    foreach (var item in holdItems)
        //    {
        //        _uow.GenericRepository<OrderHoldItems>().Delete(item);
        //    }

        //    _uow.GenericRepository<OrdersHold>().Delete(holdOrder);

        //    _uow.Save();
        //}

      //public  Task<string> Update(Orders orders)
      //  {
      //      try
      //      {
      //          if (order == null || order.Orderid <= 0)
      //              return "InvalidOrder";

      //          if (order.Items == null || !order.Items.Any())
      //              return "NoItemsFound";

      //          var now = DateTime.Now;

      //          var existingOrder = _uow.GenericRepository<Orders>().Table()
      //              .FirstOrDefault(x => x.Orderid == order.Orderid
      //                                && x.OrgId == order.OrgId
      //                                && x.BranchId == order.BranchId
      //                                && x.IsDeleted == false);

      //          if (existingOrder == null)
      //              return "OrderNotFound";

      //          // Header update
      //          existingOrder.TableId = order.TableId;
      //          existingOrder.OrderType = order.OrderType;
      //          existingOrder.OrderStatus = "In Kitchen";
      //          existingOrder.ItemCount = order.Items.Count;
      //          existingOrder.SubtotalAmount = order.SubtotalAmount ?? 0;
      //          existingOrder.TaxAmount = order.TaxAmount ?? 0;
      //          existingOrder.DiscountAmount = order.DiscountAmount ?? 0;
      //          existingOrder.TotalAmount = order.TotalAmount ?? 0;
      //          existingOrder.ShiftId = order.ShiftId;
      //          existingOrder.UpdatedBy = order.CreatedBy;
      //          existingOrder.UpdatedDate = now;
      //          existingOrder.ContactNumber= order.ContactNumber;
      //          existingOrder.CustomerName = order.CustomerName;

      //          _uow.GenericRepository<Orders>().Update(existingOrder);

      //          var existingItems = _uow.GenericRepository<Orderitems>().Table()
      //              .Where(x => x.Orderid == existingOrder.Orderid
      //                       && x.OrgId == order.OrgId
      //                       && x.IsDeleted == false)
      //              .ToList();

      //          var incomingItems = order.Items ?? new List<Orderitems>();

      //          foreach (var item in incomingItems)
      //          {
      //              var oldItem = existingItems.FirstOrDefault(x =>
      //                  x.Menuitemid == item.Menuitemid &&
      //                  (x.ComboMenuItemId ?? 0) == (item.ComboMenuItemId ?? 0) &&
      //                  x.IsDeleted == false);

      //              if (oldItem != null)
      //              {
      //                  // Existing item update
      //                  oldItem.Menuitemid = item.Menuitemid;
      //                  oldItem.ComboMenuItemId = item.ComboMenuItemId;
      //                  oldItem.Itemname = item.Itemname;
      //                  oldItem.Quantity = item.Quantity;
      //                  oldItem.Unitprice = item.Unitprice;
      //                  oldItem.Totalprice = item.Totalprice > 0
      //                      ? item.Totalprice
      //                      : item.Quantity * item.Unitprice;

      //                  oldItem.DiscountAmount = item.DiscountAmount ?? 0;
      //                  oldItem.TaxAmount = item.TaxAmount ?? 0;
      //                  oldItem.Modifierdetails = item.Modifierdetails;
      //                  oldItem.Itemstatus = "In Kitchen";
      //                  oldItem.Notes = item.Notes;

      //                  oldItem.OrgId = order.OrgId;
      //                  oldItem.IsDeleted = false;
      //                  oldItem.UpdatedBy = order.CreatedBy;
      //                  oldItem.UpdatedDate = now;

      //                  _uow.GenericRepository<Orderitems>().Update(oldItem);
      //              }
      //              else
      //              {
      //                  // New item insert
      //                  var newItem = new Orderitems
      //                  {
      //                      Orderid = existingOrder.Orderid,
      //                      Menuitemid = item.Menuitemid,
      //                      ComboMenuItemId = item.ComboMenuItemId,
      //                      Itemname = item.Itemname,
      //                      Quantity = item.Quantity,
      //                      Unitprice = item.Unitprice,
      //                      Totalprice = item.Totalprice > 0
      //                          ? item.Totalprice
      //                          : item.Quantity * item.Unitprice,

      //                      DiscountAmount = item.DiscountAmount ?? 0,
      //                      TaxAmount = item.TaxAmount ?? 0,
      //                      Modifierdetails = item.Modifierdetails,
      //                      Itemstatus = "In Kitchen",
      //                      Notes = item.Notes,

      //                      OrgId = order.OrgId,
      //                      IsDeleted = false,
      //                      CreatedBy = order.CreatedBy,
      //                      CreatedDate = now
      //                  };

      //                  _uow.GenericRepository<Orderitems>().Insert(newItem);
      //              }
      //          }

      //          // Soft delete removed items
      //          var incomingKeys = incomingItems
      //              .Select(x => new
      //              {
      //                  x.Menuitemid,
      //                  ComboMenuItemId = x.ComboMenuItemId ?? 0
      //              })
      //              .ToList();

      //          var removedItems = existingItems
      //              .Where(old => old.IsDeleted == false &&
      //                  !incomingKeys.Any(newItem =>
      //                      newItem.Menuitemid == old.Menuitemid &&
      //                      newItem.ComboMenuItemId == (old.ComboMenuItemId ?? 0)))
      //              .ToList();

      //          foreach (var removed in removedItems)
      //          {
      //              removed.IsDeleted = true;
      //              removed.UpdatedBy = order.CreatedBy;
      //              removed.UpdatedDate = now;

      //              _uow.GenericRepository<Orderitems>().Update(removed);
      //          }

      //          _uow.Save();

      //          //DeleteHoldOrderPermanently(order);

      //          return "Updated";
      //      }
      //      catch (Exception ex)
      //      {
      //          return ex.Message;
      //      }
      //  }
      

        public string GenerateOrderNumber(int orgId, int branchId)
        {
            var template = _uow.GenericRepository<CodeTemplate>().Table()
                .Where(x => x.Name == "Order Screen"
                         && x.IsActive == true
                         && x.IsDeleted == false
                         && x.OrgId == orgId
                         && x.BranchId == branchId)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            if (template == null)
                throw new Exception("CodeTemplate not found for Order Screen");

            int currentValue = template.CurrentValue ;
            int startValue = template.StartValue ;

            int nextValue = currentValue > 0
                ? currentValue + 1
                : startValue;

            string prefix = template.Prefix ?? "";
            string suffix = template.Suffix ?? "";
            int noOfDigit = template.NoOfDigit ;

            string orderNo = prefix + nextValue.ToString().PadLeft(noOfDigit, '0') + suffix;

            template.CurrentValue = nextValue;
           //template.UpdatedBy = template.us
            template.UpdatedDate = DateTime.Now;

            _uow.GenericRepository<CodeTemplate>().Update(template);
            _uow.Save();

            return orderNo;
        }


        public async Task<string> Create(Orders order)
        {
            if (order == null)
                return "InvalidOrder";

            if (order.EntityNo == 0)
                return "EntityNoRequired";

            if (order.Items == null || !order.Items.Any())
                return "NoItemsFound";

            var itemsTable = new DataTable();
            itemsTable.Columns.Add("Itemid", typeof(int));
            itemsTable.Columns.Add("Menuitemid", typeof(int));
            itemsTable.Columns.Add("ComboMenuItemId", typeof(int));
            itemsTable.Columns.Add("Itemname", typeof(string));
            itemsTable.Columns.Add("Quantity", typeof(decimal));
            itemsTable.Columns.Add("Unitprice", typeof(decimal));
            itemsTable.Columns.Add("Totalprice", typeof(decimal));
            itemsTable.Columns.Add("DiscountAmount", typeof(decimal));
            itemsTable.Columns.Add("TaxAmount", typeof(decimal));
            itemsTable.Columns.Add("Modifierdetails", typeof(string));
            itemsTable.Columns.Add("Itemstatus", typeof(string));
            itemsTable.Columns.Add("Notes", typeof(string));

            foreach (var item in order.Items)
            {
                itemsTable.Rows.Add(
                    item.Itemid == 0 ? DBNull.Value : item.Itemid,
                    item.Menuitemid == 0 ? DBNull.Value : item.Menuitemid,
                    item.ComboMenuItemId == 0 ? DBNull.Value : item.ComboMenuItemId,
                    item.Itemname ?? "",
                    item.Quantity,
                    item.Unitprice,
                    item.Totalprice,
                    item.DiscountAmount ?? 0,
                    item.TaxAmount ?? 0,
                    item.Modifierdetails ?? "",
                    item.Itemstatus ?? "In Kitchen",
                    item.Notes ?? ""
                );
            }

            var itemsParam = new SqlParameter("@Items", SqlDbType.Structured)
            {
                TypeName = "dbo.OrderItemType",
                Value = itemsTable
            };

            var list = await _context.Set<Response>()
                .FromSqlRaw(
                    @"EXEC dbo.sp_Order_Create
                @EntityNo        = @EntityNo,
                @OrderNumber     = @OrderNumber,
                @TableId         = @TableId,
                @OrderType       = @OrderType,
                @OrderStatus     = @OrderStatus,
                @Itemcount       = @Itemcount,
                @SubtotalAmount  = @SubtotalAmount,
                @TaxAmount       = @TaxAmount,
                @DiscountAmount  = @DiscountAmount,
                @TotalAmount     = @TotalAmount,
                @ContactNumber   = @ContactNumber,
                @CustomerName    = @CustomerName,
                @ShiftId         = @ShiftId,
                @OrgId           = @OrgId,
                @BranchId        = @BranchId,
                @CreatedBy       = @CreatedBy,
                @Items           = @Items",
                    new SqlParameter("@EntityNo", order.EntityNo),
                    new SqlParameter("@OrderNumber", string.IsNullOrWhiteSpace(order.OrderNumber) ? DBNull.Value : order.OrderNumber),
                    new SqlParameter("@TableId", order.TableId == 0 ? DBNull.Value : order.TableId),
                    new SqlParameter("@OrderType", order.OrderType ?? (object)DBNull.Value),
                    new SqlParameter("@OrderStatus", string.IsNullOrWhiteSpace(order.OrderStatus) ? "In Kitchen" : order.OrderStatus),
                    new SqlParameter("@Itemcount", order.ItemCount == 0 ? order.Items.Count : order.ItemCount),
                    new SqlParameter("@SubtotalAmount", order.SubtotalAmount ?? 0),
                    new SqlParameter("@TaxAmount", order.TaxAmount ?? 0),
                    new SqlParameter("@DiscountAmount", order.DiscountAmount ?? 0),
                    new SqlParameter("@TotalAmount", order.TotalAmount ?? 0),
                    new SqlParameter("@ContactNumber", order.ContactNumber ?? (object)DBNull.Value),
                    new SqlParameter("@CustomerName", order.CustomerName ?? (object)DBNull.Value),
                    new SqlParameter("@ShiftId", order.ShiftId == 0 ? DBNull.Value : order.ShiftId),
                    new SqlParameter("@OrgId", order.OrgId),
                    new SqlParameter("@BranchId", order.BranchId),
                    new SqlParameter("@CreatedBy", order.CreatedBy ?? (object)DBNull.Value),
                    itemsParam
                )
                .AsNoTracking()
                .ToListAsync();

            var result = list.FirstOrDefault();

            if (result == null)
                return "Failed";

            return result.Result == "Success"
                ? result.OrderId?.ToString() ?? "Failed"
                : result.Result;
        }

        public Task<string> Update(Orders orders)
        {
            throw new NotImplementedException();
        }
    }
}
