namespace UNITYPOS_API.Entities
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EmpCode { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}
