namespace UNITYPOS_API.Entities
{
    public class RolePermission:CommonClass
    {

        public int Id { get; set; }
        public int RoleId { get; set; }

        public int EntityNo {  get; set; }
       public bool Create {  get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool View { get; set; }




    }
}
