using CleverCode.Models;

public class Review
{
    public int Review_ID { get; set; }

    public string? CommentEn { get; set; }
    public string? CommentAr { get; set; }

    public int Rate { get; set; }
    public DateTime Date { get; set; }
    public string? Name { get; set; }
    public string? Company { get; set; }
    public bool IsApproved { get; set; }

    public int? Service_ID { get; set; }
    public Service? Service { get; set; }
}
