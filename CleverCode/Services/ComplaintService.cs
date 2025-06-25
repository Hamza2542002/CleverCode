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

        public async Task<ComplaintDto> CreateAsync(ComplaintDto dto)
        {
            var complaint = new Complaint
            {
                Type = dto.Type,
                Description = dto.Description,
                Status = dto.Status,
                Priority = dto.Priority,
                Name = dto.Name,
                Date = dto.Date,
                Service_ID = dto.Service_ID
            };

            _context.Complaints.Add(complaint);
            await _context.SaveChangesAsync();

            dto.Complaint_ID = complaint.Complaint_ID;
            return dto;
        }

        public async Task<IEnumerable<ComplaintDto>> GetAllAsync()
        {
            Console.WriteLine("GetAllAsync called");
            return await _context.Complaints
                .Select(c => new ComplaintDto
                {
                    Complaint_ID = c.Complaint_ID,
                    Type = c.Type,
                    Description = c.Description,
                    Status = c.Status,
                    Priority = c.Priority,
                    Name = c.Name,
                    Date = c.Date,
                    Service_ID = c.Service_ID
                })
                .ToListAsync();
        }


        public async Task<ComplaintDto?> GetByIdAsync(int id)
        {
            var c = await _context.Complaints.FindAsync(id);
            if (c == null) return null;

            return new ComplaintDto
            {
                Complaint_ID = c.Complaint_ID,
                Type = c.Type,
                Description = c.Description,
                Status = c.Status,
                Priority = c.Priority,
                Name = c.Name,
                Date = c.Date,
                Service_ID = c.Service_ID
            };
        }

        public async Task<bool> UpdateAsync(int id, ComplaintDto dto)
        {
            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint == null) return false;

            complaint.Type = dto.Type;
            complaint.Description = dto.Description;
            complaint.Status = dto.Status;
            complaint.Priority = dto.Priority;
            complaint.Name = dto.Name;
            complaint.Date = dto.Date;
            complaint.Service_ID = dto.Service_ID;

            _context.Complaints.Update(complaint);
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

