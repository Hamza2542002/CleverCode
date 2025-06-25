using CleverCode.DTO;

namespace CleverCode.Interfaces
{
    public interface IFAQService
    {
        Task<IEnumerable<FAQDto>> GetAllAsync();
        Task<FAQDto?> GetByIdAsync(int id);
        Task<FAQDto> CreateAsync(FAQDto dto);
        Task<bool> UpdateAsync(int id, FAQDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
