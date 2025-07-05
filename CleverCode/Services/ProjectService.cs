using AutoMapper;
using CleverCode.Data;
using CleverCode.DTO;
using CleverCode.Helpers;
using CleverCode.Interfaces;
using CleverCode.Models;
using medical_app_db.Core.Interfaces;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CleverCode.Services
{
    public class ProjectServices : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IImageService _imageService;

        public ProjectServices(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, IImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _imageService = imageService;
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
            var projects = await _context.Projects.ToListAsync();
            var localized = projects.Select(p => LocalizeProject(p)).ToList();

            return new ServiceResult
            {
                Data = dto,
                Data = localized,
                Message = "Projects retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<ServiceResult> GetProjectByIdAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return new ServiceResult
                {
                    Message = "Project not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            return new ServiceResult
            {
                Data = LocalizeProject(project),
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

            var projects = await _context.ProjectServices
                .Include(ps => ps.Project)
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
                    Message = "Relation already exists",
                    StatusCode = HttpStatusCode.Conflict,
                    Success = false
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

        public async Task<ServiceResult> DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return new ServiceResult
                {
                    Message = "Project not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Message = "Project deleted",
                StatusCode = HttpStatusCode.OK,
                Success = true
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
            var project = _mapper.Map<Project>(projectDto);

            // حفظ الصورة إذا وجدت
            if (projectDto.Image != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(projectDto.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
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
                    await projectDto.Image.CopyToAsync(stream);
                }

                project.ImageUrl = "/uploads/" + fileName;
            }

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            // ربط المشروع بالخدمة إذا تم تحديد Service_ID
            if (projectDto.Service_ID != 0)
            {
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
            var project = await _context.Projects
                .Include(p => p.ProjectServices)
                .FirstOrDefaultAsync(p => p.Project_ID == id);

            if (project == null)
            {
                return new ServiceResult
                {
                    Message = "Project not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            if(projectDto.Image is not null)
            {
                project.ImageUrl = string.Empty;
                if (projectDto.Image is not null)
                {
                    foreach (var image in projectDto.Image)
                    {
                        project.ImageUrl += $"{await _cloudinaryService.UpdateImageAsync(image, $"project-{project.Project_ID}-{image.Name}")},";
                    }
                }
            }

            }

            // تحديث الحقول الأساسية
            project.TitleAr = projectDto.TitleAr;
            project.TitleEn = projectDto.TitleEn;
            project.DescriptionAr = projectDto.DescriptionAr;
            project.DescriptionEn = projectDto.DescriptionEn;
            project.Rate = projectDto.Rate;
            project.Tech = projectDto.Tech;
            project.ProjectLink = projectDto.ProjectLink;

            var updatedEntity = _context.Projects.Update(project);

            if(await _context.Services.FirstOrDefaultAsync(s => s.Service_ID == projectDto.Service_ID) is null)
            {
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Service not found",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            var projectService = await _context.ProjectServices
                   .FirstOrDefaultAsync(s => s.Project_ID == project.Project_ID);


            
            if(projectService is not null)
                _context.ProjectServices.Remove(projectService);

            //if(projectService is not null && projectService.Service_ID != projectDto.Service_ID)
                await _context.ProjectServices.AddAsync(new Models.ProjectService()
                {
                    Service_ID = projectDto.Service_ID,
                    Project_ID = project.Project_ID
                });
            var result = await _context.SaveChangesAsync();

            if(result < 0)
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Couldn't update project",
                    StatusCode = HttpStatusCode.BadRequest
                };
            var dto = _mapper.Map<ProjectDto>(updatedEntity.Entity);
            dto.Service_ID = updatedEntity.Entity.ProjectServices.FirstOrDefault()?.Service_ID ?? 0;
            return new ServiceResult() 
            { 
                Data = dto,
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
            project.ProjectLink = projectDto.ProjectLink;

            // حفظ الصورة الجديدة (لو موجودة)
            if (projectDto.Image != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(projectDto.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await projectDto.Image.CopyToAsync(stream);
                }

                project.ImageUrl = "/uploads/" + fileName;
            }

            // تحديث العلاقة بالخدمة (لو تغيرت)
            if (projectDto.Service_ID != 0)
            {
                var existingRelation = project.ProjectServices.FirstOrDefault();
                if (existingRelation == null)
                {
                    // لا توجد علاقة، فأنشئ واحدة
                    var newRelation = new ProjectService
                    {
                        Project_ID = project.Project_ID,
                        Service_ID = projectDto.Service_ID
                    };
                    await _context.ProjectServices.AddAsync(newRelation);
                }
                else if (existingRelation.Service_ID != projectDto.Service_ID)
                {
                    // إذا الخدمة تغيرت، حدثها
                    existingRelation.Service_ID = projectDto.Service_ID;
                    _context.ProjectServices.Update(existingRelation);
                }
            }

            _context.Projects.Update(project);
            await _context.SaveChangesAsync();

            // إرجاع نسخة مخصصة
            return new ServiceResult
            {
                Data = LocalizeProject(project),
                Message = "Project updated",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

    }
}
