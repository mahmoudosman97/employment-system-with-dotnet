using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmploymentSystem.Domain;
using EmploymentSystem.Persistence;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public UserController(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    // ✅ تسجيل مستخدم جديد
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
        {
            return BadRequest(new { message = "Email already exists" });
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash); // 🔐 تشفير كلمة المرور

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "User registered successfully!" });
    }

    // ✅ تسجيل الدخول وإرجاع `JWT Token`
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

        if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.PasswordHash, existingUser.PasswordHash))
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        // ✅ إنشاء التوكن
        var token = GenerateJwtToken(existingUser);

        // ✅ طباعة القيم للتأكد من صحتها
        Console.WriteLine("Generated Token: " + token);
        Console.WriteLine("User Role: " + existingUser.Role);

        return Ok(new
        {
            message = "Login successful!",
            token = token,
            user = new
            {
                existingUser.Id,
                existingUser.FullName,
                existingUser.Email,
                existingUser.Role
            }
        });
    }


        


    // ✅ دالة لإنشاء `JWT Token`
    private string GenerateJwtToken(User user)
{
    var secretKey = _config.GetValue<string>("JwtSettings:SecretKey");
    var issuer = _config.GetValue<string>("JwtSettings:Issuer");
    var audience = _config.GetValue<string>("JwtSettings:Audience");
    var expirationInMinutes = _config.GetValue<int>("JwtSettings:ExpirationInMinutes");

    if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
    {
        throw new Exception("JWT settings are missing!");
    }

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("id", user.Id.ToString()),
        new Claim("role", user.Role)
    };

    var token = new JwtSecurityToken(
        issuer,
        audience,
        claims,
        expires: DateTime.UtcNow.AddMinutes(expirationInMinutes),
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}

}
