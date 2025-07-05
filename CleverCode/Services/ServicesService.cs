using AutoMapper;
using CleverCode.Data;
using CleverCode.DTO;
using CleverCode.Helpers;
using CleverCode.Interfaces;
using CleverCode.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CleverCode.Services
{
    public class ServicesService : IServicesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ServicesService(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetLanguage()
        {
            var lang = _httpContextAccessor.HttpContext?.Request?.Headers["Accept-Language"].ToString().ToLower();
            return lang == "ar" ? "ar" : "en";
        }

        private LocalizedServiceDto LocalizeService(Service s)
        {
            var lang = GetLanguage();

            return new LocalizedServiceDto
            {
                Service_ID = s.Service_ID,
                Title = lang == "ar" ? (s.TitleAr ?? s.Title ?? string.Empty) : (s.Title ?? s.TitleAr ?? string.Empty),
                Icon = s.Icon ?? string.Empty,
                Description = lang == "ar" ? (s.DescriptionAr ?? s.Description ?? string.Empty) : (s.Description ?? s.DescriptionAr ?? string.Empty),
                Pricing = s.Pricing,
                Feature = s.Feature ?? string.Empty,
                Category = s.Category ?? string.Empty,
                TimeLine = s.TimeLine ?? string.Empty,

                Projects = _mapper.Map<List<ProjectDto>>(s.ProjectServices?.Select(ps => ps.Project).ToList() ?? new List<Project>()),
                Reviews = _mapper.Map<List<ReviewDto>>(s.Reviews ?? new List<Review>()),
                Complaints = _mapper.Map<List<ComplaintDto>>(s.Complaints ?? new List<Complaint>()),
                Messages = _mapper.Map<List<MessageDto>>(s.Messages ?? new List<Message>())
            };
        }


        public async Task<ServiceResult> GetAllServicesAsync()
        {
            var services = await _context.Services
                .Include(s => s.ProjectServices)
                .ThenInclude(ps => ps.Project)
                .Include(s => s.Reviews)
                .Include(s => s.Complaints)
                .Include(s => s.Messages)
                .ToListAsync();

            var localizedServices = services.Select(s => LocalizeService(s)).ToList();

            return new ServiceResult()
            {
                Data = localizedServices,
                Message = "Services retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<ServiceResult> GetServiceByIdAsync(int id)
        {
            var service = await _context.Services
                .Include(s => s.ProjectServices)
                .ThenInclude(ps => ps.Project)
                .Include(s => s.Reviews)
                .Include(s => s.Complaints)
                .Include(s => s.Messages)
                .FirstOrDefaultAsync(s => s.Service_ID == id);

            if (service == null)
                return new ServiceResult()
                {
                    Message = "Service not found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };
            var projects = await _context.ProjectServices
                .Where(ps => ps.Service_ID == service.Service_ID)
                .Select(ps => ps.Project)
                .ToListAsync();
            var reviews = await _context.Reviews
                .Where(r => r.Service_ID == service.Service_ID)
                .ToListAsync();
            var complaints = await _context.Complaints
                .Where(c => c.Service_ID == service.Service_ID)
                .ToListAsync();
            var messages = await _context.Messages
                .Where(m => m.Service_ID == service.Service_ID)
                .ToListAsync();

            var projectsDto = _mapper.Map<List<ProjectDto>>(projects);
            foreach (var item in projectsDto)
            {
                item.Service_ID = service.Service_ID;
            }

            var serviceDto = new ServiceDto
            {
                Service_ID = service.Service_ID,
                Title = service.Title,
                Icon = service.Icon,
                Description = service.Description,
                Pricing = service.Pricing,
                Feature = service.Feature,
                Category = service.Category,
                TimeLine = service.TimeLine,
                Projects = projectsDto,
                Reviews = _mapper.Map<List<ReviewDto>>(reviews),
                Complaints = _mapper.Map<List<ComplaintDto>>(complaints),
                Messages = _mapper.Map<List<MessageDto>>(messages)
            };
            return new ServiceResult()
            {
                Data = LocalizeService(service),
                Message = "Service retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<ServiceResult> CreateServiceAsync(ServiceDto serviceDto)
        {
            var serviceEntity = _mapper.Map<Service>(serviceDto);
            var entity = await _context.Services.AddAsync(serviceEntity);
            var result = await _context.SaveChangesAsync();

            if (result < 1)
            {
                return new ServiceResult()
                {
                    Message = "Couldn't create service",
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            return new ServiceResult()
            {
                Data = _mapper.Map<ServiceDto>(entity.Entity),
                Message = "Service created successfully",
                StatusCode = HttpStatusCode.Created,
                Success = true
            };
        }

        public async Task<ServiceResult> UpdateServiceAsync(int id, ServiceDto serviceDto)
        {
            var service = await _context.Services.FirstOrDefaultAsync(s => s.Service_ID == id);
            if (service == null)
            {
                return new ServiceResult()
                {
                    Message = "Service not found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };
            }
            _context.Services.Update(service);
            var result = await _context.SaveChangesAsync();

            if (result < 1)
            {
                return new ServiceResult()
                {
                    Message = "Couldn't update service",
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            return new ServiceResult()
            {
                Data = _mapper.Map<ServiceDto>(service),
                Message = "Service updated successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<ServiceResult> DeleteServiceAsync(int id)
        {
            var service = await _context.Services.FirstOrDefaultAsync(s => s.Service_ID == id);
            if (service == null)
            {
                return new ServiceResult()
                {
                    Message = "Service not found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };
            }

            _context.Services.Remove(service);
            var result = await _context.SaveChangesAsync();

            if (result < 1)
            {
                return new ServiceResult()
                {
                    Message = "Couldn't delete service",
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            return new ServiceResult()
            {
                Message = "Service deleted successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }
    }
}
