public interface IFAQService
{
    Task<List<LocalizedFaqDto>> GetAllAsync();
    Task<LocalizedFaqDto?> GetByIdAsync(int id);
    Task<FAQDto> CreateAsync(FAQDto dto);
    Task<bool> UpdateAsync(int id, FAQDto dto);
    Task<bool> DeleteAsync(int id);
}

