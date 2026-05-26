using Microsoft.AspNetCore.Razor.TagHelpers;
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

            // ✅ Dining table list
            var diningTables = _uow.GenericRepository<DiningTableMaster>().Table()
                .Where(x => x.IsDeleted == false
                         && x.OrgId == org
                         && x.BranchId == branch)
                .ToList();

            var result = orderList.Select(ord =>
            {
                var table = diningTables.FirstOrDefault(t => t.Id == ord.TableId);

                return new
                {
                    OrderId = ord.Orderid,
                    OrderNumber = ord.OrderNumber,

                    TableId = ord.TableId,
                    TableName = table != null ? table.Name : "", // ✅ added

                    OrderType = ord.OrderType,
                    OrderStatus = ord.OrderStatus,

                    ItemCount = ord.ItemCount ?? 0,

                    SubtotalAmount = ord.SubtotalAmount ?? 0,
                    TaxAmount = ord.TaxAmount ?? 0,
                    DiscountAmount = ord.DiscountAmount ?? 0,
                    TotalAmount = ord.TotalAmount ?? 0,
                    CustomerName = ord.CustomerName,
                    ContactNumber = ord.ContactNumber,
                    ShiftId = ord.ShiftId,
                    OrganizationId = ord.OrgId,
                    Notes = ord.Notes,
                    BranchId = ord.BranchId,
                    CreatedBy = ord.CreatedBy,
                    CreatedDate = ord.CreatedDate,

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
                };
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

            int currentValue = template.CurrentValue;
            int startValue = template.StartValue;

            int nextValue = currentValue > 0
                ? currentValue + 1
                : startValue;

            string prefix = template.Prefix ?? "";
            string suffix = template.Suffix ?? "";
            int noOfDigit = template.NoOfDigit;

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
                @HoldOrderId = @HoldOrderId,
                @Notes         = @Notes,
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
                    new SqlParameter("@HoldOrderId", order.HoldOrderId),
                     new SqlParameter("@Notes", string.IsNullOrWhiteSpace(order.Notes) ? "----" : order.Notes),
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

        public async Task<string> Update(Orders order)
        {
            if (order == null)
                return "InvalidOrder";

            if (order.Orderid == 0)
                return "OrderIdRequired";

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
                    @"EXEC dbo.sp_Order_Update
                @OrderId         = @OrderId,
                @TableId         = @TableId,
                @Notes         = @Notes,
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
                @UpdatedBy       = @UpdatedBy,
                @Items           = @Items",
                  
                    new SqlParameter("@OrderId", order.Orderid),
                    new SqlParameter("@TableId", order.TableId == 0 ? DBNull.Value : order.TableId),
                    new SqlParameter("@OrderType", order.OrderType ?? (object)DBNull.Value),
                    new SqlParameter("@OrderStatus", string.IsNullOrWhiteSpace(order.OrderStatus) ? "In Kitchen" : order.OrderStatus),
                    new SqlParameter("@Notes", string.IsNullOrWhiteSpace(order.Notes) ? "----" : order.Notes),
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
                    new SqlParameter("@UpdatedBy", order.UpdatedBy ?? order.CreatedBy ?? (object)DBNull.Value),
                    itemsParam
                )
                .AsNoTracking()
                .ToListAsync();

            var result = list.FirstOrDefault();

            if (result == null)
                return "Failed";

            return result.Result == "Success"
                ? result.OrderId?.ToString() ?? order.Orderid.ToString()
                : result.Result;
        }


        
        


        public string StatusChange(Orders order)
        {
            if (order == null)
                return "InvalidOrder";

            if (order.Orderid <= 0)
                return "OrderIdRequired";

            var existingOrder = _uow.GenericRepository<Orders>()
                .Table()
                .FirstOrDefault(x =>
                    x.Orderid == order.Orderid &&
                    x.IsDeleted == false);

            if (existingOrder == null)
                return "OrderNotFound";

            var status = string.IsNullOrWhiteSpace(order.OrderStatus)
                ? "In Kitchen"
                : order.OrderStatus.Trim();

            var updatedBy = order.UpdatedBy ?? order.CreatedBy ?? 0;

            // Header update
            existingOrder.OrderStatus = status;
            existingOrder.UpdatedBy = updatedBy;
            existingOrder.UpdatedDate = DateTime.Now;

            _uow.GenericRepository<Orders>().Update(existingOrder);

            // Item update
            var items = _uow.GenericRepository<Orderitems>()
                .Table()
                .Where(x =>
                    x.Orderid == existingOrder.Orderid &&
                    x.IsDeleted == false)
                .ToList();

            foreach (var item in items)
            {
                item.Itemstatus = status;
                item.UpdatedBy = updatedBy;
                item.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<Orderitems>().Update(item);
            }

            _uow.Save();

            return existingOrder.Orderid.ToString();
        }


        public string KitchenItemStatusChange(Orderitems orderItem)
        {
            if (orderItem == null)
                return "InvalidItem";

            if (orderItem.Orderid <= 0)
                return "OrderIdRequired";

            if (orderItem.Itemid <= 0)
                return "ItemIdRequired";

            var status = string.IsNullOrWhiteSpace(orderItem.Itemstatus)
                ? "Preparing"
                : orderItem.Itemstatus.Trim();

            var updatedBy = orderItem.UpdatedBy ?? orderItem.CreatedBy ?? 0;

            var existingItem = _uow.GenericRepository<Orderitems>()
                .Table()
                .FirstOrDefault(x =>
                    x.Orderid == orderItem.Orderid &&
                    x.Itemid == orderItem.Itemid &&
                    x.IsDeleted == false);

            if (existingItem == null)
                return "ItemNotFound";

            existingItem.Itemstatus = status;
            existingItem.UpdatedBy = updatedBy;
            existingItem.UpdatedDate = DateTime.Now;

            _uow.GenericRepository<Orderitems>().Update(existingItem);

            // ✅ Check all active items in this order
            var allItems = _uow.GenericRepository<Orderitems>()
                .Table()
                .Where(x =>
                    x.Orderid == orderItem.Orderid &&
                    x.IsDeleted == false)
                .ToList();

            bool allSameStatus = allItems.Any() &&
                                 allItems.All(x =>
                                     string.Equals(
                                         x.Itemstatus?.Trim(),
                                         status,
                                         StringComparison.OrdinalIgnoreCase));

            if (allSameStatus)
            {
                var existingOrder = _uow.GenericRepository<Orders>()
                    .Table()
                    .FirstOrDefault(x =>
                        x.Orderid == orderItem.Orderid &&
                        x.IsDeleted == false);

                if (existingOrder != null)
                {
                    existingOrder.OrderStatus = status;
                    existingOrder.UpdatedBy = updatedBy;
                    existingOrder.UpdatedDate = DateTime.Now;

                    _uow.GenericRepository<Orders>().Update(existingOrder);
                }
            }

            _uow.Save();

            return existingItem.Itemid.ToString();
        }
    }
}
