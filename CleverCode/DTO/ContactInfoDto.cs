public class ContactInfoDto
{
    public string? Email { get; set; }
    public string? Phone { get; set; }
    // احذف الحقل Address (بدون لغة)
    public string? AddressEn { get; set; }
    public string? AddressAr { get; set; }
}