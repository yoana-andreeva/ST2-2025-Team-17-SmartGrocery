using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartGrocery;
using SmartGrocery.Data; // ��� DatabaseContext � � SmartGrocery.Data
using SmartGrocery.Models;
using SmartGrocery.Repositories;
using SmartGrocery.Services;

var builder = WebApplication.CreateBuilder(args);

// --- Services ---
// DB
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity (ApplicationUser ������ �� � ��������)
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<DatabaseContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

// MVC + Razor Pages (Identity UI uses Razor Pages)
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<AiService>();
builder.Services.AddRazorPages();

// Repositories
builder.Services.AddControllersWithViews();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

var app = builder.Build();

// --- Middleware pipeline ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Important: authentication must come before authorization
app.UseAuthentication();
app.UseAuthorization();

// MVC routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map Razor Pages (Identity UI pages)
app.MapRazorPages();

// Database init & seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    db.Database.Migrate();
    DbSeeder.Seed(db);
}

app.Run();
