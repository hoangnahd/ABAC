namespace ABAC.Models
{
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public string TokenValue { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
