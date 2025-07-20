using CleverCode.DTO;

public class LocalizedServiceDto
{
    public int Service_ID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Pricing { get; set; }
    public string Feature { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string TimeLine { get; set; } = string.Empty;

    public ICollection<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
    public ICollection<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
    public ICollection<ComplaintDto> Complaints { get; set; } = new List<ComplaintDto>();
    public ICollection<MessageDto> Messages { get; set; } = new List<MessageDto>();
}
