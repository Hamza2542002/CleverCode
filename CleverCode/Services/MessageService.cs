using CleverCode.Data;
using CleverCode.DTO;
using CleverCode.Interfaces;
using CleverCode.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace CleverCode.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MessageService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetLanguage()
        {
            var lang = _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString().ToLower();
            return lang == "ar" ? "ar" : "en";
        }

        public async Task<IEnumerable<object>> GetAllAsync()
        {
            var lang = GetLanguage();

            var messages = await _context.Messages.ToListAsync();

            return messages.Select(m =>
            {
                if (lang == "ar")
                {
                    return new
                    {
                        message_ID = m.Message_ID,
                        messageText = m.MessageTextAr,   // بس العربي
                        name = m.Name,
                        date = m.Date,
                        company = m.Company,
                        email = m.Email,
                        service_ID = m.Service_ID
                    };
                }
                else // en
                {
                    return new
                    {
                        message_ID = m.Message_ID,
                        messageText = m.MessageTextEn,   // بس الإنجليزي
                        name = m.Name,
                        date = m.Date,
                        company = m.Company,
                        email = m.Email,
                        service_ID = m.Service_ID
                    };
                }
            });
        }


        public async Task<object?> GetByIdAsync(int id)
        {
            var lang = GetLanguage();

            var message = await _context.Messages.FindAsync(id);
            if (message == null) return null;

            if (lang == "ar")
            {
                return new
                {
                    message_ID = message.Message_ID,
                    messageText = message.MessageTextAr,
                    name = message.Name,
                    date = message.Date,
                    company = message.Company,
                    email = message.Email,
                    service_ID = message.Service_ID
                };
            }
            else
            {
                return new
                {
                    message_ID = message.Message_ID,
                    messageText = message.MessageTextEn,
                    name = message.Name,
                    date = message.Date,
                    company = message.Company,
                    email = message.Email,
                    service_ID = message.Service_ID
                };
            }
        }

        public async Task<MessageDto> CreateAsync(MessageDto dto)
        {
            var message = new Message
            {
                MessageTextEn = dto.MessageTextEn,
                MessageTextAr = dto.MessageTextAr,
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

            message.MessageTextEn = dto.MessageTextEn;
            message.MessageTextAr = dto.MessageTextAr;
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
