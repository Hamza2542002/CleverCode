using AutoMapper;
using CleverCode.Data;
using CleverCode.DTO;
using CleverCode.Helpers;
using CleverCode.Interfaces;
using CleverCode.Models;
using medical_app_db.Core.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Net;
namespace CleverCode.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _cloudinaryService;


        public ProjectService(ApplicationDbContext context, IMapper mapper, IImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _cloudinaryService = imageService;
        }
        public async Task<ServiceResult> GetAllProjectsAsync()
        {
            var projects = await _context.Projects.ToListAsync();
            return new ServiceResult ()
            {
                Data = _mapper.Map<List<ProjectDto>>(projects),
                Message = "Projects retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }
        public async Task<ServiceResult> GetSerivceProjects(int serviceId)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Service_ID == serviceId);
            if(service == null)
            {
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Service not found",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            var projectServices =  await _context.ProjectServices
                .Include(p => p.Project)
                .Where(ps => ps.Service_ID == serviceId)
                .Select(ps => ps.Project)
                .ToListAsync();
            return new ServiceResult()
            {
                Data = _mapper.Map<List<ProjectDto>>(projectServices),
                Message = "Projects retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }
        public async Task<ServiceResult> AddProjectToService(int serviceId, int projectId)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Service_ID == serviceId);
            if(service == null)
            {
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Service not found",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Project_ID == projectId);
            if(project == null)
            {
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Project not found",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            var projectService = new Models.ProjectService()
            {
                Project_ID = projectId,
                Service_ID = serviceId
            };
            await _context.ProjectServices.AddAsync(projectService);
            var result = await _context.SaveChangesAsync();
            if(result < 0)
            {
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Couldn't add project to service",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            return new ServiceResult()
            {
                Data = _mapper.Map<ProjectDto>(project),
                Message = "Project added to service successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }
        public async Task<ServiceResult> GetProjectByIdAsync(int id)
        {
            var projects = await _context.Projects.FirstOrDefaultAsync();
            return new ServiceResult()
            {
                Data = _mapper.Map<ProjectDto>(projects),
                Message = projects != null ? "Project retrieved successfully" : "Project not found",
                StatusCode = projects != null ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                Success = projects != null
            };
        }
        public async Task<ServiceResult> CreateProjectAsync(ProjectDto projectDto)
        {
            var projectEntity = _mapper.Map<Models.Project>(projectDto);
            if(projectDto.Image is not null)
                projectEntity.ImageUrl = await _cloudinaryService.UploadImageAsync(projectDto.Image);
            var entity = await _context.Projects.AddAsync(projectEntity);
            await _context.SaveChangesAsync();
            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Service_ID == projectDto.ServiceId);
            if (service == null)
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Service not found",
                    StatusCode = HttpStatusCode.BadRequest
                };

            var projectService = new Models.ProjectService()
            {
                Project_ID = entity.Entity.Project_ID,
                Service_ID = service.Service_ID
            };
            await _context.ProjectServices.AddAsync(projectService);
            var result = await _context.SaveChangesAsync();
            if(result < 0)
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Couldn't create project",
                    StatusCode = HttpStatusCode.BadRequest
                };
            var projectToreturnDto = _mapper.Map<ProjectDto>(entity.Entity);
            projectToreturnDto.ServiceId = service.Service_ID;
            return new ServiceResult()
            {
                Data = projectToreturnDto,
                Message = "Project created successfully",
                StatusCode = HttpStatusCode.Created,
                Success = true
            };
        }
        public async Task<ServiceResult> UpdateProjectAsync(int id, ProjectDto projectDto)
        {
            var project = await _context.Projects
                .Include(p => p.ProjectServices)
                .FirstOrDefaultAsync(p => p.Project_ID == id);
            if (project == null)
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Project not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            project.Rate = projectDto.Rate;
            project.Title = projectDto.Title;
            project.Description = projectDto.Description;
            project.Tech = projectDto.Tech;
            var updatedEntity = _context.Projects.Update(project);
            var result = await _context.SaveChangesAsync();
            if(result < 0)
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Couldn't update project",
                    StatusCode = HttpStatusCode.BadRequest
                };
            return new ServiceResult() 
            { 
                Data = _mapper.Map<ProjectDto>(updatedEntity.Entity),
                Message = "Project created successfully", 
                StatusCode = HttpStatusCode.Created, Success = true 
            };
        }
        public async Task<ServiceResult> DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Project_ID == id);
            if (project == null)
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Project not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            _context.Projects.Remove(project);
            var result = _context.SaveChangesAsync();
            if (result.Result < 0)
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Couldn't delete project",
                    StatusCode = HttpStatusCode.BadRequest
                };
            return new ServiceResult()
            {
                Data = null,
                Message = "Project deleted successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<ServiceResult> DeleteProjectFromService(int service_id, int project_id)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Service_ID == service_id);
            if (service == null)
            {
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Service not found",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Project_ID == project_id);
            if (project == null)
            {
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Project not found",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            var projectService = await _context.ProjectServices
                .FirstOrDefaultAsync(ps => ps.Service_ID == service_id && ps.Project_ID == project_id);
            if (projectService == null)
                {
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Project not found in service",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            _context.ProjectServices.Remove(projectService);
            var result = await _context.SaveChangesAsync();
            if (result < 0)
            {
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Couldn't delete project from service",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            return new ServiceResult()
            {
                Data = _mapper.Map<ProjectDto>(project),
                Message = "Project deleted from service successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }
    }
}
