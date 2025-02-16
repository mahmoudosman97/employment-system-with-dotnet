using EmploymentSystem.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔹 إضافة قاعدة البيانات (MS SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 إضافة Controllers لدعم الـ API
builder.Services.AddControllers();

// 🔹 إضافة Swagger للتوثيق
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔹 تفعيل Swagger في وضع التطوير فقط
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔹 تشغيل HTTPS
app.UseHttpsRedirection();

// 🔹 تفعيل الـ Controllers
app.UseAuthorization();
app.MapControllers();

app.Run();
