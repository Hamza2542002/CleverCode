using CleverCode.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleverCode.Interfaces
{
    public interface ICompanyInformationService
    {
        // قراءة تعتمد على Localized DTO (حسب لغة الهيدر)
        Task<List<CompanyInformationLocalizedDto>> GetAllAsync();
        Task<CompanyInformationLocalizedDto?> GetByIdAsync(int id);

        // تعديل/إنشاء تعتمد على DTO الكامل
        Task<CompanyInformationDto> CreateAsync(CompanyInformationDto dto);
        Task<bool> UpdateAsync(int id, CompanyInformationDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
