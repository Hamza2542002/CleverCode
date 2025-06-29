namespace CleverCode.DTO
{
    public class ProjectDto
    {
        public int? Project_ID { get; set; }
        public string? Title { get; set; }
        public int Rate { get; set; }
        public string? Description { get; set; }
        public string? Tech { get; set; }
        public string? ProjectLink { get; set; }
        public int Service_ID { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
    }
}
