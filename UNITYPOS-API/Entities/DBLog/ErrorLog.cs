using System.ComponentModel.DataAnnotations;

namespace UNITYPOS_API.Entities.DBLog
{
    public class ErrorLog
    {
        [Key]
        public int ErrorId { get; set; }
        public string? ErrorInComponent { get; set; }
        public string? ErrorEvent { get; set; }
        public string? Error { get; set; }
        public DateTime ErrorTime { get; set; }
        public string? LoggedUserName { get; set; }
    }
}
