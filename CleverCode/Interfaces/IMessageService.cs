using CleverCode.DTO;

public interface IMessageService
{
    Task<IEnumerable<object>> GetAllAsync();
    Task<object?> GetByIdAsync(int id);
    Task<MessageDto> CreateAsync(MessageDto dto);
    Task<bool> UpdateAsync(int id, MessageDto dto);
    Task<bool> DeleteAsync(int id);
}
