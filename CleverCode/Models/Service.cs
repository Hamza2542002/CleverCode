using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace CleverCode.Models
{
    public class Service
    {
        public int Service_ID { get; set; }
        public string? Title { get; set; }
        public string? Icon { get; set; }
        public string? Description { get; set; }
        public decimal Pricing { get; set; }
        public string? Feature { get; set; }
        public string? Category { get; set; }
        public string? TimeLine { get; set; }

        public ICollection<ProjectService> ProjectServices { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Complaint>? Complaints { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }

}
