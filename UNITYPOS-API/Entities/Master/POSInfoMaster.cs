namespace UNITYPOS_API.Entities.Master
{
    public class POSInfoMaster : CommonClass
    {
        public int Id { get; set; }

        public string POSName { get; set; }           // Counter POS
              
                // Usage Type
        public string UsedFor { get; set; }           // Counter / Waiter / Manager

        // Table Group Mapping
        public int? DefaultFloorId { get; set; }      // Main / 1st Floor / 2nd Floor
        public string IpAddress { get; set; }         // 192.168.1.10
        public string? MachineName { get; set; }      // DESKTOP-123 / POS1
        public string? MacAddress { get; set; }       // Optional (more secure)


        // Mapping
        public int BranchId { get; set; }
        public int? TerminalId { get; set; }
        public int? CounterId { get; set; }
        public string? Remarks { get; set; }

        
    }
}
