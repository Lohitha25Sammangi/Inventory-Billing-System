using InventoryBilling.API.DTOs;
using InventoryBilling.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JwtService _jwtService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        JwtService jwtService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
    }

    // ✅ REGISTER (Admin Only)
    [HttpPost("register")]
   
    public async Task<IActionResult> Register(RegisterDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
            return BadRequest("User already exists.");

        if (!await _roleManager.RoleExistsAsync(model.Role))
            return BadRequest("Invalid role.");

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FullName = model.FullName
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(user, model.Role);

        return Ok("User registered successfully." );
    }

    // ✅ LOGIN
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
            return Unauthorized("Invalid email or password.");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);

        if (!isPasswordValid)
            return Unauthorized("Invalid email or password.");

        var roles = await _userManager.GetRolesAsync(user);

        var token = _jwtService.GenerateToken(user, roles);

        return Ok(new
        {
            token,
            user.Email,
            user.FullName,
            roles
        });
    }
}