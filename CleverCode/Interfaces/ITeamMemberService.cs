using CleverCode.DTO;
using CleverCode.Helpers;

namespace CleverCode.Interfaces
{
    public interface ITeamMemberService
    {
        Task<ServiceResult> GetAllTeamMembersAsync();
        Task<ServiceResult?> GetTeamMemberByIdAsync(int id);
        Task<ServiceResult> CreateTeamMemberAsync(TeamMemberDto teamMemberDto);
        Task<ServiceResult> UpdateTeamMemberAsync(int id, TeamMemberDto teamMemberDto);
        Task<ServiceResult> DeleteTeamMemberAsync(int id);
    }
}
