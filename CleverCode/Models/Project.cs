namespace CleverCode.Models
{
    public class Project
    {
        public int Project_ID { get; set; }
        public string? Title { get; set; }
        public int Rate { get; set; }
        public string? Description { get; set; }
        public string? Tech { get; set; }
        public string? ProjectLink { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<ProjectService>? ProjectServices { get; set; }
    }

}
