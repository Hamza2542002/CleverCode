using CleverCode.DTO;
using CleverCode.Helpers;

namespace CleverCode.Interfaces
{
    public interface IServicesService
    {
        Task<ServiceResult> GetAllServicesAsync();
        Task<ServiceResult> GetServiceByIdAsync(int id);
        Task<ServiceResult> CreateServiceAsync(ServiceDto ServiceDto);
        Task<ServiceResult> UpdateServiceAsync(int id, ServiceDto ServiceDto);
        Task<ServiceResult> DeleteServiceAsync(int id);
    }
}
