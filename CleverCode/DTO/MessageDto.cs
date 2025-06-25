namespace CleverCode.DTO
{
    public class MessageDto
    {
        public int Message_ID { get; set; }
        public string? MessageText { get; set; }
        public string? Name { get; set; }
        public DateTime Date { get; set; }
        public string? Company { get; set; }
        public string? Email { get; set; }
        public int? Service_ID { get; set; }
    }
}
