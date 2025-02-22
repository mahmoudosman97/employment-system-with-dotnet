using EmploymentSystem.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EmploymentSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ قراءة إعدادات JWT من `appsettings.json`
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
{
    throw new Exception("JWT settings are missing in appsettings.json!");
}

// ✅ إعداد قاعدة البيانات
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    

builder.Services.AddHostedService<ArchiveExpiredVacanciesService>();

builder.Services.AddMemoryCache();


// ✅ إضافة `JWT Authentication`
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), // ✅ التأكد من عدم وجود `null`
            ValidateIssuer = true,
            ValidIssuer = issuer, // ✅ تعيين `Issuer`
            ValidateAudience = true,
            ValidAudience = audience, // ✅ تعيين `Audience`
            ValidateLifetime = true, // ✅ التأكد من صلاحية التوكن
            ClockSkew = TimeSpan.Zero // ✅ منع أي تأخير في انتهاء صلاحية التوكن
        };
    });

// ✅ إضافة `Authorization`
builder.Services.AddAuthorization();

// ✅ إضافة `Controllers` لدعم الـ API
builder.Services.AddControllers();

// ✅ إضافة Swagger للتوثيق
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ تفعيل Swagger في وضع التطوير فقط
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ تأمين الـ API بـ `JWT Authentication`
app.UseAuthentication();
app.UseAuthorization();

// ✅ تشغيل الـ Controllers
app.MapControllers();

app.Run();
