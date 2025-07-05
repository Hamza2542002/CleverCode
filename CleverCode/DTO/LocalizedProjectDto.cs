namespace CleverCode.DTO
{
    public class LocalizedProjectDto
    {
        public int? Project_ID { get; set; }
        public string? Title { get; set; }  // Localized
        public int Rate { get; set; }
        public string? Description { get; set; }  // Localized
        public string? Tech { get; set; }
        public string? ProjectLink { get; set; }
        public string? ImageUrl { get; set; }
        public int Service_ID { get; set; }
    }
}