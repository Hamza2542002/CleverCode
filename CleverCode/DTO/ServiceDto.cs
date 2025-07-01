using CleverCode.DTO;

public class ServiceDto
{
    public int Service_ID { get; set; }
    public string? Title { get; set; }
    public string? TitleAr { get; set; }  // جديد
    public string? Icon { get; set; }
    public string? Description { get; set; }
    public string? DescriptionAr { get; set; }  // جديد
    public decimal Pricing { get; set; }
    public string? Feature { get; set; }
    public string? Category { get; set; }
    public string? TimeLine { get; set; }

    public ICollection<ProjectDto>? Projects { get; set; }
    public ICollection<ReviewDto>? Reviews { get; set; }
    public ICollection<ComplaintDto>? Complaints { get; set; }
    public ICollection<MessageDto>? Messages { get; set; }
}
