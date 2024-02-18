using Microsoft.EntityFrameworkCore;
using wydatki2.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

//baza danych
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection")));



var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();


