using AzafranML_V3.Data;
using AzafranML_V3.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using System;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Use ApplicationUser instead of IdentityUser
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // This enables role management
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// Define policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OnlyMyAccount", policy => policy.RequireClaim(ClaimTypes.Email, "a2017117595@estudiantes.upsa.edu.bo"));
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin")); // New policy for Admin role
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Ensure the "Admin" role exists
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // Find your user account by email
    var userEmail = "a2017117595@estudiantes.upsa.edu.bo"; // Replace with your email 
    var user = await userManager.FindByEmailAsync(userEmail);

    if (user != null)
    {
        // Check if the user is already in the "Admin" role
        var isInAdminRole = await userManager.IsInRoleAsync(user, "Admin");

        if (!isInAdminRole)
        {
            // Assign the "Admin" role to the user
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
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

app.MapRazorPages();

app.Run();
