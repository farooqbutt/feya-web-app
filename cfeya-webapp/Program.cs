using cfeya_webapp.Data;
using cfeya_webapp.Email;
using cfeya_webapp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//Adding Time Span for Confirmation Link
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(24);
});
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{ 
    options.SignIn.RequireConfirmedAccount = true; 
    options.User.RequireUniqueEmail = true;

})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//Configuring Email Functionality
builder.Services.AddTransient<IEmailSender, SendEmail>();

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseDeveloperExceptionPage();


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
