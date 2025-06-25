using CleverCode.DTO;

namespace CleverCode.Models
{
    public class CompanyInformation
    {
        public int Company_ID { get; set; }
        public string? Mission { get; set; }
        public string? Vision { get; set; }
        public List<CompanyValues>? Values { get; set; }
        public ContactInfo? ContactInfo { get; set; }
        public string? Logo { get; set; }
        public string? SocialLink { get; set; }
        public string? Story { get; set; }
        public string? ResponseTime { get; set; }
     
        public string? Name { get; set; }
        public string? Description { get; set; }
  
    }

}
