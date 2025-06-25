namespace CleverCode.Models
{
    public class CompanyValues
    {
        public int Id { get; set; }
        public int Company_ID { get; set; }
        public CompanyInformation? CompanyInformation { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }
}


