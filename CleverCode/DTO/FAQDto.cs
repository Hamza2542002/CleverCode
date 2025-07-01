public class FAQDto
{
    public int FAQ_ID { get; set; }
    public string? Questions { get; set; } // سيتم ملؤه حسب اللغة المطلوبة
    public string? Answer { get; set; }    // سيتم ملؤه حسب اللغة المطلوبة

    public string? QuestionsEn { get; set; }
    public string? QuestionsAr { get; set; }
    public string? AnswerEn { get; set; }
    public string? AnswerAr { get; set; }
}
