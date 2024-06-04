using GaoQue.DataAccess;
using GaoQue.Models;
using GaoQue.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Đặt trước AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();

var app = builder.Build();

// Đặt trước UseRouting
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "trang-chu",
        pattern: "trang-chu",
        defaults: new { controller = "Home", action = "Index" });

    endpoints.MapControllerRoute(
         name: "hoan-thanh",
         pattern: "hoan-thanh",
         defaults: new { controller = "Cart", action = "Complete" });

    endpoints.MapControllerRoute(
         name: "thanh-toan",
         pattern: "thanh-toan",
         defaults: new { controller = "Cart", action = "ProcessPayment" });

    endpoints.MapControllerRoute(
         name: "gio-hang",
         pattern: "gio-hang",
         defaults: new { controller = "Cart", action = "Index" });

    endpoints.MapControllerRoute(
         name: "them-gio-hang",
         pattern: "them-gio-hang",
         defaults: new { controller = "Cart", action = "AddItem" });

    endpoints.MapControllerRoute(
        name: "tim-kiem-san-pham",
        pattern: "tim-kiem-san-pham",
        defaults: new { controller = "Product", action = "SearchProducts" });

    endpoints.MapControllerRoute(
        name: "chi-tiet-san-pham",
        pattern: "chi-tiet-san-pham",
        defaults: new { controller = "Product", action = "ProductDetail" });

    endpoints.MapControllerRoute(
        name: "lien-he",
        pattern: "lien-he",
        defaults: new { controller = "Contact", action = "Index" });

    endpoints.MapControllerRoute(
        name: "san-pham",
        pattern: "san-pham",
        defaults: new { controller = "Product", action = "Index" });

    endpoints.MapControllerRoute(
        name: "gioi-thieu",
        pattern: "gioi-thieu",
        defaults: new { controller = "Banner", action = "Index" });

    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=ProductManager}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.MapRazorPages();
app.Run();
