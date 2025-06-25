namespace CleverCode.DTO
{
    public class ProjectDto
    {
        public int? Project_ID { get; set; }
        public string? Title { get; set; }
        public int Rate { get; set; }
        public string? Description { get; set; }
        public string? Tech { get; set; }
        public int? ServiceId { get; set; }
        public ICollection<ProjectServiceDto>? ProjectServices { get; set; }
    }
}
