using AutoMapper;
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
        private readonly IMapper _mapper;

        public CompanyInformationService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompanyInformationDto>> GetAllAsync()
        {
            var companies = await _context.CompanyInformations.ToListAsync();
            return _mapper.Map<IEnumerable<CompanyInformationDto>>(companies);
        }

        public async Task<CompanyInformationDto?> GetByIdAsync(int id)
        {
            var company = await _context.CompanyInformations.FindAsync(id);
            return company == null ? null : _mapper.Map<CompanyInformationDto>(company);
        }

        public async Task<CompanyInformationDto> CreateAsync(CompanyInformationDto dto)
        {
            var company = _mapper.Map<CompanyInformation>(dto);
            _context.CompanyInformations.Add(company);
            await _context.SaveChangesAsync();
            return _mapper.Map<CompanyInformationDto>(company);
        }

        public async Task<bool> UpdateAsync(int id, CompanyInformationDto dto)
        {
            var company = await _context.CompanyInformations.FindAsync(id);
            if (company == null) return false;

            _mapper.Map(dto, company);
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

