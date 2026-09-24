using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FluentValidation;
using HireMatch.Application.Abstractions.Persistence;
using HireMatch.Application.Abstractions.Services;
using HireMatch.Application.Handlers;
using HireMatch.Application.Validators;
using HireMatch.Infrastructure.Authentication;
using HireMatch.Infrastructure.Persistence;
using HireMatch.API.Middleware;
using HireMatch.Infrastructure.Persistence.Repositories;
using HireMatch.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterCandidateValidator>();

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICandidateProfileRepository, CandidateProfileRepository>();
builder.Services.AddScoped<IResumeRepository, ResumeRepository>();

builder.Services.AddScoped<
    ICompanyMembershipRepository,
    CompanyMembershipRepository>();

builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IResumeSkillRepository, ResumeSkillRepository>();
builder.Services.AddScoped<IJobPostRepository, JobPostRepository>();
builder.Services.AddScoped<IJobSkillRepository, JobSkillRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
builder.Services.AddScoped<IResumeTextExtractor, ResumeTextExtractor>();
builder.Services.AddScoped<ISkillExtractor, SkillExtractor>();
builder.Services.AddScoped<IJobMatchingService, JobMatchingService>();

builder.Services.AddScoped<RegisterCandidateHandler>();
builder.Services.AddScoped<LoginHandler>();
builder.Services.AddScoped<GetCandidateProfileHandler>();
builder.Services.AddScoped<UpdateCandidateProfileHandler>();
builder.Services.AddScoped<CreateResumeHandler>();
builder.Services.AddScoped<ExtractResumeSkillsHandler>();
builder.Services.AddScoped<CreateJobPostHandler>();
builder.Services.AddScoped<RegisterEmployerHandler>();
builder.Services.AddScoped<CreateCompanyHandler>();
builder.Services.AddScoped<GetJobPostsHandler>();
builder.Services.AddScoped<GetJobPostByIdHandler>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secretKey = builder.Configuration["Jwt:SecretKey"]
                        ?? throw new InvalidOperationException(
                            "JWT secret key is not configured.");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)),

            ValidateIssuer = false,
            ValidateAudience = false,

            ValidateLifetime = true,

            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();