using Authentication.Models;
using Microsoft.EntityFrameworkCore;
using Authentication.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvcCore().AddNewtonsoftJson();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("DBConnect");
builder.Services.AddDbContext<AuthenticationContext>(options => options.UseSqlServer(connectionString));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapHub<NoteHub>("/noteHub");

app.MapControllerRoute(
    "default",
    "{controller=Account}/{action=Index}/{id?}");

app.UseEndpoints(endpoints => {
    endpoints.MapControllers(); // enables controllers in endpoint routing
    endpoints.MapDefaultControllerRoute(); // adds the default route {controller=Home}/{action=Index}/{id?}
});

app.Run();