using MyJwtTokenAuthentication;

namespace MyJwtTokenAuthentication.Models
{
    public class JwtSettings
    {
        public JwtSettings() { }
        public List<string>? Issuers { get; set; }
        public List<string>? Audiences { get; set; }
        public string SecretKey { get; set; } = SecretKeyGenerator.GenerateSecretKey();
    }
}
