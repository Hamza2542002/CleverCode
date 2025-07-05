namespace CleverCode.Models
{
    public class Project
    {
        public int Project_ID { get; set; }
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public int Rate { get; set; }
        public string? Tech { get; set; }
        public string? ProjectLink { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<ProjectService>? ProjectServices { get; set; }
    }
}