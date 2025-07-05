
using AutoMapper;
using CleverCode.Data;
using CleverCode.DTO;
using CleverCode.Helpers;
using CleverCode.Interfaces;
using CleverCode.Models;
using medical_app_db.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.IO;

namespace CleverCode.Services
{
    public class ProjectServices : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IImageService _cloudinaryService;

        public ProjectServices(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, IImageService cloudinaryService)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cloudinaryService = cloudinaryService;
        }

        private string GetLanguage()
        {
            var lang = _httpContextAccessor.HttpContext?.Request?.Headers["Accept-Language"].ToString().ToLower();
            return lang == "ar" ? "ar" : "en";
        }

        private LocalizedProjectDto LocalizeProject(Project p)
        {
            var lang = GetLanguage();
            return new LocalizedProjectDto
            {
                Project_ID = p.Project_ID,
                Title = lang == "ar" ? p.TitleAr : p.TitleEn,
                Description = lang == "ar" ? p.DescriptionAr : p.DescriptionEn,
                Rate = p.Rate,
                Tech = p.Tech,
                ProjectLink = p.ProjectLink,
                ImageUrl = p.ImageUrl
            };
        }

        public async Task<ServiceResult> GetAllProjectsAsync()
        {
            var projects = await _context.Projects.Include(p => p.ProjectServices).ToListAsync();
            var localized = projects.Select(p => LocalizeProject(p)).ToList();

            return new ServiceResult
            {
                Data = localized,
                Message = "Projects retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<ServiceResult> GetProjectByIdAsync(int id)
        {
            var project = await _context.Projects.Include(e => e.ProjectServices).FirstOrDefaultAsync(p => p.Project_ID == id);
            if (project == null)
                return new ServiceResult
                {
                    Data = null,
                    Message = "Project not found",
                    StatusCode = HttpStatusCode.NotFound
                };

            var dto = _mapper.Map<ProjectDto>(project);
            dto.Service_ID = project.ProjectServices.FirstOrDefault()?.Service_ID ?? 0;

            return new ServiceResult
            {
                Data = dto,
                Message = "Project retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<ServiceResult> GetSerivceProjects(int serviceId)
        {
            var service = await _context.Services.FindAsync(serviceId);
            if (service == null)
            {
                return new ServiceResult
                {
                    Message = "Service not found",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            var projects = await _context.ProjectServices.Include(ps => ps.Project)
                .Where(ps => ps.Service_ID == serviceId)
                .Select(ps => ps.Project)
                .ToListAsync();

            var localized = projects.Select(p => LocalizeProject(p)).ToList();

            return new ServiceResult
            {
                Data = localized,
                Message = "Projects retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<ServiceResult> AddProjectToService(int serviceId, int projectId)
        {
            var service = await _context.Services.FindAsync(serviceId);
            var project = await _context.Projects.FindAsync(projectId);

            if (service == null || project == null)
            {
                return new ServiceResult
                {
                    Message = "Service or Project not found",
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            var existingRelation = await _context.ProjectServices
                .FirstOrDefaultAsync(ps => ps.Service_ID == serviceId && ps.Project_ID == projectId);

            if (existingRelation != null)
            {
                return new ServiceResult
                {
                    Message = "Project already exists in service",
                    StatusCode = HttpStatusCode.Conflict,
                    Success = false
                };
            }

            var relation = new ProjectService { Service_ID = serviceId, Project_ID = projectId };
            await _context.ProjectServices.AddAsync(relation);
            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Data = LocalizeProject(project),
                Message = "Project added to service",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<ServiceResult> DeleteProjectFromService(int serviceId, int projectId)
        {
            var relation = await _context.ProjectServices
                .FirstOrDefaultAsync(ps => ps.Service_ID == serviceId && ps.Project_ID == projectId);

            if (relation == null)
            {
                return new ServiceResult
                {
                    Message = "Relation not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            _context.ProjectServices.Remove(relation);
            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Message = "Project removed from service",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }
        public async Task<ServiceResult> DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Project_ID == id);
            if (project == null)
            {
                return new ServiceResult
                {
                    Data = null,
                    Message = "Project not found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };
            }

            if (!string.IsNullOrEmpty(project.ImageUrl))
            {
                var imageUrls = project.ImageUrl
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(url => url.Trim());

                foreach (var imageUrl in imageUrls)
                {
                    await _cloudinaryService.DeleteImageAsync(imageUrl);
                }
            }

           
            _context.Projects.Remove(project);
            var result = await _context.SaveChangesAsync();

            if (result < 1)
            {
                return new ServiceResult
                {
                    Data = null,
                    Message = "Couldn't delete project",
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            return new ServiceResult
            {
                Data = null,
                Message = "Project deleted successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }


        public async Task<ServiceResult> CreateProjectAsync(ProjectDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);

            if (projectDto.Image != null && projectDto.Image.Any())
            {
                project.ImageUrl = string.Empty;
                foreach (var image in projectDto.Image)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(image, $"project-{project.Project_ID}-{image.Name}");
                    project.ImageUrl += imageUrl + ",";
                }
            }

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            if (projectDto.Service_ID != 0)
            {
                var service = await _context.Services.FindAsync(projectDto.Service_ID);
                if (service == null)
                    return new ServiceResult
                    {
                        Data = null,
                        Message = "Service not found",
                        StatusCode = HttpStatusCode.BadRequest
                    };

                var relation = new ProjectService
                {
                    Project_ID = project.Project_ID,
                    Service_ID = projectDto.Service_ID
                };

                await _context.ProjectServices.AddAsync(relation);
                await _context.SaveChangesAsync();
            }

            return new ServiceResult
            {
                Data = LocalizeProject(project),
                Message = "Project created",
                StatusCode = HttpStatusCode.Created,
                Success = true
            };
        }

        public async Task<ServiceResult> UpdateProjectAsync(int id, ProjectDto projectDto)
        {
            var project = await _context.Projects.Include(p => p.ProjectServices).FirstOrDefaultAsync(p => p.Project_ID == id);

            if (project == null)
            {
                return new ServiceResult
                {
                    Message = "Project not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            if (projectDto.Image != null && projectDto.Image.Any())
            {
                project.ImageUrl = string.Empty;
                foreach (var image in projectDto.Image)
                {
                    project.ImageUrl += await _cloudinaryService.UpdateImageAsync(image, $"project-{project.Project_ID}-{image.Name}") + ",";
                }
            }

            project.TitleAr = projectDto.TitleAr;
            project.TitleEn = projectDto.TitleEn;
            project.DescriptionAr = projectDto.DescriptionAr;
            project.DescriptionEn = projectDto.DescriptionEn;
            project.Rate = projectDto.Rate;
            project.Tech = projectDto.Tech;
            project.ProjectLink = projectDto.ProjectLink;

            _context.Projects.Update(project);

            if (projectDto.Service_ID != 0)
            {
                var existingRelation = project.ProjectServices.FirstOrDefault();

                if (existingRelation == null)
                {
                    var newRelation = new ProjectService
                    {
                        Project_ID = project.Project_ID,
                        Service_ID = projectDto.Service_ID
                    };
                    await _context.ProjectServices.AddAsync(newRelation);
                }
                else if (existingRelation.Service_ID != projectDto.Service_ID)
                {
                    existingRelation.Service_ID = projectDto.Service_ID;
                    _context.ProjectServices.Update(existingRelation);
                }
            }

            var result = await _context.SaveChangesAsync();

            if (result < 0)
            {
                return new ServiceResult
                {
                    Data = null,
                    Message = "Couldn't update project",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            var dto = _mapper.Map<ProjectDto>(project);
            dto.Service_ID = project.ProjectServices.FirstOrDefault()?.Service_ID ?? 0;

            return new ServiceResult
            {
                Data = dto,
                Message = "Project updated successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }
    }
}
