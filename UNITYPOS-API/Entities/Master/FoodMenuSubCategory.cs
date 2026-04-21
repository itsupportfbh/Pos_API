namespace UNITYPOS_API.Entities.Master
{
    public class FoodMenuSubCategory : CommonClass
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public FoodMenuCategory Category { get; set; }
    }
}
