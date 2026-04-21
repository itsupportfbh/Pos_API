namespace UNITYPOS_API.Entities.Master
{
    public class FoodMenu : CommonClass
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }

    }
}
