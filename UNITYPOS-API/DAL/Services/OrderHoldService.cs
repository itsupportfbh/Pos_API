using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.Context;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities;
using Microsoft.Data.SqlClient;
using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.Common;
namespace UNITYPOS_API.DAL.Services
{
    public class OrderHoldService : IOrderHoldService
    {
        private readonly IUnitOfWork _uow;
        private readonly IOrderService _orderService;
        private readonly ICodeTemplateService _codeTemplateService;
        private readonly IConfiguration _configuration;
        private readonly POSContext _context;
        public OrderHoldService(IUnitOfWork uow, IOrderService orderService,POSContext poscontext, ICodeTemplateService codeTemplateService,IConfiguration configuration)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _codeTemplateService = codeTemplateService;
            _configuration = configuration;
            _context = poscontext;
        }

        //public string Create(OrdersHold ordershold)
        //{
        //    if (ordershold == null)
        //        return "InvalidOrder";

        //    if (ordershold.Items == null || !ordershold.Items.Any())
        //        return "NoItemsFound";

        //    var exists = _uow.GenericRepository<OrdersHold>().Table()
        //        .Any(o => o.Ordernumber.ToLower() == ordershold.Ordernumber.ToLower()
        //               && o.OrgId == ordershold.OrgId
        //               && o.BranchId == ordershold.BranchId
        //               && o.IsDeleted == false);

        //    if (exists)
        //        return "AlreadyExists";

        //    var now = DateTime.Now;
        //    string Code = _codeTemplateService.GetLatestCode(ordershold.EntityNo, ordershold.OrgId, 0);

        //    var entity = new OrdersHold
        //    {
        //       // Ordernumber = newOrderNo,
        //        Ordernumber = ordershold.Ordernumber,
        //        Tableid = ordershold.Tableid,
        //        Ordertype = ordershold.Ordertype,
        //        Orderstatus = string.IsNullOrWhiteSpace(ordershold.Orderstatus) ? "OPEN" : ordershold.Orderstatus,

        //        Itemcount = ordershold.Items.Count,
        //        SubtotalAmount = ordershold.SubtotalAmount ?? 0,
        //        TaxAmount = ordershold.TaxAmount ?? 0,
        //        DiscountAmount = ordershold.DiscountAmount ?? 0,
        //        TotalAmount = ordershold.TotalAmount ?? 0,
        //        ContactNumber=ordershold.ContactNumber,
        //        CustomerName=ordershold.CustomerName,
        //        Shiftid = ordershold.Shiftid,
        //        OrgId = ordershold.OrgId,
        //        BranchId = ordershold.BranchId,

        //        IsDeleted = false,
        //        CreatedBy = ordershold.CreatedBy,
        //        CreatedDate = now
        //    };

        //    _uow.GenericRepository<OrdersHold>().Insert(entity);
        //    _uow.Save();

        //    foreach (var item in ordershold.Items)
        //    {
        //        var orderItem = new OrderHoldItems
        //        {
        //            Orderid = entity.OrderId,
        //            Menuitemid = item.Menuitemid,
        //            ComboMenuItemId = item.ComboMenuItemId,
        //            Itemname = item.Itemname,
        //            Quantity = item.Quantity,
        //            Unitprice = item.Unitprice,
        //            Totalprice = item.Totalprice > 0 ? item.Totalprice : item.Quantity * item.Unitprice,

        //            DiscountAmount = item.DiscountAmount ?? 0,
        //            TaxAmount = item.TaxAmount ?? 0,
        //            Modifierdetails = item.Modifierdetails,
        //            Itemstatus = string.IsNullOrWhiteSpace(item.Itemstatus) ? "OPEN" : item.Itemstatus,
        //            Notes = item.Notes,

        //            OrgId = item.OrgId > 0 ? item.OrgId : ordershold.OrgId,
        //            IsDeleted = false,
        //            CreatedBy = ordershold.CreatedBy,
        //            CreatedDate = now
        //        };

        //        _uow.GenericRepository<OrderHoldItems>().Insert(orderItem);
        //    }


        //    var codeTemplate = _uow.GenericRepository<CodeTemplate>().Table()
        //                       .FirstOrDefault(x => x.EntityNo == ordershold.EntityNo
        //                                         && x.OrgId == ordershold.OrgId
        //                                         && (x.BranchId == 0 || x.BranchId == ordershold.BranchId));
        //                                        // && x.IsMaster == true);

        //    if (codeTemplate != null)
        //    {
        //        codeTemplate.CurrentValue = codeTemplate.CurrentValue + 1;
        //        _uow.GenericRepository<CodeTemplate>().Update(codeTemplate);
        //    }




        //    _uow.Save();

        //    return entity.OrderId.ToString();
        //}




        //public string Update(OrdersHold ordershold)
        //{
        //    if (ordershold == null || ordershold.OrderId <= 0)
        //        return "InvalidOrder";

        //    if (ordershold.Items == null || !ordershold.Items.Any())
        //        return "NoItemsFound";

        //    var exists = _uow.GenericRepository<OrdersHold>().Table()
        //        .Any(o => o.Ordernumber.ToLower() == ordershold.Ordernumber.ToLower()
        //               && o.OrderId != ordershold.OrderId
        //               && o.OrgId == ordershold.OrgId
        //               && o.BranchId == ordershold.BranchId
        //               && o.IsDeleted == false);

        //    if (exists)
        //        return "AlreadyExists";

        //    var existingOrder = _uow.GenericRepository<OrdersHold>().Table()
        //        .FirstOrDefault(x => x.OrderId == ordershold.OrderId
        //                          && x.OrgId == ordershold.OrgId
        //                          && x.BranchId == ordershold.BranchId
        //                          && x.IsDeleted == false);

        //    if (existingOrder == null)
        //        return "OrderNotFound";

        //    var now = DateTime.Now;

        //    existingOrder.Tableid = ordershold.Tableid;
        //    existingOrder.Ordertype = ordershold.Ordertype;
        //    existingOrder.Orderstatus = string.IsNullOrWhiteSpace(ordershold.Orderstatus) ? "Hold" : ordershold.Orderstatus;

        //    existingOrder.Itemcount = ordershold.Items.Count;
        //    existingOrder.SubtotalAmount = ordershold.SubtotalAmount ?? 0;
        //    existingOrder.TaxAmount = ordershold.TaxAmount ?? 0;
        //    existingOrder.DiscountAmount = ordershold.DiscountAmount ?? 0;
        //    existingOrder.TotalAmount = ordershold.TotalAmount ?? 0;

        //    existingOrder.Shiftid = ordershold.Shiftid;
        //    existingOrder.UpdatedBy = ordershold.UpdatedBy;
        //    existingOrder.UpdatedDate = now;

        //    _uow.GenericRepository<OrdersHold>().Update(existingOrder);

        //    var existingItems = _uow.GenericRepository<OrderHoldItems>().Table()
        //        .Where(x => x.Orderid == existingOrder.OrderId)
        //        .ToList();

        //    foreach (var item in ordershold.Items)
        //    {
        //        var oldItem = existingItems.FirstOrDefault(x =>
        //            x.Menuitemid == item.Menuitemid &&
        //            (x.ComboMenuItemId ?? 0) == (item.ComboMenuItemId ?? 0) &&
        //            x.IsDeleted == false);

        //        if (oldItem != null)
        //        {
        //            oldItem.Itemname = item.Itemname;
        //            oldItem.Quantity = item.Quantity;
        //            oldItem.Unitprice = item.Unitprice;
        //            oldItem.Totalprice = item.Totalprice > 0 ? item.Totalprice : item.Quantity * item.Unitprice;
        //            oldItem.DiscountAmount = item.DiscountAmount ?? 0;
        //            oldItem.TaxAmount = item.TaxAmount ?? 0;
        //            oldItem.Modifierdetails = item.Modifierdetails;
        //            oldItem.Itemstatus = string.IsNullOrWhiteSpace(item.Itemstatus) ? "Hold" : item.Itemstatus;
        //            oldItem.Notes = item.Notes;
        //            oldItem.OrgId = item.OrgId > 0 ? item.OrgId : existingOrder.OrgId;
        //            oldItem.IsDeleted = false;
        //            oldItem.UpdatedBy = ordershold.UpdatedBy;
        //            oldItem.UpdatedDate = now;

        //            _uow.GenericRepository<OrderHoldItems>().Update(oldItem);
        //        }
        //        else
        //        {
        //            var newItem = new OrderHoldItems
        //            {
        //                Orderid = existingOrder.OrderId,
        //                Menuitemid = item.Menuitemid,
        //                ComboMenuItemId = item.ComboMenuItemId,
        //                Itemname = item.Itemname,
        //                Quantity = item.Quantity,
        //                Unitprice = item.Unitprice,
        //                Totalprice = item.Totalprice > 0 ? item.Totalprice : item.Quantity * item.Unitprice,

        //                DiscountAmount = item.DiscountAmount ?? 0,
        //                TaxAmount = item.TaxAmount ?? 0,
        //                Modifierdetails = item.Modifierdetails,
        //                Itemstatus = string.IsNullOrWhiteSpace(item.Itemstatus) ? "Hold" : item.Itemstatus,
        //                Notes = item.Notes,

        //                OrgId = item.OrgId > 0 ? item.OrgId : existingOrder.OrgId,
        //                IsDeleted = false,
        //                CreatedBy = ordershold.UpdatedBy,
        //                CreatedDate = now
        //            };

        //            _uow.GenericRepository<OrderHoldItems>().Insert(newItem);
        //        }
        //    }

        //    var incomingKeys = ordershold.Items
        //        .Select(x => new
        //        {
        //            x.Menuitemid,
        //            ComboMenuItemId = x.ComboMenuItemId ?? 0
        //        })
        //        .ToList();

        //    var removedItems = existingItems
        //        .Where(old => old.IsDeleted == false &&
        //            !incomingKeys.Any(x =>
        //                x.Menuitemid == old.Menuitemid &&
        //                x.ComboMenuItemId == (old.ComboMenuItemId ?? 0)))
        //        .ToList();

        //    foreach (var removed in removedItems)
        //    {
        //        removed.IsDeleted = true;
        //        removed.UpdatedBy = ordershold.UpdatedBy;
        //        removed.UpdatedDate = now;

        //        _uow.GenericRepository<OrderHoldItems>().Update(removed);
        //    }

        //    _uow.Save();

        //    return existingOrder.OrderId.ToString();
        //}

        public IEnumerable<object> GetAll(int orgid)
        {
            return (from oh in _uow.GenericRepository<OrdersHold>().Table()
                    join o in _uow.GenericRepository<Organization>().Table()
                        on oh.OrgId equals o.Id
                    join dt in _uow.GenericRepository<DiningTableMaster>().Table()
                        on oh.Tableid equals dt.Id into tableJoin
                    from dt in tableJoin.DefaultIfEmpty()
                    where oh.IsDeleted != true
                       && (orgid == 0 || oh.OrgId == orgid)
                    orderby oh.OrderId descending
                    select new
                    {
                        oh.OrderId,
                        oh.Ordernumber,
                        oh.Tableid,
                        TableName = dt != null ? dt.Name : "",
                        oh.Ordertype,
                        oh.Orderstatus,
                        oh.Itemcount,
                        oh.SubtotalAmount,
                        oh.TaxAmount,
                        oh.DiscountAmount,
                        oh.TotalAmount,
                        oh.Shiftid,
                        OrganizationId = oh.OrgId,
                        OrganizationName = o.Name
                    }).ToList();
        }

        public IEnumerable<object> GetAllHoldorderDetails(long orderId)
        {
            return (from oh in _uow.GenericRepository<OrdersHold>().Table()
                    join o in _uow.GenericRepository<Organization>().Table()
                        on oh.OrgId equals o.Id
                    where oh.IsDeleted != true
                       && oh.OrderId == orderId
                    select new
                    {
                        oh.OrderId,
                        oh.Ordernumber,
                        oh.Tableid,
                        oh.Ordertype,
                        oh.Orderstatus,
                        oh.Itemcount,
                        oh.SubtotalAmount,
                        oh.ContactNumber,
                        oh.CustomerName,
                        oh.TaxAmount,
                        oh.DiscountAmount,
                        oh.TotalAmount,
                        oh.Shiftid,
                        OrganizationId = oh.OrgId,
                        OrganizationName = o.Name,

                        Items = _uow.GenericRepository<OrderHoldItems>().Table()
                            .Where(x => x.Orderid == oh.OrderId && x.IsDeleted != true)
                            .Select(x => new
                            {
                                x.Itemid,
                                x.Orderid,
                                x.Menuitemid,
                                x.ComboMenuItemId,
                                x.Itemname,
                                x.Quantity,
                                x.Unitprice,
                                x.Totalprice,
                                x.DiscountAmount,
                                x.TaxAmount,
                                x.Modifierdetails,
                                x.Itemstatus,
                                x.Notes,
                                x.OrgId,
                                x.CreatedBy,
                                x.CreatedDate,
                                x.UpdatedBy,
                                x.UpdatedDate
                            }).ToList()
                    }).ToList();
        }

        public OrdersHold GetById(long orderId)
        {
            return _uow.GenericRepository<OrdersHold>().Table()
                .FirstOrDefault(x => x.OrderId == orderId && x.IsDeleted == false);
        }

        public string Delete(long orderId)
        {
            var order = _uow.GenericRepository<OrdersHold>().Table()
                .FirstOrDefault(x => x.OrderId == orderId && x.IsDeleted == false);

            if (order == null)
                return "OrderNotFound";

            order.IsDeleted = true;
            order.UpdatedDate = DateTime.Now;

            _uow.GenericRepository<OrdersHold>().Update(order);

            var items = _uow.GenericRepository<OrderHoldItems>().Table()
                .Where(x => x.Orderid == orderId && x.IsDeleted == false)
                .ToList();

            foreach (var item in items)
            {
                item.IsDeleted = true;
                item.UpdatedDate = DateTime.Now;
                _uow.GenericRepository<OrderHoldItems>().Update(item);
            }

            _uow.Save();

            return order.OrderId.ToString();
        }


        public async Task<string> Create(OrdersHold ordershold)
        {
            if (ordershold == null)
                return "InvalidOrder";

            if (ordershold.Items == null || !ordershold.Items.Any())
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

            foreach (var item in ordershold.Items)
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
                    item.Itemstatus ?? "",
                    item.Notes ?? ""
                );
            }

            var itemsParam = new SqlParameter("@Items", SqlDbType.Structured)
            {
                TypeName = "dbo.OrderHoldItemType",
                Value = itemsTable
            };

            var result = _context.Set<Response>()
    .FromSqlRaw(
        @"EXEC sp_OrderHold_Create
            @EntityNo       = @EntityNo,
            @OrderNumber    = @OrderNumber,
            @TableId        = @TableId,
            @OrderType      = @OrderType,
            @OrderStatus    = @OrderStatus,
            @Itemcount   = @Itemcount,
            @SubtotalAmount = @SubtotalAmount,
            @TaxAmount      = @TaxAmount,
            @DiscountAmount = @DiscountAmount,
            @TotalAmount    = @TotalAmount,
            @ContactNumber  = @ContactNumber,
            @CustomerName   = @CustomerName,
            @ShiftId        = @ShiftId,
            @OrgId          = @OrgId,
            @BranchId       = @BranchId,
            @CreatedBy      = @CreatedBy,
            @Items = @Items",
       //new SqlParameter("@EntityNo", "ORDERHOLD"),
       new SqlParameter("@EntityNo", SqlDbType.Int)
       {
           Value = ordershold.EntityNo == 0 ? DBNull.Value : ordershold.EntityNo
       },


      //new SqlParameter("@EntityNo", ordershold.EntityNo ?? 0),
        new SqlParameter("@OrderNumber",string.IsNullOrWhiteSpace(ordershold.Ordernumber)? (object)DBNull.Value : ordershold.Ordernumber),
        new SqlParameter("@TableId", ordershold.Tableid ?? (object)DBNull.Value),
        new SqlParameter("@OrderType", ordershold.Ordertype ?? (object)DBNull.Value),
        new SqlParameter("@OrderStatus", ordershold.Orderstatus ?? (object)DBNull.Value),
        new SqlParameter("@Itemcount", ordershold.Itemcount ?? (object)DBNull.Value),
        new SqlParameter("@SubtotalAmount", ordershold.SubtotalAmount ?? 0),
        new SqlParameter("@TaxAmount", ordershold.TaxAmount ?? 0),
        new SqlParameter("@DiscountAmount", ordershold.DiscountAmount ?? 0),
        new SqlParameter("@TotalAmount", ordershold.TotalAmount ?? 0),
        new SqlParameter("@ContactNumber", ordershold.ContactNumber ?? (object)DBNull.Value),
        new SqlParameter("@CustomerName", ordershold.CustomerName ?? (object)DBNull.Value),
        new SqlParameter("@ShiftId", ordershold.Shiftid == 0 ? DBNull.Value : ordershold.Shiftid),
        new SqlParameter("@OrgId", ordershold.OrgId),
        new SqlParameter("@BranchId", ordershold.BranchId),
        new SqlParameter("@CreatedBy", ordershold.CreatedBy ?? (object)DBNull.Value),
        itemsParam
    )
    .AsNoTracking()
    .AsEnumerable()
    .FirstOrDefault();

            if (result == null)
                return "Failed";

            return result.Result == "Success"
                ? result.OrderId?.ToString() ?? "Failed"
                : result.Result;
        }

        public async Task<string> Update(OrdersHold ordershold)
        {
            if (ordershold == null)
                return "InvalidOrder";

            if (ordershold.OrderId == 0)
                return "OrderIdRequired";

            if (ordershold.Items == null || !ordershold.Items.Any())
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

            foreach (var item in ordershold.Items)
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
                    item.Itemstatus ?? "",
                    item.Notes ?? ""
                );
            }

            var itemsParam = new SqlParameter("@Items", SqlDbType.Structured)
            {
                TypeName = "dbo.OrderHoldItemType",
                Value = itemsTable
            };

            var result = _context.Set<Response>()
                .FromSqlRaw(
                    @"EXEC dbo.sp_OrderHold_Update1
                @OrderId        = @OrderId,
                @TableId        = @TableId,
                @OrderType      = @OrderType,
                @OrderStatus    = @OrderStatus,
                @Itemcount      = @Itemcount,
                @SubtotalAmount = @SubtotalAmount,
                @TaxAmount      = @TaxAmount,
                @DiscountAmount = @DiscountAmount,
                @TotalAmount    = @TotalAmount,
                @ContactNumber  = @ContactNumber,
                @CustomerName   = @CustomerName,
                @ShiftId        = @ShiftId,
                @OrgId          = @OrgId,
                @BranchId       = @BranchId,
                @UpdatedBy      = @UpdatedBy,
                @Items          = @Items",
                    new SqlParameter("@OrderId", ordershold.OrderId),
                    new SqlParameter("@TableId", ordershold.Tableid ?? (object)DBNull.Value),
                    new SqlParameter("@OrderType", ordershold.Ordertype ?? (object)DBNull.Value),
                    new SqlParameter("@OrderStatus", ordershold.Orderstatus ?? (object)DBNull.Value),
                    new SqlParameter("@Itemcount", ordershold.Itemcount ?? (object)DBNull.Value),
                    new SqlParameter("@SubtotalAmount", ordershold.SubtotalAmount ?? 0),
                    new SqlParameter("@TaxAmount", ordershold.TaxAmount ?? 0),
                    new SqlParameter("@DiscountAmount", ordershold.DiscountAmount ?? 0),
                    new SqlParameter("@TotalAmount", ordershold.TotalAmount ?? 0),
                    new SqlParameter("@ContactNumber", ordershold.ContactNumber ?? (object)DBNull.Value),
                    new SqlParameter("@CustomerName", ordershold.CustomerName ?? (object)DBNull.Value),
                    new SqlParameter("@ShiftId", ordershold.Shiftid == 0 ? DBNull.Value : ordershold.Shiftid),
                    new SqlParameter("@OrgId", ordershold.OrgId),
                    new SqlParameter("@BranchId", ordershold.BranchId),
                    new SqlParameter("@UpdatedBy", ordershold.UpdatedBy ?? (object)DBNull.Value),
                    itemsParam
                )
                .AsNoTracking()
                .AsEnumerable()
                .FirstOrDefault();

            if (result == null)
                return "Failed";

            return result.Result == "Success"
                ? result.OrderId?.ToString() ?? "Failed"
                : result.Result;
        }
    }
}