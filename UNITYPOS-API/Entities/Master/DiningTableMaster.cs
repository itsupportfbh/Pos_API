namespace UNITYPOS_API.Entities.Master
{
    public class DiningTableMaster : CommonClass
    {
        public int Id { get; set; }
        public string Code { get; set; }      
        public string Name { get; set; }          
        public int SeatingSize { get; set; }
        public int BranchId { get; set; }
        public int FloorId { get; set; }
        public string Image { get; set; }
        public string Remarks { get; set; }
           
       
        public bool IsOccupied { get; set; }       
        public bool IsReservable { get; set; }     
        public bool IsJoinable { get; set; }      
        public bool IsOrdered { get; set; }
        public bool IsAvaible { get; set; }
        public int DisplayOrder { get; set; }
        public int OrgId { get; set; }

    }
}
