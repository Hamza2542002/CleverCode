public class CompanyInformationLocalizedDto
{
    public int Company_ID { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Mission { get; set; }
    public string? Vision { get; set; }
    public string? Story { get; set; }
    public string? ResponseTime { get; set; }
    public string? Logo { get; set; }
    public string? SocialLink { get; set; }
    public List<CompanyValueLocalizedDto>? Values { get; set; }
    public ContactInfoLocalizedDto? ContactInfo { get; set; }
}

public class CompanyValueLocalizedDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class ContactInfoLocalizedDto
{
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
}
