namespace UNITYPOS_API.Entities.Master
{
    public class Printer:CommonClass
    {
        public int Id { get; set; }
        public string Name { get; set; }      // Kitchen Printer, Billing Printer
        public string Code { get; set; }      // PRN001
        public int BranchId { get; set; }            // Branch mapping
        public int? CounterId { get; set; }          // Optional
        public int? TerminalId { get; set; }         // Optional
         public string Remarks { get; set; }

        

    }
}
