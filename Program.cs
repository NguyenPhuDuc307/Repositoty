using Microsoft.EntityFrameworkCore;
using BilliardManagement.Data;
using BilliardManagement.Repositories;
using BilliardManagement.Data.Entities;
using BilliardManagement.Services;
using BilliardManagement.Utils.ConfigOptions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")
    ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.")));

builder.Services.AddTransient<IClubRepository, ClubRepository>();
builder.Services.AddTransient<IBranchRepository, BranchRepository>();
builder.Services.AddTransient<UnitOfWork>();
builder.Services.AddSingleton<IVnPayService, VnPayService>();
builder.Services.Configure<GoogleCloudStorageConfigOptions>(
    builder.Configuration.GetSection("GoogleCloudStorage"));
builder.Services.Configure<VnPayConfigOptions>(
    builder.Configuration.GetSection("VnPay"));
builder.Services.AddSingleton<ICloudStorageService, CloudStorageService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}
app.Run();
