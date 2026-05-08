namespace UNITYPOS_API.Entities.Master
{
    public class FoodMenu : CommonClass
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public decimal Price { get; set; }
        public int OrgId { get; set; }

    }
}
