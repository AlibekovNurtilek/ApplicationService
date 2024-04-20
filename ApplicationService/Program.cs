using ApplicationService.BLL.Mappers;
using ApplicationService.BLL.Models;
using ApplicationService.BLL.Services;
using ApplicationService.DAL.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultString")
        ?? throw new Exception("Connection string not found");
    options.UseSqlServer(connectionString);
});

// Add Identity
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


//Configure AccessToken Validation Parameters
var tokenValidationParameters = new TokenValidationParameters()
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ClockSkew = TimeSpan.Zero,
    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
    ValidAudience = builder.Configuration["JWT:ValidAudience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
};

// Add Authentication and JwtBearer + GoogleAuth

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = tokenValidationParameters;
    });

// �������� ��������� cookies, ���� ��� ��� �� ���� ���������
builder.Services.AddAuthentication().AddCookie();

builder.Services.Configure<AmazonSettings>(builder.Configuration.GetSection("AmazonSettings"));
builder.Services.Configure<IdentityOptions>(options =>
{
    // ��������� ���������� ��������� ������
    options.Password.RequireDigit = false; // ���������� �����
    options.Password.RequiredLength = 4; // ����������� ����� ������
    options.Password.RequireNonAlphanumeric = false; // ���������� �����������-��������� �������
    options.Password.RequireUppercase = false; // ���������� ������� � ������� ��������
    options.Password.RequireLowercase = true; // ���������� ������� � ������ ��������
    options.Password.RequiredUniqueChars = 1; // ����������� ���������� ���������� �������� � ������
});


builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddScoped<IAmazonService, AmazonService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<IGlobalExamService, GlobalExamService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAnyOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
