namespace UNITYPOS_API.Common
{
    public class Response
    {

        public string Result { get; set; } = string.Empty;
        public int? OrderId { get; set; }
        public string? OrderNumber { get; set; }
    }
}
