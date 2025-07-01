using CleverCode.Data;
using CleverCode.DTO;
using CleverCode.Interfaces;
using CleverCode.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CleverCode.Services
{
    public class FAQService : IFAQService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FAQService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // دالة لجلب اللغة من الهيدر
        private string GetLanguage()
        {
            var lang = _httpContextAccessor.HttpContext?.Request?.Headers["Accept-Language"].ToString().ToLower();
            return lang == "ar" ? "ar" : "en";
        }

        // GET All FAQs بلغة الهيدر
        public async Task<List<LocalizedFaqDto>> GetAllAsync()
        {
            var lang = GetLanguage();
            var faqs = await _context.FAQs.ToListAsync();

            var localizedFaqs = faqs.Select(faq => new LocalizedFaqDto
            {
                Faq_ID = faq.FAQ_ID,
                Questions = lang == "ar" ? faq.QuestionsAr : faq.QuestionsEn,
                Answer = lang == "ar" ? faq.AnswerAr : faq.AnswerEn
            }).ToList();

            return localizedFaqs;
        }

        // GET FAQ by ID بلغة الهيدر
        public async Task<LocalizedFaqDto?> GetByIdAsync(int id)
        {
            var lang = GetLanguage();
            var faq = await _context.FAQs.FindAsync(id);

            if (faq == null)
                return null;

            return new LocalizedFaqDto
            {
                Faq_ID = faq.FAQ_ID,
                Questions = lang == "ar" ? faq.QuestionsAr : faq.QuestionsEn,
                Answer = lang == "ar" ? faq.AnswerAr : faq.AnswerEn
            };
        }

        // POST Create FAQ (يحفظ النصوص بالعربي والانجليزي)
        public async Task<FAQDto> CreateAsync(FAQDto dto)
        {
            var faq = new FAQ
            {
                QuestionsAr = dto.QuestionsAr,
                QuestionsEn = dto.QuestionsEn,
                AnswerAr = dto.AnswerAr,
                AnswerEn = dto.AnswerEn
            };

            _context.FAQs.Add(faq);
            await _context.SaveChangesAsync();

            dto.FAQ_ID = faq.FAQ_ID;
            dto.Questions = GetLanguage() == "ar" ? faq.QuestionsAr : faq.QuestionsEn;
            dto.Answer = GetLanguage() == "ar" ? faq.AnswerAr : faq.AnswerEn;

            return dto;
        }

        // PUT تحديث البيانات
        public async Task<bool> UpdateAsync(int id, FAQDto dto)
        {
            var faq = await _context.FAQs.FindAsync(id);
            if (faq == null) return false;

            faq.QuestionsAr = dto.QuestionsAr;
            faq.QuestionsEn = dto.QuestionsEn;
            faq.AnswerAr = dto.AnswerAr;
            faq.AnswerEn = dto.AnswerEn;

            await _context.SaveChangesAsync();
            return true;
        }

        // DELETE حذف
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
