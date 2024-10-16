namespace Core.Models
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public int RefreshHours { get; set; }
        public string? Issuer { get; set; } = "Blog";
        public string? Audience { get; set; }
    }
}
