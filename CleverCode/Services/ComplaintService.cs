using CleverCode.Data;
using CleverCode.DTO;
using CleverCode.Interfaces;
using CleverCode.Models;
using Microsoft.EntityFrameworkCore;

namespace CleverCode.Services
{
    public class ComplaintService : IComplaintService
    {
        private readonly ApplicationDbContext _context;

        public ComplaintService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ComplaintDto> CreateAsync(ComplaintDto dto, string? lang = "en")
        {
            var complaint = new Complaint
            {
                Type_AR = lang == "ar" ? dto.Type : null,
                Type_EN = lang == "en" ? dto.Type : null,
                Description_AR = lang == "ar" ? dto.Description : null,
                Description_EN = lang == "en" ? dto.Description : null,
                Status_AR = lang == "ar" ? dto.Status : null,
                Status_EN = lang == "en" ? dto.Status : null,
                Priority_AR = lang == "ar" ? dto.Priority : null,
                Priority_EN = lang == "en" ? dto.Priority : null,
                Name_AR = lang == "ar" ? dto.Name : null,
                Name_EN = lang == "en" ? dto.Name : null,
                Date = dto.Date,
                Service_ID = dto.Service_ID
            };

            _context.Complaints.Add(complaint);
            await _context.SaveChangesAsync();

            dto.Complaint_ID = complaint.Complaint_ID;
            return dto;
        }

        public async Task<IEnumerable<ComplaintDto>> GetAllAsync(string? lang = "en")
        {
            return await _context.Complaints
                .Select(c => new ComplaintDto
                {
                    Complaint_ID = c.Complaint_ID,
                    Type = lang == "ar" ? c.Type_AR : c.Type_EN,
                    Description = lang == "ar" ? c.Description_AR : c.Description_EN,
                    Status = lang == "ar" ? c.Status_AR : c.Status_EN,
                    Priority = lang == "ar" ? c.Priority_AR : c.Priority_EN,
                    Name = lang == "ar" ? c.Name_AR : c.Name_EN,
                    Date = c.Date,
                    Service_ID = c.Service_ID
                })
                .ToListAsync();
        }

        public async Task<ComplaintDto?> GetByIdAsync(int id, string? lang = "en")
        {
            var c = await _context.Complaints.FindAsync(id);
            if (c == null) return null;

            return new ComplaintDto
            {
                Complaint_ID = c.Complaint_ID,
                Type = lang == "ar" ? c.Type_AR : c.Type_EN,
                Description = lang == "ar" ? c.Description_AR : c.Description_EN,
                Status = lang == "ar" ? c.Status_AR : c.Status_EN,
                Priority = lang == "ar" ? c.Priority_AR : c.Priority_EN,
                Name = lang == "ar" ? c.Name_AR : c.Name_EN,
                Date = c.Date,
                Service_ID = c.Service_ID
            };
        }

        public async Task<bool> UpdateAsync(int id, ComplaintDto dto)
        {
            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint == null) return false;

            complaint.Type_AR = dto.Type;
            complaint.Type_EN = dto.Type;
            complaint.Description_AR = dto.Description;
            complaint.Description_EN = dto.Description;
            complaint.Status_AR = dto.Status;
            complaint.Status_EN = dto.Status;
            complaint.Priority_AR = dto.Priority;
            complaint.Priority_EN = dto.Priority;
            complaint.Name_AR = dto.Name;
            complaint.Name_EN = dto.Name;
            complaint.Date = dto.Date;
            complaint.Service_ID = dto.Service_ID;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint == null) return false;

            _context.Complaints.Remove(complaint);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
