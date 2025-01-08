using Microsoft.EntityFrameworkCore;
using PersonalBlog.Data;
using Microsoft.AspNetCore.Identity;
using PersonalBlog.Models;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MyAppContext>(options => options.UseSqlite("Data Source=PersonalBlog.db"));

builder.Services.AddDefaultIdentity<IdentityUser>(options => 
        options.SignIn.RequireConfirmedAccount = false).
    AddRoles<IdentityRole>().
    AddEntityFrameworkStores<MyAppContext>();



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

app.MapStaticAssets();


app.UseAuthorization();
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var roleManager = 
        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    
    var roles= new[] {"Admin", "User"};

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role)) 
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = 
        scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    
    var email = "admin@admin.com";
    var password = "Test1234#";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser
        {
            UserName = email,
            Email = email
        };
        await userManager.CreateAsync(user, password);
        
        await userManager.AddToRoleAsync(user, "Admin");
    }
}
app.Run();