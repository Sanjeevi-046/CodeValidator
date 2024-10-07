// ***********************************************************************************************
//
//  (c) Copyright 2023, Computer Task Group, Inc. (CTG)
//
//  This software is licensed under a commercial license agreement. For the full copyright and
//  license information, please contact CTG for more information.
//
//  Description: Sample Description.
//
// ***********************************************************************************************

using CodeNest.BLL.AutoMapper;
using CodeNest.BLL.Service;
using CodeNest.DAL.Context;
using CodeNest.DAL.Repository;
using System.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRepository, CodeNest.DAL.Repository.UserRepository>();
builder.Services.AddTransient<IFormatterServices, FormatterServices>();
builder.Services.AddTransient<IWorkspaceService, WorkspaceService>();
builder.Services.AddTransient<IWorkSpaceRepository, WorkSpaceRepository>();
builder.Services.AddTransient<IJsonService, JsonService>();
builder.Services.AddTransient<IJsonRepository, JsonRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient("", _client =>
{
    _client.Timeout = TimeSpan.FromMinutes(5);
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout period
    options.Cookie.HttpOnly = true; // Cookie settings
    options.Cookie.IsEssential = true;
});
WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    _ = app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=LoginBasic}/{id?}");

app.Run();
