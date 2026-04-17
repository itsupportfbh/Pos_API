namespace UNITYPOS_API.Entities
{
    public class Tax:CommonClass
    {

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Percentage { get; set; }

    }
}
