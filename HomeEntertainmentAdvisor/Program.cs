using Blazored.LocalStorage;
using HomeEntertainmentAdvisor.Areas.Identity;
using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Hubs;
using HomeEntertainmentAdvisor.Localization;
using HomeEntertainmentAdvisor.Middleware;
using HomeEntertainmentAdvisor.MiddleWare;
using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Models.Options;
using HomeEntertainmentAdvisor.Services;
using HomeEntertainmentAdvisor.Services.Authorizarion;
using HomeEntertainmentAdvisor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MudBlazor;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

var connectionString = string.Empty;
string? fbId;
string? fbSecret;
string? googleId;
string? googleSecret;
string? cloudinaryUrl;
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

cloudinaryUrl= configuration["CloudinaryUrl"]??Environment.GetEnvironmentVariable("CLOUDINARY_URL");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredLocalStorage(config => config.JsonSerializerOptions.WriteIndented = true);

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
    .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.DetachedLazyLoadingWarning)));




builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;
}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 5;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
});


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
builder.Services.AddTransient<IMediaPieceGroupsRepo, MediaPieceGroupsRepo>();
builder.Services.AddTransient<IReviewImagesRepo, ReviewImagesRepo>();
builder.Services.AddTransient<IReviewLikesRepo, ReviewLikesRepo>();
builder.Services.AddTransient<IReviewsRepo, ReviewsRepo>();
builder.Services.AddTransient<IReviewTagRelationsRepo, ReviewTagRelationsRepo>();
builder.Services.AddTransient<ITagRepo, TagRepo>();
builder.Services.AddTransient<IRatingRepo, RatingRepo>();

builder.Services.AddTransient<IReviewService, ReviewService>();
builder.Services.AddTransient<IMediaService, MediaService>();
builder.Services.AddTransient<IRatingService, RatingService>();
builder.Services.AddTransient<ITagService, TagService>();
builder.Services.AddTransient<IReviewLikeService, ReviewLikeService>();
builder.Services.AddTransient<IReviewCommentsService, ReviewCommentsService>();
builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IImageCloud, ImageCloud>(serviceProvider => new(cloudinaryUrl));


string? smtpPassword = configuration["SMTP_Password"]??Environment.GetEnvironmentVariable("SMTP_PASSWORD");
if (smtpPassword==null) throw new Exception("SMTP password is null");
SMTPSettings smtpSettings = new("mashkovna2905@gmail.com", "HomeEntertainmentAdvisorAdmin", smtpPassword, "smtp.gmail.com", 587);
builder.Services.AddTransient<IEmailSender, EmailSender>(serviceProvider => new(smtpSettings));



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserIsAuthor", policy => policy.Requirements.Add(new UserIsAuthorRequirement()));
    options.AddPolicy("IsNotBlocked", policy => policy.Requirements.Add(new IsNotBlockedRequirement()));
});
builder.Services.AddTransient<IAuthorizationHandler, ReviewOwnerAuthorizationHandler>();
builder.Services.AddTransient<IAuthorizationHandler, IsNotBlockedAuthorizationHandler>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
          new[] { "application/octet-stream" });
});

builder.Services.AddControllers();

builder.Services.AddLocalization(options => options.ResourcesPath = "Shared/Resources");

builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<User>>();
builder.Services.AddTransient<MudLocalizer, ResXMudLocalizer>();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PreventDuplicates = false;

});
builder.Services.AddMudMarkdownServices();
var app = builder.Build();

app.UseResponseCompression();
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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await RoleInitializer.InitializeAsync(userManager, rolesManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.MapControllers();
app.MapBlazorHub();
app.MapHub<CommentHub>("/commenthub");
app.MapFallbackToPage("/_Host");

app.Run();
