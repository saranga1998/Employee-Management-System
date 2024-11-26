using EMS_Project.Data;
using EMS_Project.Repository.Employee;
using EMS_Project.Repository.Holiday;
using EMS_Project.Repository.PasswordHasherRepository;
using EMS_Project.Repository.TokenGenerator;
using EMS_Project.Repository.UserRepository;
using EMS_Project.ViewModels.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
 
// Add services to the container.
builder.Services.AddControllersWithViews();

// Bind the "Authentication" section to a strongly-typed object
var AuthenticationConfiguration = new AuthenticationConfiguration();
builder.Configuration.Bind("Authentication", AuthenticationConfiguration);
builder.Services.AddSingleton(AuthenticationConfiguration);
//------------------------------------------------------------Add Connections with Database
builder.Services.AddDbContext<AppDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));

//------------------------------------------------------------Add Services 
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IHolidayRepository,HolidayRepository>();
builder.Services.AddSingleton<IUserRepository,UserRepository>();
builder.Services.AddSingleton<IPasswordHash,PasswordHash>();
builder.Services.AddSingleton<TokenGenerator>();
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

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
