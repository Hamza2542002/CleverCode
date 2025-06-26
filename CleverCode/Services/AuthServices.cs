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
            Issuer = _jwt.IssureIP,
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
            Issuer = _jwt.IssureIP,
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
}
