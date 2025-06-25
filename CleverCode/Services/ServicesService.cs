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
            var services = await _context.Services.ToListAsync();
            return new ServiceResult()
            {
                Data = _mapper.Map<List<ServiceDto>>(services),
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
            var projectServices = await _context.ProjectServices
                .Where(ps => ps.Service_ID == service.Service_ID)
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
            return new ServiceResult()
            {
                Data = _mapper.Map<ServiceDto>(service),
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
