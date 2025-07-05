using Microsoft.AspNetCore.Http;

namespace CleverCode.DTO
{
    public class ProjectDto
    {
        public int? Project_ID { get; set; }

        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }

        public int Rate { get; set; }

        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }

        public string? Tech { get; set; }

        public string? ProjectLink { get; set; }

        public int Service_ID { get; set; }
        public List<IFormFile>? Image { get; set; }
        public string? ImageUrl { get; set; }
    }
}
