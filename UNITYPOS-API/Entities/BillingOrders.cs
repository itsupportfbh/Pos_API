namespace UNITYPOS_API.Entities
{
    public class BillingOrders
    {
        public int Id { get; set; }

        public int BillingId { get; set; }
        public string BillNo { get; set; } = string.Empty;

        public int OrderId { get; set; }
        public string? OrderNo { get; set; }

        public int? TableId { get; set; }
        public string? TableName { get; set; }

        public int OrgId { get; set; }
        public int BranchId { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
