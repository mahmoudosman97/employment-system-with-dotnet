using Microsoft.AspNetCore.Mvc;
using EmploymentSystem.Domain;
using EmploymentSystem.Persistence;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    // تسجيل مستخدم جديد
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok(new { message = "User registered successfully!" });
    }

    // تسجيل الدخول
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User user)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == user.Email && u.PasswordHash == user.PasswordHash);

        if (existingUser == null)
            return Unauthorized(new { message = "Invalid credentials" });

        return Ok(new { message = "Login successful!", user = existingUser });
    }
}
