using EMS_Project.Data;
using EMS_Project.Models;
using EMS_Project.Repository.Authenticators;
using EMS_Project.Repository.Employee;
using EMS_Project.Repository.Holiday;
using EMS_Project.Repository.PasswordHasherRepository;
using EMS_Project.Repository.RefreshTokenRepository;
using EMS_Project.Repository.TokenGenerator;
using EMS_Project.Repository.TokenValidator;
using EMS_Project.Repository.UserRepository;
using EMS_Project.ViewModels.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
 
// Add services to the container.

builder.Services.AddControllersWithViews();
//builder.Services.AddIdentityCore<User>();

//------------------------------------------------------------Add Connections with Database

builder.Services.AddDbContext<AppDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));

// Bind the "Authentication" section to a strongly-typed object

var AuthenticationConfiguration = new AuthenticationConfiguration();
builder.Configuration.Bind("Authentication", AuthenticationConfiguration);
builder.Services.AddSingleton(AuthenticationConfiguration);

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = AuthenticationConfiguration.Issuer,
            ValidAudience = AuthenticationConfiguration.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthenticationConfiguration.AccessTokenSecret))
        };
        // Optional: Error handling
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                context.HandleResponse(); // Prevent default behavior
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new { message = "Token is invalid or expired." });
                return context.Response.WriteAsync(result);
            },
            OnMessageReceived = context =>
            {
                // Optionally handle tokens sent via query strings or cookies
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                // Custom actions after token validation, if needed
                return Task.CompletedTask;
            }
        };
    });

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
//});

//------------------------------------------------------------Add Services 

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IHolidayRepository,HolidayRepository>();
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IPasswordHash,PasswordHash>();
builder.Services.AddScoped<IRefreshToken, RefreshTokenRepo>();
builder.Services.AddScoped<Authenticator>();

builder.Services.AddSingleton<TokenGenerator>();
builder.Services.AddSingleton<AccessTokenGenerator>();
builder.Services.AddSingleton<RefreshTokenGenerator>();
builder.Services.AddSingleton<RefreshTokenValidator>();

//------------------------------------------------------------End Services

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
