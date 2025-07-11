using CleverCode.DTO;
using CleverCode.Models;

public interface IAuthServices
{
    Task<AuthModels> GetTokenAsync(TokenRequestModel model);
    Task<AuthModels> RegisterAsync(RegisterModel model, bool isAdmin = false);
    Task<AuthModels> UpdateAdminAsync(string id, UpdateAdminDto model);
    Task<AuthModels> DeleteAdminAsync(string id);
    Task<List<AdminDto>> GetAllAdminsAsync();
    Task<AdminDto?> GetAdminByIdAsync(string id);

}
