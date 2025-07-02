using AutoMapper;
using CleverCode.Data;
using CleverCode.DTO;
using CleverCode.Helpers;
using CleverCode.Interfaces;
using CleverCode.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CleverCode.Services
{
    public class ServicesService : IServicesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ServicesService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            var serviceDtos = _mapper.Map<List<ServiceDto>>(services);
            foreach (var item in services)
            {
                var serviceDto = serviceDtos.FirstOrDefault(s => s.Service_ID == item.Service_ID);
                serviceDto.Projects = _mapper.Map<List<ProjectDto>>(item.ProjectServices.Select(ps => ps.Project).ToList());
                foreach (var project in serviceDto.Projects)
                {
                    project.Service_ID = item.Service_ID;
                }
            }
            var projects = await _context.ProjectServices
                .Select(s => s.Project)
                .ToListAsync();
            return new ServiceResult()
            {
                Data = serviceDtos,
                Message = "Services retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<ServiceResult> GetServiceByIdAsync(int id)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Service_ID == id);
            if (service == null)
                return new ServiceResult()
                {
                    Message = "Service not found",
                    StatusCode = HttpStatusCode.NotFound
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
                Data = serviceDto,
                Message = service != null ? "Service retrieved successfully" : "Service not found",
                StatusCode = service != null ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                Success = service != null
            };
        }

        public async Task<ServiceResult> CreateServiceAsync(ServiceDto serviceDto)
        {
            var serviceEntity = _mapper.Map<Service>(serviceDto);
            var entity = await _context.Services.AddAsync(serviceEntity);
            var result = await _context.SaveChangesAsync();
            if (result < 0)
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
            if (service is null)
            {
                return new ServiceResult()
                {
                    Message = "Service not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }
            service.Category = serviceDto.Category;
            service.Description = serviceDto.Description;
            service.Feature = serviceDto.Feature;
            service.Icon = serviceDto.Icon;
            service.Pricing = serviceDto.Pricing;
            service.TimeLine = serviceDto.TimeLine;
            service.Title = serviceDto.Title;

            _context.Services.Update(service);
            var result = await _context.SaveChangesAsync();
            if (result < 0)
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
            if (service is null)
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
            if (result < 0)
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
