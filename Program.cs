using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PRN222_TaskManagement.Mail;
using PRN222_TaskManagement.Models;
using PRN222_TaskManagement.Services;
using PRN222_TaskManagement.Services.Implement;
using PRN222_TaskManagement.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Prn222TaskManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/access-denied";
                    options.LogoutPath = "/logout";
                });

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));

builder.Services.AddHostedService<EventReminderService>();

builder.Services.AddTransient<MailHelper>();
builder.Services.AddHttpContextAccessor();


builder.Services.AddTransient<IUserService, UserServiceImplement>();
builder.Services.AddTransient<IEventService, EventServiceImplement>();
builder.Services.AddTransient<ICategoryService, CategoryServiceImplement>();
builder.Services.AddTransient<ITaskService, TaskServiceImplement>();
builder.Services.AddTransient<IEventShareService, EventShareServiceImplement>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
