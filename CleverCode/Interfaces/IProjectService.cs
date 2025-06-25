using CleverCode.DTO;
using CleverCode.Helpers;

namespace CleverCode.Interfaces
{
    public interface IProjectService
    {
        Task<ServiceResult> GetAllProjectsAsync();
        Task<ServiceResult> GetSerivceProjects(int serviceId);
        Task<ServiceResult> GetProjectByIdAsync(int id);
        Task<ServiceResult> AddProjectToService(int serviceId, int projectId);
        Task<ServiceResult> CreateProjectAsync(ProjectDto project);
        Task<ServiceResult> UpdateProjectAsync(int id, ProjectDto project);
        Task<ServiceResult> DeleteProjectAsync(int id);
    }
}
