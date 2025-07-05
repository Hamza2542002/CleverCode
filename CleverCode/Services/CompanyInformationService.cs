using CleverCode.Data;
using CleverCode.DTO;
using CleverCode.Interfaces;
using CleverCode.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CleverCode.Services
{
    public class CompanyInformationService : ICompanyInformationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompanyInformationService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetLanguage()
        {
            var lang = _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString().ToLower();
            return lang == "ar" ? "ar" : "en";
        }


        public async Task<List<CompanyInformationLocalizedDto>> GetAllAsync()
        {
            var lang = GetLanguage();

            var companies = await _context.CompanyInformations
                .Include(c => c.Values)
                .Include(c => c.ContactInfo)
                .ToListAsync();

            return companies.Select(company => new CompanyInformationLocalizedDto
            {
                Company_ID = company.Company_ID,
                ResponseTime = company.ResponseTime,
                Logo = company.Logo,
                SocialLink = company.SocialLink,

                Name = lang == "ar" ? company.NameAr : company.NameEn,
                Description = lang == "ar" ? company.DescriptionAr : company.DescriptionEn,
                Mission = lang == "ar" ? company.MissionAr : company.MissionEn,
                Vision = lang == "ar" ? company.VisionAr : company.VisionEn,
                Story = lang == "ar" ? company.StoryAr : company.StoryEn,

                Values = company.Values?.Select(v => new CompanyValueLocalizedDto
                {
                    Id = v.Id,
                    Name = lang == "ar" ? v.NameAr : v.NameEn,
                    Description = lang == "ar" ? v.DescriptionAr : v.DescriptionEn
                }).ToList(),

                ContactInfo = company.ContactInfo == null ? null : new ContactInfoLocalizedDto
                {
                    Email = company.ContactInfo.Email,
                    Phone = company.ContactInfo.Phone,
                    Address = lang == "ar" ? company.ContactInfo.AddressAr : company.ContactInfo.AddressEn
                }
            }).ToList();
        }

        public async Task<CompanyInformationLocalizedDto?> GetByIdAsync(int id)
        {
            var lang = GetLanguage();

            var company = await _context.CompanyInformations
                .Include(c => c.Values)
                .Include(c => c.ContactInfo)
                .FirstOrDefaultAsync(c => c.Company_ID == id);

            if (company == null) return null;

            return new CompanyInformationLocalizedDto
            {
                Company_ID = company.Company_ID,
                ResponseTime = company.ResponseTime,
                Logo = company.Logo,
                SocialLink = company.SocialLink,

                Name = lang == "ar" ? company.NameAr : company.NameEn,
                Description = lang == "ar" ? company.DescriptionAr : company.DescriptionEn,
                Mission = lang == "ar" ? company.MissionAr : company.MissionEn,
                Vision = lang == "ar" ? company.VisionAr : company.VisionEn,
                Story = lang == "ar" ? company.StoryAr : company.StoryEn,

                Values = company.Values?.Select(v => new CompanyValueLocalizedDto
                {
                    Id = v.Id,
                    Name = lang == "ar" ? v.NameAr : v.NameEn,
                    Description = lang == "ar" ? v.DescriptionAr : v.DescriptionEn
                }).ToList(),

                ContactInfo = company.ContactInfo == null ? null : new ContactInfoLocalizedDto
                {
                    Email = company.ContactInfo.Email,
                    Phone = company.ContactInfo.Phone,
                    Address = lang == "ar" ? company.ContactInfo.AddressAr : company.ContactInfo.AddressEn
                }
            };
        }

        // CREATE جديد (يحفظ بالعربي والإنجليزي)
        public async Task<CompanyInformationDto> CreateAsync(CompanyInformationDto dto)
        {
            var company = new CompanyInformation
            {
                NameEn = dto.NameEn,
                NameAr = dto.NameAr,
                DescriptionEn = dto.DescriptionEn,
                DescriptionAr = dto.DescriptionAr,
                MissionEn = dto.MissionEn,
                MissionAr = dto.MissionAr,
                VisionEn = dto.VisionEn,
                VisionAr = dto.VisionAr,
                StoryEn = dto.StoryEn,
                StoryAr = dto.StoryAr,
                ResponseTime = dto.ResponseTime,
                Logo = dto.Logo,
                SocialLink = dto.SocialLink,
                Values = dto.Values?.Select(v => new CompanyValues
                {
                    NameEn = v.NameEn,
                    NameAr = v.NameAr,
                    DescriptionEn = v.DescriptionEn,
                    DescriptionAr = v.DescriptionAr
                }).ToList(),
                ContactInfo = dto.ContactInfo == null ? null : new ContactInfo
                {
                    Email = dto.ContactInfo.Email,
                    Phone = dto.ContactInfo.Phone,
                    AddressEn = dto.ContactInfo.AddressEn,
                    AddressAr = dto.ContactInfo.AddressAr
                }
            };

            _context.CompanyInformations.Add(company);
            await _context.SaveChangesAsync();

            // جلب الكائن بعد الحفظ (بشمل قيم القيم وبيانات الاتصال)
            await _context.Entry(company).Collection(c => c.Values).LoadAsync();
            if (company.ContactInfo != null)
                await _context.Entry(company).Reference(c => c.ContactInfo).LoadAsync();

            return MapToDto(company);
        }

        private CompanyInformationDto MapToDto(CompanyInformation company)
        {
            return new CompanyInformationDto
            {
                Company_ID = company.Company_ID,
                NameEn = company.NameEn,
                NameAr = company.NameAr,
                DescriptionEn = company.DescriptionEn,
                DescriptionAr = company.DescriptionAr,
                MissionEn = company.MissionEn,
                MissionAr = company.MissionAr,
                VisionEn = company.VisionEn,
                VisionAr = company.VisionAr,
                StoryEn = company.StoryEn,
                StoryAr = company.StoryAr,
                ResponseTime = company.ResponseTime,
                Logo = company.Logo,
                SocialLink = company.SocialLink,
                Values = company.Values?.Select(v => new CompanyValuesDto
                {
                    NameEn = v.NameEn,
                    NameAr = v.NameAr,
                    DescriptionEn = v.DescriptionEn,
                    DescriptionAr = v.DescriptionAr
                }).ToList(),
                ContactInfo = company.ContactInfo == null ? null : new ContactInfoDto
                {
                    Email = company.ContactInfo.Email,
                    Phone = company.ContactInfo.Phone,
                    AddressEn = company.ContactInfo.AddressEn,
                    AddressAr = company.ContactInfo.AddressAr
                }
            };
        }


        // UPDATE (يحفظ بالعربي والإنجليزي)
        public async Task<bool> UpdateAsync(int id, CompanyInformationDto dto)
        {
            var company = await _context.CompanyInformations
                .Include(c => c.Values)
                .Include(c => c.ContactInfo)
                .FirstOrDefaultAsync(c => c.Company_ID == id);

            if (company == null) return false;

            company.NameEn = dto.NameEn;
            company.NameAr = dto.NameAr;
            company.DescriptionEn = dto.DescriptionEn;
            company.DescriptionAr = dto.DescriptionAr;
            company.MissionEn = dto.MissionEn;
            company.MissionAr = dto.MissionAr;
            company.VisionEn = dto.VisionEn;
            company.VisionAr = dto.VisionAr;
            company.StoryEn = dto.StoryEn;
            company.StoryAr = dto.StoryAr;
            company.ResponseTime = dto.ResponseTime;
            company.Logo = dto.Logo;
            company.SocialLink = dto.SocialLink;

            // تحديث القيم (Values)
            if (dto.Values != null)
            {
                if (company.Values != null)
                    _context.CompanyValues.RemoveRange(company.Values);

                company.Values = dto.Values.Select(v => new CompanyValues
                {
                    NameEn = v.NameEn,
                    NameAr = v.NameAr,
                    DescriptionEn = v.DescriptionEn,
                    DescriptionAr = v.DescriptionAr
                }).ToList();
            }

            // تحديث ContactInfo
            if (dto.ContactInfo != null)
            {
                if (company.ContactInfo == null)
                    company.ContactInfo = new ContactInfo();

                company.ContactInfo.Email = dto.ContactInfo.Email;
                company.ContactInfo.Phone = dto.ContactInfo.Phone;
                company.ContactInfo.AddressEn = dto.ContactInfo.AddressEn;
                company.ContactInfo.AddressAr = dto.ContactInfo.AddressAr;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var company = await _context.CompanyInformations.FindAsync(id);
            if (company == null) return false;

            _context.CompanyInformations.Remove(company);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
