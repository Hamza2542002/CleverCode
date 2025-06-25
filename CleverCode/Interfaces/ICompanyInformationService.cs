using CleverCode.DTO;

namespace CleverCode.Interfaces
{
    public interface ICompanyInformationService
    {
        Task<IEnumerable<CompanyInformationDto>> GetAllAsync();
        Task<CompanyInformationDto?> GetByIdAsync(int id);
        Task<CompanyInformationDto> CreateAsync(CompanyInformationDto dto);
        Task<bool> UpdateAsync(int id, CompanyInformationDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
