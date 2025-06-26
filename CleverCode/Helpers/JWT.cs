namespace CleverCode.Helpers
{
    public class JWT
    {
        public string? SecurityKey { get; set; }
        public string? AudienceIP { get; set; }
        public string? IssuerIP { get; set; }
        public double DurationInDays { get; set; }
    }
}
