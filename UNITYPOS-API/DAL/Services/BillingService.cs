using System.Linq;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class BillingService:IBillingService
    {
        private readonly IUnitOfWork _uow;
        private readonly ICodeTemplateService _codeTemplateService;

        public BillingService(IUnitOfWork uow, ICodeTemplateService codeTemplateService)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _codeTemplateService = codeTemplateService;
        }




       



        public string Create(Billing billing)
        {
            if (billing == null)
                return "InvalidBilling";

            if (billing.EntityNo <= 0)
                return "EntityNoRequired";

            if (billing.OrgId <= 0)
                return "OrgIdRequired";

            if (billing.BranchId <= 0)
                return "BranchIdRequired";

            if (billing.BillingDetails == null || !billing.BillingDetails.Any())
                return "BillingDetailsRequired";

            var now = DateTime.Now;

            var orderIds = new List<int>();

            if (billing.OrderIds != null && billing.OrderIds.Any())
            {
                orderIds = billing.OrderIds
                    .Where(x => x > 0)
                    .Distinct()
                    .ToList();
            }
            else if (billing.OrderId > 0)
            {
                orderIds.Add(billing.OrderId);
            }

            if (!orderIds.Any())
                return "OrderIdRequired";

            bool alreadyBilled = _uow.GenericRepository<BillingOrders>().Table()
                .Any(x => orderIds.Contains(x.OrderId)
                       && x.OrgId == billing.OrgId
                       && x.BranchId == billing.BranchId
                       && x.IsDeleted == false);

            if (alreadyBilled)
                return "AlreadyExists";

            string billNo = _codeTemplateService.GetLatestCode(
                billing.EntityNo,
                billing.OrgId,
                billing.BranchId
            );

            if (string.IsNullOrWhiteSpace(billNo))
                return "CodeTemplateNotFound";

            decimal totalAmount = billing.TotalAmount;
            decimal receivedAmount = billing.ReceivedAmount;

            decimal balanceAmount = 0;
            decimal changeAmount = 0;
            int paymentStatus;

            if (receivedAmount <= 0)
            {
                paymentStatus = 0;//Pending
                balanceAmount = totalAmount;
            }
            else if (receivedAmount < totalAmount)
            {
                paymentStatus = 2;//partialpayment
                balanceAmount = totalAmount - receivedAmount;
            }
            else if (receivedAmount == totalAmount)
            {
                paymentStatus = 1;//Paid
            }
            else
            {
                paymentStatus = 7;//OverPaid
                changeAmount = receivedAmount - totalAmount;
            }

            string paymentType = billing.BillingDetails.Count > 1
                ? "Multi Payment"
                : billing.BillingDetails.First().PaymentMode;

            var billDate = billing.BillDate == default ? now : billing.BillDate;

            var todayStart = billDate.Date;
            var tomorrowStart = todayStart.AddDays(1);

            int lastTokenNo = _uow.GenericRepository<Billing>().Table()
    .Where(x => x.OrgId == billing.OrgId
             && x.BranchId == billing.BranchId
             && x.BillDate >= todayStart
             && x.BillDate < tomorrowStart
             && x.IsDeleted == false)
    .Max(x => (int?)x.TokenNo) ?? 0;

            int tokenNo = lastTokenNo + 1;


            var billEntity = new Billing
            {
                BillNo = billNo,
                OrderId = orderIds.First(),

                CustomerId = billing.CustomerId,
                BillDate = billDate,
                TokenNo = tokenNo,

                GrossAmount = billing.GrossAmount,
                DiscountAmount = billing.DiscountAmount,
                ServiceCharge = billing.ServiceCharge,
                TaxAmount = billing.TaxAmount,
                TaxPercentage = billing.TaxPercentage,
                TipAmount = billing.TipAmount,
                RoundOff = billing.RoundOff,
                TotalAmount = totalAmount,
               // TokenNo=tokenno,
                ReceivedAmount = receivedAmount,
                BalanceAmount = balanceAmount,
                ChangeAmount = changeAmount,

                BillMode = string.IsNullOrWhiteSpace(billing.BillMode)
                    ? "DineIn"
                    : billing.BillMode,

                PaymentStatus = paymentStatus,
                PaymentType = paymentType,
                Remarks = billing.Remarks,

                OrgId = billing.OrgId,
                BranchId = billing.BranchId,

                IsActive = true,
                IsDeleted = false,
                CreatedBy = billing.CreatedBy,
                CreatedDate = now
            };

            _uow.GenericRepository<Billing>().Insert(billEntity);
            _uow.Save();

            if (billEntity.Id <= 0)
                return "BillingSaveFailed";

            var orders = _uow.GenericRepository<Orders>().Table()
                .Where(x => orderIds.Contains(x.Orderid)
                         && x.OrgId == billing.OrgId
                         && x.BranchId == billing.BranchId
                         && x.IsDeleted == false)
                .ToList();

            foreach (var orderId in orderIds)
            {
                var order = orders.FirstOrDefault(x => x.Orderid == orderId);

                var billOrder = new BillingOrders
                {
                    BillingId = billEntity.Id,
                    BillNo = billEntity.BillNo,

                    OrderId = orderId,
                    OrderNo = order?.OrderNumber,
                    TableId = order?.TableId,

                    OrgId = billing.OrgId,
                    BranchId = billing.BranchId,

                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = billing.CreatedBy,
                    CreatedDate = now
                };

                _uow.GenericRepository<BillingOrders>().Insert(billOrder);
            }

            _uow.Save();

            foreach (var detail in billing.BillingDetails)
            {
                var detailEntity = new BillingDetails
                {
                    BillingId = billEntity.Id,

                    PaymentMode = string.IsNullOrWhiteSpace(detail.PaymentMode)
                        ? paymentType
                        : detail.PaymentMode,

                    GrossAmount = detail.GrossAmount,

                    ReferenceNo = detail.ReferenceNo,
                    TransactionId = detail.TransactionId,
                    CardNumber = detail.CardNumber,

                    TaxableAmount = detail.TaxableAmount,
                    TaxPercentage = detail.TaxPercentage,
                    TaxAmount = detail.TaxAmount,

                    SGSTPercentage = detail.SGSTPercentage,
                    SGSTAmount = detail.SGSTAmount,

                    CGSTPercentage = detail.CGSTPercentage,
                    CGSTAmount = detail.CGSTAmount,

                    IGSTPercentage = detail.IGSTPercentage,
                    IGSTAmount = detail.IGSTAmount,

                    TotalAmount = detail.TotalAmount,
                    Remarks = detail.Remarks,

                    PaymentStatus = detail.PaymentStatus == 0
                        ? paymentStatus
                        : detail.PaymentStatus,

                    OrgId = billing.OrgId,
                    BranchId = billing.BranchId,

                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = billing.CreatedBy,
                    CreatedDate = now
                };

                _uow.GenericRepository<BillingDetails>().Insert(detailEntity);
            }

            _uow.Save();

            if (billing.OrderItems != null && billing.OrderItems.Any())
            {
                foreach (var item in billing.OrderItems)
                {
                    if (item.Orderid <= 0)
                        item.Orderid = orderIds.First();

                    if (!orderIds.Contains(item.Orderid))
                        continue;

                    // IMPORTANT: force backend values
                    item.OrgId = billing.OrgId;
                    item.BranchId = billing.BranchId;

                    var existingItem = _uow.GenericRepository<Orderitems>().Table()
                        .FirstOrDefault(x => x.Orderid == item.Orderid
                                          && x.OrgId == billing.OrgId
                                          && x.BranchId == billing.BranchId
                                          && x.Menuitemid == item.Menuitemid
                                          && x.Itemname == item.Itemname
                                          && x.IsDeleted == false);

                    if (existingItem != null)
                    {
                        existingItem.Quantity += item.Quantity;
                        existingItem.Totalprice += item.Totalprice;
                        existingItem.TaxAmount += item.TaxAmount;
                        existingItem.DiscountAmount += item.DiscountAmount;

                        existingItem.OrgId = billing.OrgId;
                        existingItem.BranchId = billing.BranchId;
                        existingItem.Itemstatus = 6;

                        existingItem.UpdatedBy = billing.CreatedBy;
                        existingItem.UpdatedDate = now;

                        _uow.GenericRepository<Orderitems>().Update(existingItem);
                    }
                    else
                    {
                        var newItem = new Orderitems
                        {
                            Orderid = item.Orderid,

                            Itemid = item.Itemid,
                            Menuitemid = item.Menuitemid,
                            ComboMenuItemId = item.ComboMenuItemId,
                            Itemname = item.Itemname,

                            Quantity = item.Quantity,
                            Unitprice = item.Unitprice,
                            Totalprice = item.Totalprice,
                            DiscountAmount = item.DiscountAmount,
                            TaxAmount = item.TaxAmount,

                            Modifierdetails = item.Modifierdetails,
                            Notes = item.Notes,

                            Itemstatus = 6,

                            OrgId = billing.OrgId,
                            BranchId = billing.BranchId,

                            
                            IsDeleted = false,
                            CreatedBy = billing.CreatedBy,
                            CreatedDate = now
                        };

                        _uow.GenericRepository<Orderitems>().Insert(newItem);
                    }
                }

                _uow.Save();
            }

            foreach (var order in orders)
            {
                order.OrderStatus = 6;
                order.UpdatedBy = billing.CreatedBy;
                order.UpdatedDate = now;

                _uow.GenericRepository<Orders>().Update(order);
            }

            _uow.Save();

            var orderItems = _uow.GenericRepository<Orderitems>().Table()
                .Where(x => orderIds.Contains(x.Orderid)
                         && x.OrgId == billing.OrgId
                         && x.BranchId == billing.BranchId
                         && x.IsDeleted == false)
                .ToList();

            foreach (var item in orderItems)
            {
                item.Itemstatus = 6;
                item.OrgId = billing.OrgId;
                item.BranchId = billing.BranchId;
                item.UpdatedBy = billing.CreatedBy;
                item.UpdatedDate = now;

                _uow.GenericRepository<Orderitems>().Update(item);
            }

        

            var codeTemplate = _uow.GenericRepository<CodeTemplate>().Table()
                .FirstOrDefault(x => x.EntityNo == billing.EntityNo
                                  && x.OrgId == billing.OrgId
                                  && x.BranchId == billing.BranchId
                                  && x.IsMaster == false);

            if (codeTemplate != null)
            {
                codeTemplate.CurrentValue += 1;
                _uow.GenericRepository<CodeTemplate>().Update(codeTemplate);
                _uow.Save();
            }
            _uow.Save();

            // Free dining tables after successful payment
            var tableIds = orders
                .Where(x => x.TableId.HasValue && x.TableId.Value > 0)
                .Select(x => x.TableId.Value)
                .Distinct()
                .ToList();

            var floorIds = orders
                .Where(x => x.FloorId.HasValue && x.FloorId.Value > 0)
                .Select(x => x.FloorId.Value)
                .Distinct()
                .ToList();

            if (tableIds.Any())
            {
                var tables = _uow.GenericRepository<DiningTableMaster>().Table()
                    .Where(x => tableIds.Contains(x.Id)
                             && floorIds.Contains(x.FloorId)
                             && x.OrgId == billing.OrgId
                             && x.BranchId == billing.BranchId
                             && x.IsDeleted == false)
                    .ToList();

                foreach (var table in tables)
                {
                    table.IsOccupied = false;
                    table.UpdatedBy = billing.CreatedBy;
                    table.UpdatedDate = now;

                    _uow.GenericRepository<DiningTableMaster>().Update(table);
                }

                _uow.Save();
            }







            return billEntity.Id.ToString();
        }
        public string Update(Billing billing)
        {
            if (billing == null)
                return "InvalidBilling";

            if (billing.Id <= 0)
                return "BillingIdRequired";

            if (billing.OrderId <= 0)
                return "OrderIdRequired";

            if (billing.OrgId <= 0)
                return "OrgIdRequired";

            if (billing.BranchId <= 0)
                return "BranchIdRequired";

            if (billing.BillingDetails == null || !billing.BillingDetails.Any())
                return "BillingDetailsRequired";

            var entity = _uow.GenericRepository<Billing>().Table()
                .FirstOrDefault(x => x.Id == billing.Id
                                  && x.OrgId == billing.OrgId
                                  && x.BranchId == billing.BranchId
                                  && x.IsDeleted == false);

            if (entity == null)
                return "BillingNotFound";

            bool alreadyExists = _uow.GenericRepository<Billing>().Table()
                .Any(x => x.Id != billing.Id
                       && x.OrderId == billing.OrderId
                       && x.OrgId == billing.OrgId
                       && x.BranchId == billing.BranchId
                       && x.IsDeleted == false);

            if (alreadyExists)
                return "AlreadyExists";

            decimal totalAmount = billing.TotalAmount;
            decimal receivedAmount = billing.ReceivedAmount;

            decimal balanceAmount = totalAmount - receivedAmount;
            decimal changeAmount = 0;
            int paymentStatus;

            if (receivedAmount <= 0)
            {
                paymentStatus = 0; // Pending
                balanceAmount = totalAmount;
                changeAmount = 0;
            }
            else if (receivedAmount < totalAmount)
            {
                paymentStatus = 2; // Partial Paid
                balanceAmount = totalAmount - receivedAmount;
                changeAmount = 0;
            }
            else if (receivedAmount == totalAmount)
            {
                paymentStatus = 1; // Paid
                balanceAmount = 0;
                changeAmount = 0;
            }
            else
            {
                paymentStatus = 7; // Overpaid
                balanceAmount = 0;
                changeAmount = receivedAmount - totalAmount;
            }

            string paymentType = billing.BillingDetails.Count > 1
                ? "Multi Payment"
                : billing.BillingDetails.First().PaymentMode;

            entity.OrderId = billing.OrderId;
            entity.CustomerId = billing.CustomerId;
            entity.BillDate = billing.BillDate == default ? entity.BillDate : billing.BillDate;
            entity.TokenNo = billing.TokenNo;

            entity.GrossAmount = billing.GrossAmount;
            entity.DiscountAmount = billing.DiscountAmount;
            entity.ServiceCharge = billing.ServiceCharge;
            entity.TaxAmount = billing.TaxAmount;
            entity.TaxPercentage = billing.TaxPercentage;
            entity.TipAmount = billing.TipAmount;
            entity.RoundOff = billing.RoundOff;
            entity.TotalAmount = billing.TotalAmount;

            entity.ReceivedAmount = receivedAmount;
            entity.BalanceAmount = balanceAmount;
            entity.ChangeAmount = changeAmount;

            entity.BillMode = string.IsNullOrWhiteSpace(billing.BillMode)
                ? entity.BillMode
                : billing.BillMode;

            entity.PaymentStatus = paymentStatus;
            entity.PaymentType = paymentType;
            entity.Remarks = billing.Remarks;

            entity.UpdatedBy = billing.UpdatedBy;
            entity.UpdatedDate = DateTime.Now;

            _uow.GenericRepository<Billing>().Update(entity);

            var oldDetails = _uow.GenericRepository<BillingDetails>().Table()
                .Where(x => x.BillingId == billing.Id
                         && x.OrgId == billing.OrgId
                         && x.BranchId == billing.BranchId)
                .ToList();

            foreach (var oldDetail in oldDetails)
            {
                _uow.GenericRepository<BillingDetails>().Delete(oldDetail);
            }

            foreach (var detail in billing.BillingDetails)
            {
                var detailEntity = new BillingDetails
                {
                    BillingId = entity.Id,

                    PaymentMode = string.IsNullOrWhiteSpace(detail.PaymentMode)
                        ? paymentType
                        : detail.PaymentMode,

                    GrossAmount = detail.GrossAmount,

                    ReferenceNo = detail.ReferenceNo,
                    TransactionId = detail.TransactionId,
                    CardNumber = detail.CardNumber,

                    TaxableAmount = detail.TaxableAmount,

                    TaxPercentage = detail.TaxPercentage,
                    TaxAmount = detail.TaxAmount,

                    SGSTPercentage = detail.SGSTPercentage,
                    SGSTAmount = detail.SGSTAmount,

                    CGSTPercentage = detail.CGSTPercentage,
                    CGSTAmount = detail.CGSTAmount,

                    IGSTPercentage = detail.IGSTPercentage,
                    IGSTAmount = detail.IGSTAmount,

                    TotalAmount = detail.TotalAmount,

                    Remarks = detail.Remarks,

                    PaymentStatus = detail.PaymentStatus == 0
                        ? paymentStatus
                        : detail.PaymentStatus,

                    OrgId = billing.OrgId,
                    BranchId = billing.BranchId,

                    IsActive = true,
                    CreatedBy = billing.UpdatedBy,
                    CreatedDate = DateTime.Now
                };

                _uow.GenericRepository<BillingDetails>().Insert(detailEntity);
            }

            // ✅ Update Order status
            var billDate = (billing.CreatedDate ?? DateTime.Now).Date;
            var nextDate = billDate.AddDays(1);

            var order = _uow.GenericRepository<Orders>().Table()
                .FirstOrDefault(x => x.Orderid == billing.OrderId
                                  && x.OrgId == billing.OrgId
                                  && x.BranchId == billing.BranchId
                                  && x.CreatedDate >= billDate
                                  && x.CreatedDate < nextDate
                                  && x.IsDeleted == false);

            if (order != null)
            {
                order.OrderStatus = 5; // Completed
                order.UpdatedBy = billing.UpdatedBy;
                order.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<Orders>().Update(order);

                // ✅ Update OrderItems status
                var orderItems = _uow.GenericRepository<Orderitems>().Table()
                    .Where(x => x.Orderid == billing.OrderId
                             && x.OrgId == billing.OrgId
                            // && x.BranchId == billing.BranchId
                             && x.IsDeleted == false)
                    .ToList();

                foreach (var item in orderItems)
                {
                    item.Itemstatus = 6; // Completed
                    item.UpdatedBy = billing.UpdatedBy;
                    item.UpdatedDate = DateTime.Now;

                    _uow.GenericRepository<Orderitems>().Update(item);
                }
            }

            _uow.Save();

            return entity.Id.ToString();
        }
    }
}
