using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.Entities
{
    public class Orders :CommonClass
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        public string? TableId { get; set; }

        public string OrderType { get; set; } = string.Empty;

        public string OrderStatus { get; set; } 

        public int? GuestCount { get; set; } = 1;

        public decimal? SubtotalAmount { get; set; } = 0.00m;

        public decimal? TaxAmount { get; set; } = 0.00m;

        public decimal? DiscountAmount { get; set; } = 0.00m;

        public decimal? TotalAmount { get; set; } = 0.00m;

        public string? Shiftid { get; set; }

        public int OrgId { get; set; }


    }
}
