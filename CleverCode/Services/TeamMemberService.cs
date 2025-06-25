using CleverCode.Data;
using CleverCode.DTO;
using CleverCode.Helpers;
using CleverCode.Interfaces;
using CleverCode.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CleverCode.Services
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TeamMemberService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult> GetAllTeamMembersAsync()
        {
            var teamMembers = await _context.TeamMembers.ToListAsync();

            return new ServiceResult()
            {
                Data = _mapper.Map<List<TeamMemberDto>>(teamMembers),
                Message = "Team members retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<ServiceResult?> GetTeamMemberByIdAsync(int id)
        {
            var teamMember = await _context.TeamMembers.FirstOrDefaultAsync(t => t.TeamMember_ID == id);
            if (teamMember == null)
            {
                return new ServiceResult()
                {
                    Message = "Team member not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }


            return new ServiceResult()
            {
                Data = _mapper.Map<TeamMemberDto>(teamMember),
                Message = "Team member retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<ServiceResult> CreateTeamMemberAsync(TeamMemberDto teamMemberDto)
        {
            var entity = _mapper.Map<TeamMember>(teamMemberDto);

            var result = await _context.TeamMembers.AddAsync(entity);
            var saveResult = await _context.SaveChangesAsync();
            if (saveResult < 0)
            {
                return new ServiceResult()
                {
                    Message = "Couldn't create team member",
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            return new ServiceResult()
            {
                Data = _mapper.Map<TeamMemberDto>(result.Entity),
                Message = "Team member created successfully",
                StatusCode = HttpStatusCode.Created,
                Success = true
            };
        }

        public async Task<ServiceResult> UpdateTeamMemberAsync(int id, TeamMemberDto teamMemberDto)
        {
            var teamMember = await _context.TeamMembers.FirstOrDefaultAsync(tm => tm.TeamMember_ID == id);
            if (teamMember == null)
            {
                return new ServiceResult()
                {
                    Message = "Team member not found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };
            }
            _context.TeamMembers.Update(teamMember);
            var saveResult = await _context.SaveChangesAsync();
            if (saveResult < 0)
            {
                return new ServiceResult()
                {
                    Message = "Couldn't update team member",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            return new ServiceResult()
            {
                Data = _mapper.Map<TeamMemberDto>(teamMember),
                Message = "Team member updated successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<ServiceResult> DeleteTeamMemberAsync(int id)
        {
            var teamMember = await _context.TeamMembers.FindAsync(id);
            if (teamMember == null)
            {
                return new ServiceResult()
                {
                    Message = "Team member not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            _context.TeamMembers.Remove(teamMember);
            await _context.SaveChangesAsync();

            return new ServiceResult()
            {
                Message = "Team member deleted successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }
    }
}
