namespace ABAC.Models
{
    public class JwtSettings
    {
        public string SecretPath { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiryMinutes { get; set; }
    }
}
