using CleverCode.Data;
using CleverCode.DTO;
using CleverCode.Interfaces;
using CleverCode.Models;
using Microsoft.EntityFrameworkCore;

namespace CleverCode.Services
{
    public class FAQService : IFAQService
    {
        private readonly ApplicationDbContext _context;

        public FAQService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FAQDto>> GetAllAsync()
        {
            var faqs = await _context.FAQs.ToListAsync();
            return faqs.Select(f => new FAQDto
            {
                FAQ_ID = f.FAQ_ID,
                Questions = f.Questions,
                Answer = f.Answer
            });
        }

        public async Task<FAQDto?> GetByIdAsync(int id)
        {
            var faq = await _context.FAQs.FindAsync(id);
            if (faq == null) return null;

            return new FAQDto
            {
                FAQ_ID = faq.FAQ_ID,
                Questions = faq.Questions,
                Answer = faq.Answer
            };
        }

        public async Task<FAQDto> CreateAsync(FAQDto dto)
        {
            var faq = new FAQ
            {
                Questions = dto.Questions,
                Answer = dto.Answer
            };

            _context.FAQs.Add(faq);
            await _context.SaveChangesAsync();

            dto.FAQ_ID = faq.FAQ_ID;
            return dto;
        }

        public async Task<bool> UpdateAsync(int id, FAQDto dto)
        {
            var faq = await _context.FAQs.FindAsync(id);
            if (faq == null) return false;

            faq.Questions = dto.Questions;
            faq.Answer = dto.Answer;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var faq = await _context.FAQs.FindAsync(id);
            if (faq == null) return false;

            _context.FAQs.Remove(faq);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

