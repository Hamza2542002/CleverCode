namespace CleverCode.Models
{
    public class Complaint
    {
        public int Complaint_ID { get; set; }
        public string? Type_AR { get; set; }
        public string? Type_EN { get; set; }
        public string? Description_AR { get; set; }
        public string? Description_EN { get; set; }
        public string? Status_AR { get; set; }
        public string? Status_EN { get; set; }
        public string? Priority_AR { get; set; }
        public string? Priority_EN { get; set; }
        public string? Name_AR { get; set; }
        public string? Name_EN { get; set; }
        public DateTime Date { get; set; }

        public int? Service_ID { get; set; }
        public Service? Service { get; set; }
    }
}
