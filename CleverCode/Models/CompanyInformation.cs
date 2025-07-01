public class CompanyInformation
{
    public int Company_ID { get; set; }

    public string? NameEn { get; set; }
    public string? NameAr { get; set; }

    public string? DescriptionEn { get; set; }
    public string? DescriptionAr { get; set; }

    public string? MissionEn { get; set; }
    public string? MissionAr { get; set; }

    public string? VisionEn { get; set; }
    public string? VisionAr { get; set; }

    public string? StoryEn { get; set; }
    public string? StoryAr { get; set; }

    public string? ResponseTime { get; set; }
    public string? Logo { get; set; }
    public string? SocialLink { get; set; }

    public List<CompanyValues>? Values { get; set; }
    public ContactInfo? ContactInfo { get; set; }
}