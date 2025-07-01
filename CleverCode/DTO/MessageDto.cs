namespace CleverCode.DTO
{
    public class MessageDto
    {
        public int Message_ID { get; set; }

        // دعم اللغات للنص
        public string? MessageTextEn { get; set; }
        public string? MessageTextAr { get; set; }

        public string? Name { get; set; }
        public DateTime Date { get; set; }
        public string? Company { get; set; }
        public string? Email { get; set; }
        public int? Service_ID { get; set; }
    }
}
