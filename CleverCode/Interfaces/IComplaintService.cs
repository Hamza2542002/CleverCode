using CleverCode.DTO;

namespace CleverCode.Interfaces
{
    public interface IComplaintService
    {
        Task<IEnumerable<ComplaintDto>> GetAllAsync();
        Task<ComplaintDto?> GetByIdAsync(int id);
        Task<ComplaintDto> CreateAsync(ComplaintDto dto);
        Task<bool> UpdateAsync(int id, ComplaintDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
