using LibraryProject.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// --- ចំណុចបន្ថែមទី ១: បន្ថែម Session Service ---
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30); // ទុកឱ្យ Login ជាប់បាន ៣០ នាទី
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ការភ្ជាប់ Database (របស់បងមានស្រាប់)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // ប្រើ StaticFiles ធម្មតាសម្រាប់ CSS/JS

app.UseRouting();

// --- ចំណុចបន្ថែមទី ២: បើកប្រើ Session Middleware (ត្រូវដាក់នៅទីនេះ) ---
app.UseSession(); 

app.UseAuthorization();

// --- ចំណុចបន្ថែមទី ៣: ប្តូរ Route ឱ្យទៅកាន់ទំព័រ Login មុនគេ ---
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();