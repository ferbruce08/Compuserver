using CompuserverBackend.Data;
using CompuserverBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _config;

    public AuthController(UserManager<ApplicationUser> userManager,
                          SignInManager<ApplicationUser> signInManager,
                          IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.FullName };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);

        // Por defecto rol "Usuario"
        await _userManager.AddToRoleAsync(user, "Usuario");

        return Ok(new { message = "User registered" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return Unauthorized();

        var res = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!res.Succeeded) return Unauthorized();

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? user.Email ?? ""),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var jwtSec = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSec["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddHours(double.Parse(jwtSec["DurationInHours"]));

        var token = new JwtSecurityToken(
            issuer: jwtSec["Issuer"],
            audience: jwtSec["Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }

    // Solo Admin puede cambiar roles (ejemplo)
    [Authorize(Roles = "Admin")]
    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] RoleAssignModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return NotFound("User not found");

        if (!await _userManager.IsInRoleAsync(user, model.Role))
            await _userManager.AddToRoleAsync(user, model.Role);

        return Ok("Role assigned");
    }
}

public class RegisterModel
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string? FullName { get; set; }
}

public class LoginModel
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}

public class RoleAssignModel
{
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
}
