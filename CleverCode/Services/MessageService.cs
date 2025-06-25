using CleverCode.Data;
using CleverCode.DTO;
using CleverCode.Interfaces;
using CleverCode.Models;
using Microsoft.EntityFrameworkCore;

namespace CleverCode.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _context;

        public MessageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MessageDto>> GetAllAsync()
        {
            var messages = await _context.Messages.ToListAsync();
            return messages.Select(m => new MessageDto
            {
                Message_ID = m.Message_ID,
                MessageText = m.MessageText,
                Name = m.Name,
                Date = m.Date,
                Company = m.Company,
                Email = m.Email,
                Service_ID = m.Service_ID
            });
        }

        public async Task<MessageDto?> GetByIdAsync(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null) return null;

            return new MessageDto
            {
                Message_ID = message.Message_ID,
                MessageText = message.MessageText,
                Name = message.Name,
                Date = message.Date,
                Company = message.Company,
                Email = message.Email,
                Service_ID = message.Service_ID
            };
        }

        public async Task<MessageDto> CreateAsync(MessageDto dto)
        {
            var message = new Message
            {
                MessageText = dto.MessageText,
                Name = dto.Name,
                Date = dto.Date,
                Company = dto.Company,
                Email = dto.Email,
                Service_ID = dto.Service_ID
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            dto.Message_ID = message.Message_ID;
            return dto;
        }

        public async Task<bool> UpdateAsync(int id, MessageDto dto)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null) return false;

            message.MessageText = dto.MessageText;
            message.Name = dto.Name;
            message.Date = dto.Date;
            message.Company = dto.Company;
            message.Email = dto.Email;
            message.Service_ID = dto.Service_ID;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null) return false;

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

