using CleverCode.Models;

public interface IAuthServices
{
    Task<AuthModels> GetTokenAsync(TokenRequestModel model);
    Task<AuthModels> RegisterAsync(RegisterModel model, bool isAdmin = false);
}
