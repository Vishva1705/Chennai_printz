//using BillingMVCProject.Models;
//using BillingMVCProject.Models;
using BillingMVCProject.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("MysqlConnector");


if (string.IsNullOrEmpty(connectionString))
{
    // Handle the case where the connection string is null or empty
    throw new InvalidOperationException("The 'MysqlConnector' connection string is missing or empty.");
}

builder.Services.AddDbContext<billingContext>(options =>
options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 28))));

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
    pattern: "{controller=GSTWithout}/{action=Index}/{id?}");


    app.Run();
