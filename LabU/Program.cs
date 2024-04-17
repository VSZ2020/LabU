using System.Security.Claims;
using LabU.Core.Identity;
using LabU.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorPages(pages =>
{
    //Настройка доступа к папкам и страницам
    pages.Conventions.AuthorizeFolder("/Student", "StudentAdminPolicy");
    pages.Conventions.AuthorizeFolder("/Teacher", "TeacherAdminPolicy");
    pages.Conventions.AuthorizeFolder("/Admin", "AdminOnlyPolicy");
});

builder.Services.AddDbContext<DataContext>(
    options => options.UseSqlite(
        builder.Configuration.GetConnectionString("DataContextConnection") ?? throw new InvalidOperationException("Connection string for 'DataContextConnection' is missing"), 
        m => m.MigrationsAssembly("LabU.Data")));
builder.Services.AddDbContext<UsersContext>(
    options => options.UseSqlite(
        builder.Configuration.GetConnectionString("DataContextConnection") ?? throw new InvalidOperationException("Connection string for 'DataContextConnection' is missing"), 
        m => m.MigrationsAssembly("LabU.Data")));
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
    });

builder.Services.AddAuthorization(options =>
{
    //Политики доступа
    options.AddPolicy("AdminOnlyPolicy", policy => policy.RequireClaim(ClaimTypes.Role,UserRoles.ADMINISTRATOR));
    options.AddPolicy("StudentAdminPolicy", policy =>
    {
        policy.RequireRole(ClaimTypes.Role, UserRoles.ADMINISTRATOR, UserRoles.STUDENT);
    });
    options.AddPolicy("TeacherAdminPolicy", policy =>
    {
        policy.RequireRole(ClaimTypes.Role, UserRoles.ADMINISTRATOR, UserRoles.TEACHER);
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Map("/Account/Logout", (HttpContext context) =>
{
    context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/Account/Login");
});
app.Run();
