using CleverCode.DTO;

namespace CleverCode.Interfaces
{
    public interface IComplaintService
    {
        Task<IEnumerable<ComplaintDto>> GetAllAsync(string? lang = "en");
        Task<ComplaintDto?> GetByIdAsync(int id, string? lang = "en");
        Task<ComplaintDto> CreateAsync(ComplaintDto dto, string? lang = "en");
        Task<bool> UpdateAsync(int id, ComplaintDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ResolveAsync(int id); // ✅ تمت الإضافة
        Task<IEnumerable<ComplaintDto>> GetPendingComplaintsAsync(string? lang = "en");

    }
}
