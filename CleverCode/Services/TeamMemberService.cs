using CleverCode.Data;
using CleverCode.DTO;
using CleverCode.Helpers;
using CleverCode.Interfaces;
using CleverCode.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Net;

namespace CleverCode.Services
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _http;

        public TeamMemberService(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor http)
        {
            _context = context;
            _mapper = mapper;
            _http = http;
        }

        private string GetLang() => _http.HttpContext?.Request?.Headers["Accept-Language"].ToString().ToLower() == "ar" ? "ar" : "en";

        private LocalizedTeamMemberDto Localize(TeamMember t)
        {
            var lang = GetLang();
            return new LocalizedTeamMemberDto
            {
                TeamMember_ID = t.TeamMember_ID,
                Name = lang == "ar" ? t.NameAr : t.Name,
                Role = lang == "ar" ? t.RoleAr : t.Role,
                Description = lang == "ar" ? t.DescriptionAr : t.Description,
                LinkedInUrl = t.LinkedInUrl,
                PhotoUrl = t.PhotoUrl,
                Bio = lang == "ar" ? t.BIOAr : t.BIO
            };
        }

        public async Task<ServiceResult> GetAllTeamMembersAsync()
        {
            var teamMembers = await _context.TeamMembers.ToListAsync();
            var localized = teamMembers.Select(t => Localize(t)).ToList();

            return new ServiceResult()
            {
                Data = localized,
                Message = "Team members retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<ServiceResult?> GetTeamMemberByIdAsync(int id)
        {
            var t = await _context.TeamMembers.FindAsync(id);
            if (t == null)
                return new ServiceResult { Message = "Team member not found", StatusCode = HttpStatusCode.NotFound };

            return new ServiceResult
            {
                Data = Localize(t),
                Message = "Team member retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<ServiceResult> CreateTeamMemberAsync(TeamMemberDto dto)
        {
            var entity = _mapper.Map<TeamMember>(dto);
            await _context.TeamMembers.AddAsync(entity);
            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Data = _mapper.Map<TeamMemberDto>(entity),
                Message = "Team member created successfully",
                StatusCode = HttpStatusCode.Created,
                Success = true
            };
        }

        public async Task<ServiceResult> UpdateTeamMemberAsync(int id, TeamMemberDto dto)
        {
            var entity = await _context.TeamMembers.FindAsync(id);
            if (entity == null)
                return new ServiceResult { Message = "Team member not found", StatusCode = HttpStatusCode.NotFound };

            _mapper.Map(dto, entity);
            _context.TeamMembers.Update(entity);
            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Data = _mapper.Map<TeamMemberDto>(entity),
                Message = "Team member updated successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<ServiceResult> DeleteTeamMemberAsync(int id)
        {
            var t = await _context.TeamMembers.FindAsync(id);
            if (t == null)
                return new ServiceResult { Message = "Team member not found", StatusCode = HttpStatusCode.NotFound };

            _context.TeamMembers.Remove(t);
            await _context.SaveChangesAsync();

            return new ServiceResult { Message = "Team member deleted successfully", StatusCode = HttpStatusCode.OK, Success = true };
        }
    }
}
