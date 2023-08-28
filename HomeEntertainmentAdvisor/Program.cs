using Blazored.LocalStorage;
using HomeEntertainmentAdvisor.Areas.Identity;
using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Localization;
using HomeEntertainmentAdvisor.Middleware;
using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Services;
using HomeEntertainmentAdvisor.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.HttpsPolicy;
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
    try
    {
        builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    connectionString =Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING")?? builder.Configuration.GetConnectionString("DefaultConnection");

}
else
{
    connectionString = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
}

fbId=configuration["Authentication:Facebook:AppId"]??Environment.GetEnvironmentVariable("FB_APPID");
fbSecret=configuration["Authentication:Facebook:AppSecret"]??Environment.GetEnvironmentVariable("FB_APPSECRET");
googleId=configuration["Authentication:Google:ClientId"]??Environment.GetEnvironmentVariable("GOOGLE_CLIENTID");
googleSecret=configuration["Authentication:Google:ClientSecret"]??Environment.GetEnvironmentVariable("GOOGLE_CLIENTSECRET");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredLocalStorage(config => config.JsonSerializerOptions.WriteIndented = true);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseLazyLoadingProxies().UseSqlServer(connectionString));

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

builder.Services.AddTransient<AppExceptionHandlingMiddleware>();

builder.Services.AddTransient<ICommentsRepo, CommentsRepo>();
builder.Services.AddTransient<IMediaPiecesRepo, MediaPiecesRepo>();
builder.Services.AddTransient<IReviewImagesRepo, ReviewImagesRepo>();
builder.Services.AddTransient<IReviewLikesRepo, ReviewLikesRepo>();
builder.Services.AddTransient<IReviewsRepo, ReviewsRepo>();
builder.Services.AddTransient<IReviewTagRelationsRepo, ReviewTagRelationsRepo>();
builder.Services.AddTransient<ITagRepo, TagRepo>();
builder.Services.AddTransient<IRatingRepo, RatingRepo>();

builder.Services.AddTransient<IReviewService, ReviewService>();
builder.Services.AddTransient<IMediaService, MediaService>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddControllers();

builder.Services.AddLocalization(options => options.ResourcesPath = "Shared/Resources");

builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<User>>();
builder.Services.AddTransient<MudLocalizer, ResXMudLocalizer>();

builder.Services.AddMudServices();

var app = builder.Build();

app.UseMiddleware<AppExceptionHandlingMiddleware>();

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
