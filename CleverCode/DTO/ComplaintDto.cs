public class ComplaintDto
{
    public int Complaint_ID { get; set; }
    public string? Type { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; } // ✅ الإيميل
    public DateTime Date { get; set; }
    public int? Service_ID { get; set; }
}
