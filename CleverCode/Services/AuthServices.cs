using CleverCode.DTO;
using CleverCode.Helpers;
using CleverCode.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthServices : IAuthServices
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JWT _jwt;

    public AuthServices(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt)
    {
        _userManager = userManager;
        _jwt = jwt.Value;
    }

    public async Task<AuthModels> DeleteAdminAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if(user == null)
        {
            return new AuthModels
            {
                Message = "Admin not found"
            };
        }
        await _userManager.DeleteAsync(user);
        return new AuthModels
        {
            Message = "Admin deleted successfully"
        };
    }

    public async Task<List<AdminDto>> GetAllAdminsAsync()
    {
        var admins = await _userManager.GetUsersInRoleAsync("Admin");
        var result = new List<AdminDto>();
        foreach (var admin in admins)
        {
            result.Add(new AdminDto
            {
                Id = admin.Id,
                UserName = admin.UserName,
                Email = admin.Email,
                PhoneNumber = admin.PhoneNumber
            });
        }

        return result;
    }
    public async Task<AdminDto?> GetAdminByIdAsync(string id)
    {
        var admin = await _userManager.FindByIdAsync(id);
        if(admin == null)
        {
            return null; 
        }
        return new AdminDto()
        {
            Id = admin.Id,
            UserName = admin.UserName,
            Email = admin.Email,
            PhoneNumber = admin.PhoneNumber
        };

    }

    public async Task<AuthModels> GetTokenAsync(TokenRequestModel model)
    {
        var authModel = new AuthModels();
        var user = await _userManager.FindByNameAsync(model.Username);

        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            authModel.Message = "Username or password is incorrect!";
            return authModel;
        }

        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in userRoles)
            authClaims.Add(new Claim(ClaimTypes.Role, role));

        var key = Encoding.UTF8.GetBytes(_jwt.SecurityKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(authClaims),
            Expires = DateTime.UtcNow.AddDays(_jwt.DurationInDays),
            Issuer = _jwt.IssuerIP,
            Audience = _jwt.AudienceIP,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthModels
        {
            IsAuthenticated = true,
            Token = tokenHandler.WriteToken(token),
            Email = user.Email,
            Username = user.UserName,
            Roles = userRoles.ToList(),
            ExpiresOn = tokenDescriptor.Expires ?? DateTime.UtcNow.AddDays(_jwt.DurationInDays)
        };
    }

    public async Task<AuthModels> RegisterAsync(RegisterModel model, bool isAdmin = false)
    {
        var existingUser = await _userManager.FindByNameAsync(model.Username);
        if (existingUser != null)
        {
            return new AuthModels
            {
                Message = "Username already exists",
                IsAuthenticated = false
            };
        }

        var user = new ApplicationUser
        {
            UserName = model.Username,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthModels
            {
                Message = errors,
                IsAuthenticated = false
            };
        }

        if (isAdmin)
        {
            await _userManager.AddToRoleAsync(user, "Admin");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
            authClaims.Add(new Claim(ClaimTypes.Role, role));

        var key = Encoding.UTF8.GetBytes(_jwt.SecurityKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(authClaims),
            Expires = DateTime.UtcNow.AddDays(_jwt.DurationInDays),
            Issuer = _jwt.IssuerIP,
            Audience = _jwt.AudienceIP,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthModels
        {
            Email = user.Email,
            Username = user.UserName,
            Roles = roles.ToList(),
            IsAuthenticated = true,
            Message = "Admin Registered Successfully",
            Token = tokenHandler.WriteToken(token),
            ExpiresOn = tokenDescriptor.Expires ?? DateTime.UtcNow.AddDays(_jwt.DurationInDays)
        };
    }

    public async Task<AuthModels> UpdateAdminAsync(string id, RegisterModel model)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return new AuthModels
            {
                Message = "Admin not found",
                IsAuthenticated = false
            };
        }
        user.UserName = model.Username;
        user.Email = model.Email;
        user.PhoneNumber = model.PhoneNumber;

        var updateToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, updateToken, model.Password);

        await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthModels
            {
                Message = errors,
                IsAuthenticated = false
            };
        }
        return new AuthModels
        {
            Email = user.Email,
            Username = user.UserName,
            PhoneNumber = user.PhoneNumber,
            IsAuthenticated = true,
            Message = "Admin Updated Successfully"
        };  
    }
}
