using HomeEntertainmentAdvisor.Areas.Identity;
using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Data.Models;
using HomeEntertainmentAdvisor.Localization;
using kedzior.io.ConnectionStringConverter;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

var connectionString = string.Empty;
string? fbId;
string? fbSecret;
string? googleId;
string? googleSecret;
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");

}
else
{
    connectionString = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
}

fbId=configuration["Authentication:Facebook:AppId"]??Environment.GetEnvironmentVariable("FB_APPID");
fbSecret=configuration["Authentication:Facebook:AppSecret"]??Environment.GetEnvironmentVariable("FB_APPSECRET");
googleId=configuration["Authentication:Google:ClientId"]??Environment.GetEnvironmentVariable("GOOGLE_CLIENTID");
googleSecret=configuration["Authentication:Google:ClientSecret"]??Environment.GetEnvironmentVariable("GOOGLE_CLIENTSECRET");


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
}).AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddAuthentication()
    .AddFacebook(facebookOptions =>
    {
        facebookOptions.AppId = fbId;
        facebookOptions.AppSecret = fbSecret;
    })
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = googleId;
        googleOptions.ClientSecret = googleSecret;
    });

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddControllers();

builder.Services.AddLocalization(options => options.ResourcesPath = "Shared/Resources");

builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<User>>();
builder.Services.AddTransient<MudLocalizer, ResXMudLocalizer>();

builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

var supportedCultures = new[] { "en-US", "ru-RU" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
