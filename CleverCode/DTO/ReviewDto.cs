namespace CleverCode.DTO
{
    public class ReviewDto
    {
        public int? Review_ID { get; set; }
        public string? Comment { get; set; }
        public int Rate { get; set; }
        public DateTime Date { get; set; }
        public string? Name { get; set; }
        public string? Company { get; set; }
        public bool IsApproved { get; set; }
        public int? Service_ID { get; set; }
        public string? ServiceName { get; set; }
    }
}
