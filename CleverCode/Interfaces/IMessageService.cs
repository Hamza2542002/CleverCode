using CleverCode.DTO;

namespace CleverCode.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageDto>> GetAllAsync();
        Task<MessageDto?> GetByIdAsync(int id);
        Task<MessageDto> CreateAsync(MessageDto dto);
        Task<bool> UpdateAsync(int id, MessageDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
