using CodeNest.BLL.AutoMapper;
using CodeNest.BLL.Service;
using CodeNest.DAL.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<MangoDbService>();
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IJsonService, JsonService>();
builder.Services.AddTransient<IWorkspaceService, WorkspaceService>();   
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient("", _client =>
{
    _client.Timeout=TimeSpan.FromMinutes(5);
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout period
    options.Cookie.HttpOnly = true; // Cookie settings
    options.Cookie.IsEssential = true;
});
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
app.UseSession();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();
