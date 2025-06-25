using CleverCode.Data;
using CleverCode.DTO;
using CleverCode.Interfaces;
using CleverCode.Models;
using Microsoft.EntityFrameworkCore;

namespace CleverCode.Services
{
    public class CompanyInformationService : ICompanyInformationService
    {
        private readonly ApplicationDbContext _context;

        public CompanyInformationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CompanyInformationDto>> GetAllAsync()
        {
            var companies = await _context.CompanyInformations.ToListAsync();
            return companies.Select(c => new CompanyInformationDto
            {
                Company_ID = c.Company_ID,
                Name = c.Name,
                Mission = c.Mission,
                Vision = c.Vision,
                ContactInfo = c.ContactInfo,
                Description = c.Description,
                Logo = c.Logo,
                SocialLink = c.SocialLink,
                Story = c.Story,
                ResponseTime = c.ResponseTime,
                Values = c.Values
            });
        }

        public async Task<CompanyInformationDto?> GetByIdAsync(int id)
        {
            var company = await _context.CompanyInformations.FindAsync(id);
            if (company == null) return null;

            return new CompanyInformationDto
            {
                Company_ID = company.Company_ID,
                Name = company.Name,
                Mission = company.Mission,
                Vision = company.Vision,
                ContactInfo = company.ContactInfo,
                Description = company.Description,
                Logo = company.Logo,
                SocialLink = company.SocialLink,
                Story = company.Story,
                ResponseTime = company.ResponseTime,
                Values = company.Values
            };
        }

        public async Task<CompanyInformationDto> CreateAsync(CompanyInformationDto dto)
        {
            var company = new CompanyInformation
            {
                Name = dto.Name,
                Mission = dto.Mission,
                Vision = dto.Vision,
                ContactInfo = dto.ContactInfo,
                Description = dto.Description,
                Logo = dto.Logo,
                SocialLink = dto.SocialLink,
                Story = dto.Story,
                ResponseTime = dto.ResponseTime,
                Values = dto.Values
            };

            _context.CompanyInformations.Add(company);
            await _context.SaveChangesAsync();

            dto.Company_ID = company.Company_ID;
            return dto;
        }

        public async Task<bool> UpdateAsync(int id, CompanyInformationDto dto)
        {
            var company = await _context.CompanyInformations.FindAsync(id);
            if (company == null) return false;

            company.Name = dto.Name;
            company.Mission = dto.Mission;
            company.Vision = dto.Vision;
            company.ContactInfo = dto.ContactInfo;
            company.Description = dto.Description;
            company.Logo = dto.Logo;
            company.SocialLink = dto.SocialLink;
            company.Story = dto.Story;
            company.ResponseTime = dto.ResponseTime;
            company.Values = dto.Values;

            await _context.SaveChangesAsync();
            return true;
        }

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
