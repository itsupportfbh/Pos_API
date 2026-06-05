using Dapper;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Linq;
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
        private readonly IConfiguration _configuration;
        private readonly ICodeTemplateService _codeTemplateService;
        public OrderService(IUnitOfWork uow, POSContext poscontext,IConfiguration configuration,ICodeTemplateService codeTemplateService)
            {
                _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _context = poscontext;
            _configuration = configuration;
            _codeTemplateService = codeTemplateService;
        }



        public IEnumerable<object> GetAllOrderDetails(int orgId, int branchId)
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var orderList = _uow.GenericRepository<Orders>().Table()
                .Where(x => x.IsDeleted == false
                         && x.OrgId == orgId
                         && x.BranchId == branchId
                         && x.CreatedDate >= today
                         && x.CreatedDate < tomorrow)
                .OrderByDescending(x => x.Orderid)
                .ToList();

            var orderIds = orderList.Select(x => x.Orderid).ToList();

            var itemList = _uow.GenericRepository<Orderitems>().Table()
                .Where(x => x.IsDeleted == false
                         && x.OrgId == orgId
                         && orderIds.Contains(x.Orderid))
                .ToList();

            var diningTables = _uow.GenericRepository<DiningTableMaster>().Table()
                .Where(x => x.IsDeleted == false
                         && x.OrgId == orgId
                         && x.BranchId == branchId)
                .ToList();

            var result = orderList.Select(ord =>
            {
                var table = diningTables.FirstOrDefault(t => t.Id == ord.TableId);

                return new
                {
                    OrderId = ord.Orderid,
                    OrderNumber = ord.OrderNumber,
                    TableId = ord.TableId,
                    TableName = table != null ? table.Name : "",

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
        public Orders? GetById(int orderId)
        {
            var today = DateTime.Today;

            var result = _uow.GenericRepository<Orders>()
                .Table()
                .Where(x =>
                    x.Orderid == orderId &&
                    x.IsDeleted == false &&
                    x.CreatedDate.HasValue &&
                    x.CreatedDate.Value.Date == today)
                .Select(x => new Orders
                {
                    Orderid = x.Orderid,
                    OrderNumber = x.OrderNumber,
                    TableId = x.TableId,
                    FloorId = x.FloorId,
                    OrderType = x.OrderType,
                    OrderStatus = x.OrderStatus,
                    ItemCount = x.ItemCount,
                    SubtotalAmount = x.SubtotalAmount,
                    TaxAmount = x.TaxAmount,
                    DiscountAmount = x.DiscountAmount,
                    TotalAmount = x.TotalAmount,
                    CustomerName = x.CustomerName,
                    ContactNumber = x.ContactNumber,
                    Notes = x.Notes,
                    ShiftId = x.ShiftId,
                    OrgId = x.OrgId,
                    BranchId = x.BranchId,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    UpdatedDate = x.UpdatedDate,
                    UpdatedBy = x.UpdatedBy,

                    Items = x.Items
                        .Where(i => i.IsDeleted == false)
                        .Select(i => new Orderitems
                        {
                            Itemid = i.Itemid,
                            Orderid = i.Orderid,
                            Menuitemid = i.Menuitemid,
                            ComboMenuItemId = i.ComboMenuItemId,
                            Itemname = i.Itemname,
                            Quantity = i.Quantity,
                            Unitprice = i.Unitprice,
                            Totalprice = i.Totalprice,
                            DiscountAmount = i.DiscountAmount,
                            TaxAmount = i.TaxAmount,
                            Modifierdetails = i.Modifierdetails,
                            Itemstatus = i.Itemstatus,
                            Notes = i.Notes,
                            OrgId = i.OrgId
                        })
                        .ToList()
                })
                .FirstOrDefault();

            return result;
        }
        public string Delete(int orderId)
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


        public async Task<string> Create(Orders order)
        {
            if (order == null)
                return "InvalidOrder";

            if (order.EntityNo == 0)
                return "EntityNoRequired";

            if (order.OrgId == 0)
                return "OrgIdRequired";

            if (order.BranchId == 0)
                return "BranchIdRequired";

            if (order.Items == null || !order.Items.Any())
                return "NoItemsFound";

            bool isAutoGeneratedOrderNo = string.IsNullOrWhiteSpace(order.OrderNumber);

            string orderNumber = isAutoGeneratedOrderNo
                ? _codeTemplateService.GetLatestCode(order.EntityNo, order.OrgId, order.BranchId)
                : order.OrderNumber!;

            if (string.IsNullOrWhiteSpace(orderNumber))
                return "CodeTemplateNotFound";

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
            itemsTable.Columns.Add("Itemstatus", typeof(int));
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
                    item.Itemstatus?? 0,
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
                @Notes           = @Notes,
                @TableId         = @TableId,
                @FloorId         = @FloorId,
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
                @HoldOrderId     = @HoldOrderId,
                @CreatedBy       = @CreatedBy,
                @Items           = @Items",

                    new SqlParameter("@EntityNo", order.EntityNo),
                    new SqlParameter("@OrderNumber", orderNumber),
                    new SqlParameter("@Notes", string.IsNullOrWhiteSpace(order.Notes) ? "----" : order.Notes),
                    new SqlParameter("@TableId", order.TableId == 0 ? DBNull.Value : order.TableId),
                    new SqlParameter("@FloorId", order.FloorId == 0 ? DBNull.Value : order.FloorId),
                    new SqlParameter("@OrderType", string.IsNullOrWhiteSpace(order.OrderType) ? DBNull.Value : order.OrderType),
                    new SqlParameter("@OrderStatus",order.OrderStatus ==0?DBNull.Value : order.OrderStatus),
                    new SqlParameter("@Itemcount", order.ItemCount == 0 ? order.Items.Count : order.ItemCount),
                    new SqlParameter("@SubtotalAmount", order.SubtotalAmount ?? 0),
                    new SqlParameter("@TaxAmount", order.TaxAmount ?? 0),
                    new SqlParameter("@DiscountAmount", order.DiscountAmount ?? 0),
                    new SqlParameter("@TotalAmount", order.TotalAmount ?? 0),
                    new SqlParameter("@ContactNumber", string.IsNullOrWhiteSpace(order.ContactNumber) ? DBNull.Value : order.ContactNumber),
                    new SqlParameter("@CustomerName", string.IsNullOrWhiteSpace(order.CustomerName) ? DBNull.Value : order.CustomerName),
                    new SqlParameter("@ShiftId", order.ShiftId == 0 ? DBNull.Value : order.ShiftId),
                    new SqlParameter("@OrgId", order.OrgId),
                    new SqlParameter("@BranchId", order.BranchId),
                    new SqlParameter("@HoldOrderId", order.HoldOrderId == 0 ? DBNull.Value : order.HoldOrderId),
                    new SqlParameter("@CreatedBy", order.CreatedBy ?? (object)DBNull.Value),
                    itemsParam
                )
                .AsNoTracking()
                .ToListAsync();

            var result = list.FirstOrDefault();

            if (result == null)
                return "Failed";

            if (result.Result != "Success")
                return result.Result;

            if (isAutoGeneratedOrderNo)
            {
                var codeTemplate = _uow.GenericRepository<CodeTemplate>().Table()
                    .FirstOrDefault(x => x.EntityNo == order.EntityNo
                                      && x.OrgId == order.OrgId
                                      && x.BranchId == order.BranchId
                                      && x.IsMaster == true);

                if (codeTemplate != null)
                {
                    codeTemplate.CurrentValue = codeTemplate.CurrentValue + 1;
                    _uow.GenericRepository<CodeTemplate>().Update(codeTemplate);
                    _uow.Save();
                }
            }

            return result.OrderId?.ToString() ?? "Failed";
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
                    item.Itemid == 0 ? DBNull.Value : item.Itemid, // old row id only
                    item.Menuitemid == 0 ? DBNull.Value : item.Menuitemid,
                    item.ComboMenuItemId == 0 ? DBNull.Value : item.ComboMenuItemId,
                    item.Itemname ?? "",
                    item.Quantity,
                    item.Unitprice,
                    item.Totalprice,
                    item.DiscountAmount ?? 0,
                    item.TaxAmount ?? 0,
                    item.Modifierdetails ?? "",
                    item.Itemstatus == null ? DBNull.Value : item.Itemstatus,
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
                @FloorId         = @FloorId,
                @Notes           = @Notes,
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
                    new SqlParameter("@FloorId", order.FloorId == 0 ? DBNull.Value : order.FloorId),
                    new SqlParameter("@Notes", string.IsNullOrWhiteSpace(order.Notes) ? DBNull.Value : order.Notes),
                    new SqlParameter("@OrderType", order.OrderType ?? (object)DBNull.Value),
                    new SqlParameter("@OrderStatus", order.OrderStatus == 0 ? DBNull.Value : order.OrderStatus),
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

            var status = order.OrderStatus;
            var updatedBy = order.UpdatedBy ?? order.CreatedBy ?? 0;

            existingOrder.OrderStatus = status;
            existingOrder.UpdatedBy = updatedBy;
            existingOrder.UpdatedDate = DateTime.Now;

            _uow.GenericRepository<Orders>().Update(existingOrder);

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

            int status = orderItem.Itemstatus ?? 0;

            if (status <= 0)
                return "StatusRequired";

            int updatedBy = orderItem.UpdatedBy ?? orderItem.CreatedBy ?? 0;

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

            var allItems = _uow.GenericRepository<Orderitems>()
                .Table()
                .Where(x =>
                    x.Orderid == orderItem.Orderid &&
                    x.IsDeleted == false)
                .ToList();

            bool allSameStatus = allItems.Any() &&
                                 allItems.All(x => x.Itemstatus == status);

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



        public IEnumerable<object> GetAllDiningTable(int orgid, int branchid)
        {
            string fileUploadPathView = _configuration["AppSettings:FileUploadPathView"] ?? string.Empty;
            DateTime today = DateTime.Today;

            var diningTables =
                (from d in _uow.GenericRepository<DiningTableMaster>().Table()
                 join o in _uow.GenericRepository<Organization>().Table()
                     on d.OrgId equals o.Id
                 join b in _uow.GenericRepository<Branch>().Table()
                     on d.BranchId equals b.Id
                 join f in _uow.GenericRepository<FloorMaster>().Table()
                     on d.FloorId equals f.Id

                 where d.IsDeleted == false
                    && d.IsActive == true
                    && d.IsOccupied == false
                    && (orgid == 0 || d.OrgId == orgid)
                    && (branchid == 0 || d.BranchId == branchid)

                 orderby d.DisplayOrder

                 select new
                 {
                     id = d.Id,
                     organizationname = o.Name,
                     name = d.Name,
                     code = d.Code,
                     branchid = d.BranchId,
                     floorid = d.FloorId,
                     displayorder = d.DisplayOrder,
                     branchname = b.Name,
                     floorname = f.Name,
                     seatingsize = d.SeatingSize,
                     occupied = d.IsOccupied,
                     remarks = d.Remarks,
                     isactive = d.IsActive,

                     isreserved =
                         (from rtm in _uow.GenericRepository<ReservationTablesMapping>().Table()
                          join r in _uow.GenericRepository<Reservations>().Table()
                              on rtm.ReservationId equals r.Id
                          where rtm.TableId == d.Id
                             && rtm.IsDeleted == false
                             && r.IsDeleted == false
                             && r.ReservationDate.Date == today
                          select rtm.Id).Any(),

                     image = string.IsNullOrWhiteSpace(d.Image)
                         ? d.Image
                         : fileUploadPathView + "DiningTable/" + d.Image
                 }).ToList();

            return diningTables;
        }


        public IEnumerable<object> GetTopSixMenuAndComboMenu(int orgid, int branchid)
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var categories = _uow.GenericRepository<FoodMenuCategory>().Table()
                .Where(x => x.IsActive == true)
                .ToList();

            var subCategories = _uow.GenericRepository<FoodMenuSubCategory>().Table()
                .Where(x => x.IsActive == true)
                .ToList();

            var foodMenusRaw = _uow.GenericRepository<FoodMenu>().Table()
                .Where(x => x.IsDeleted == false
                         && x.IsActive == true
                         && x.OrgId == orgid)
                .ToList();

            var comboMenusRaw = _uow.GenericRepository<ComboMenu>().Table()
                .Where(x => x.IsDeleted == false
                         && x.IsActive == true
                         && x.OrgId == orgid
                         && x.BranchId == branchid)
                .ToList();

            var comboMenuItems = _uow.GenericRepository<ComboMenuItem>().Table()
                .Where(x => x.IsDeleted == false && x.IsActive == true)
                .ToList();

            var todayOrderIds = _uow.GenericRepository<Orders>().Table()
                .Where(o => o.IsDeleted == false
                    && o.CreatedDate >= today
                    && o.CreatedDate < tomorrow
                    && (orgid == 0 || o.OrgId == orgid)
                    && (branchid == 0 || o.BranchId == branchid))
                .Select(o => o.Orderid)
                .ToList();

            var orderItems = _uow.GenericRepository<Orderitems>().Table()
                .Where(oi => todayOrderIds.Contains(oi.Orderid))
                .ToList();

            var topMenuIds = orderItems
                .Where(x => x.Menuitemid != null && x.Menuitemid > 0)
                .GroupBy(x => x.Menuitemid)
                .Select(g => new
                {
                    MenuId = g.Key.Value,
                    Qty = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.Qty)
                .Take(6)
                .Select(x => x.MenuId)
                .ToList();

            var topComboIds = orderItems
                .Where(x => x.ComboMenuItemId != null && x.ComboMenuItemId > 0)
                .GroupBy(x => x.ComboMenuItemId)
                .Select(g => new
                {
                    ComboMenuId = g.Key.Value,
                    Qty = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.Qty)
                .Take(6)
                .Select(x => x.ComboMenuId)
                .ToList();

            var result = new List<object>();

            var foodMenus = foodMenusRaw
                .Where(x => topMenuIds.Contains(x.Id))
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    name = x.Name,
                    price = x.Price,
                    orgid = x.OrgId,
                    branchid = branchid,
                    categoryId = x.CategoryId,
                    categoryName = categories.FirstOrDefault(c => c.Id == x.CategoryId)?.Name ?? "",
                    subCategoryId = x.SubCategoryId,
                    subCategoryName = subCategories.FirstOrDefault(sc => sc.Id == x.SubCategoryId)?.Name ?? "",
                    isactive = x.IsActive,
                    type = "Menu",
                    comboMenuItems = new List<object>(),
                    itemsCount = 0
                })
                .Cast<object>()
                .ToList();

            var comboMenus = comboMenusRaw
                .Where(x => topComboIds.Contains(x.Id))
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    name = x.Name,
                    price = x.Price,
                    orgid = x.OrgId,
                    branchid = x.BranchId,
                    categoryId = x.CategoryId,
                    categoryName = categories.FirstOrDefault(c => c.Id == x.CategoryId)?.Name ?? "",
                    subCategoryId = x.SubCategoryId,
                    subCategoryName = subCategories.FirstOrDefault(sc => sc.Id == x.SubCategoryId)?.Name ?? "",
                    isactive = x.IsActive,
                    type = "ComboMenu",
                    comboMenuItems = comboMenuItems
                        .Where(i => i.ComboMenuId == x.Id)
                        .Select(i => new
                        {
                            id = i.Id,
                            comboMenuId = i.ComboMenuId,
                            foodMenuId = i.FoodMenuId,
                            foodMenuName = foodMenusRaw.FirstOrDefault(f => f.Id == i.FoodMenuId)?.Name ?? "",
                            qty = i.Qty,
                            isactive = i.IsActive
                        })
                        .Cast<object>()
                        .ToList(),
                    itemsCount = comboMenuItems.Count(i => i.ComboMenuId == x.Id)
                })
                .Cast<object>()
                .ToList();

            result.AddRange(foodMenus);
            result.AddRange(comboMenus);

            result = result.Take(6).ToList();

            if (result.Count < 6)
            {
                var alreadyMenuIds = topMenuIds.ToList();
                var needCount = 6 - result.Count;

                var defaultMenus = foodMenusRaw
                    .Where(x => !alreadyMenuIds.Contains(x.Id))
                    .OrderBy(x => x.Id)
                    .Take(needCount)
                    .Select(x => new
                    {
                        id = x.Id,
                        code = x.Code,
                        name = x.Name,
                        price = x.Price,
                        orgid = x.OrgId,
                        branchid = branchid,
                        categoryId = x.CategoryId,
                        categoryName = categories.FirstOrDefault(c => c.Id == x.CategoryId)?.Name ?? "",
                        subCategoryId = x.SubCategoryId,
                        subCategoryName = subCategories.FirstOrDefault(sc => sc.Id == x.SubCategoryId)?.Name ?? "",
                        isactive = x.IsActive,
                        type = "Menu",
                        comboMenuItems = new List<object>(),
                        itemsCount = 0
                    })
                    .Cast<object>()
                    .ToList();

                result.AddRange(defaultMenus);
            }

            return result.Take(6).ToList();
        }

        public async Task<IEnumerable<object>> GetAllMenuAndComboMenu(  int orgid, int branchid,int? categoryId, int? subCategoryId,string? searchKey)
        {
            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            var result = await connection.QueryAsync<object>(
                "dbo.sp_GetAllMenuAndComboMenu",
                new
                {
                    OrgId = orgid,
                    BranchId = branchid,
                    CategoryId = categoryId,
                    SubCategoryId = subCategoryId,
                    SearchKey = searchKey ?? ""
                },
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
    }
}
