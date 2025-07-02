using AutoMapper;
using CleverCode.Data;
using CleverCode.DTO;
using CleverCode.Helpers;
using CleverCode.Interfaces;
using CleverCode.Models;
using medical_app_db.Core.Interfaces;
using Microsoft.Build.Evaluation;
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
            var projects = await _context.Projects
                .Include(p => p.ProjectServices)
                .ToListAsync();
            var dto = _mapper.Map<List<ProjectDto>>(projects);
            foreach (var item in dto)
            {
                var projectServices = projects.FirstOrDefault(p => p.Project_ID == item.Project_ID)?.ProjectServices;
                item.Service_ID = projectServices.Select(ps => ps.Service_ID).FirstOrDefault();
            }
            return new ServiceResult ()
            {
                Data = dto,
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
            var existingProjectService = await _context.ProjectServices
                .FirstOrDefaultAsync(ps => ps.Service_ID == serviceId && ps.Project_ID == projectId);
            if(existingProjectService != null)
            {
                return new ServiceResult()
                {
                    Data = _mapper.Map<ProjectDto>(project),
                    Message = "Project already exists in service",
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
            var project = await _context.Projects
                .Include(e => e.ProjectServices)
                .FirstOrDefaultAsync(p => p.Project_ID == id);
            if (project == null)
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Project not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            var dto = _mapper.Map<ProjectDto>(project);
            dto.Service_ID = project.ProjectServices.FirstOrDefault()?.Service_ID ?? 0;
            return new ServiceResult()
            {
                Data = _mapper.Map<ProjectDto>(project),
                Message = project != null ? "Project retrieved successfully" : "Project not found",
                StatusCode = project != null ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                Success = project != null
            };
        }
        public async Task<ServiceResult> CreateProjectAsync(ProjectDto projectDto)
        {
            var projectEntity = _mapper.Map<Models.Project>(projectDto);
            var entity = await _context.Projects.AddAsync(projectEntity);
            
            await _context.SaveChangesAsync();
            if(projectDto.Image is not null)
            {
                foreach (var image in projectDto.Image)
                {
                    projectEntity.ImageUrl += $"{await _cloudinaryService.UploadImageAsync(image, $"project-{entity.Entity.Project_ID}-{image.Name}")},";   
                }
            }
            
            
            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Service_ID == projectDto.Service_ID);
            if (service == null)
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Service not found",
                    StatusCode = HttpStatusCode.BadRequest
                };
            var existingProjectService = await _context.ProjectServices
                .FirstOrDefaultAsync(ps => ps.Service_ID == service.Service_ID && ps.Project_ID == entity.Entity.Project_ID);
            if (existingProjectService != null)
            {
                return new ServiceResult()
                {
                    Data = _mapper.Map<ProjectDto>(entity.Entity),
                    Message = "Project already exists in service",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
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
            projectToreturnDto.Service_ID = service.Service_ID;
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
            project.ImageUrl = string.Empty;
            if (projectDto.Image is not null)
            {
                foreach (var image in projectDto.Image)
                {
                    project.ImageUrl += $"{await _cloudinaryService.UpdateImageAsync(image, $"project-{project.Project_ID}-{image.Name}")},";
                }
            }

            project.Rate = projectDto.Rate;
            project.Title = projectDto.Title;
            project.Description = projectDto.Description;
            project.Tech = projectDto.Tech;
            project.ProjectLink = projectDto.ProjectLink;

            var updatedEntity = _context.Projects.Update(project);

            var projectService = await _context.ProjectServices
                   .FirstOrDefaultAsync(s => s.Project_ID == project.Project_ID);
            if(projectService is not null)
                _context.ProjectServices.Remove(projectService);

            if(await _context.Services.FirstOrDefaultAsync(s => s.Service_ID == projectDto.Service_ID) is null)
            {
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Service not found",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            await AddProjectToService(projectDto.Service_ID, project.Project_ID);
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
                Data = _mapper.Map<ProjectDto>(project),
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
            await _cloudinaryService.DeleteImageAsync(project.Title);
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
