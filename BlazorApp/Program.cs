using BlazorApp.Areas.Identity;
using BlazorApp.Data;
using BlazorApp.Hubs;
using BlazorApp.Services;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddAntDesign();
builder.Services.AddSignalR().AddHubOptions<ApplicationHub>(options =>
{
    options.EnableDetailedErrors = true;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<NotifierService>();
builder.Services.AddSingleton<TrackUserLogout>();

builder.Services.AddSingleton(opt =>
    new MongoContext(builder.Configuration.GetConnectionString("MongoConnection")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".AdventureWorks.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(60);
    options.Cookie.IsEssential = true;

});

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.Zero;
});

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

var app = builder.Build();


app.UseResponseCompression();

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

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();
app.UseEndpoints(endpoints =>
{
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapHub<ApplicationHub>("/apphub");
});

app.Run();
