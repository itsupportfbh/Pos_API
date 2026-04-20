namespace UNITYPOS_API.Entities.Master
{
    public class TableMaster : CommonClass
    {
        public int Id { get; set; }
        public string Code { get; set; }      // T1, T2, M1
        public string Name { get; set; }           // Family Table, Window Table
        public int SeatingSize { get; set; }
        public int BranchId { get; set; }
        public int FloorId { get; set; }
        public string Image { get; set; }
        public string Remarks { get; set; }
            
       
        public bool IsOccupied { get; set; }       // current status
        public bool IsReservable { get; set; }     // allow reservation
        public bool IsJoinable { get; set; }       // join table option
        public bool IsOrdered { get; set; }
        public bool IsAvaible { get; set; }
        public int DisplayOrder { get; set; }
    }
}
