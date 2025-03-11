using Microsoft.EntityFrameworkCore;
using RareNFTs.Infraestructure.Repository.Interfaces;
using RareNFTs.Infraestructure.Repository.Implementation;
using RareNFTs.Application.Profiles;
using RareNFTs.Infraestructure.Data;
using Serilog;
using Serilog.Events;
using System.Text;
using RareNFTs.Application.Services.Implementations;
using RareNFTs.Application.Services.Interfaces;
using RareNFTs.Application.Config;
using RareNFTs.Web.Middleware;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure D.I.
builder.Services.AddTransient<IRepositoryCard, RepositoryCard>();
builder.Services.AddTransient<IRepositoryClient, RepositoryClient>();
builder.Services.AddTransient<IRepositoryCountry, RepositoryCountry>();
builder.Services.AddTransient<IRepositoryNft, RepositoryNft>();
builder.Services.AddTransient<IRepositoryInvoice, RepositoryInvoice>();
builder.Services.AddTransient<IRepositoryUser, RepositoryUser>();


//Services
builder.Services.AddTransient<IServiceCountry, ServiceCountry>();
builder.Services.AddTransient<IServiceClient, ServiceClient>();
builder.Services.AddTransient<IServiceCard, ServiceCard>();
builder.Services.AddTransient<IServiceNft, ServiceNft>();
builder.Services.AddTransient<IServiceInvoice, ServiceInvoice>();
builder.Services.AddTransient<IServiceReport, ServiceReport>();
builder.Services.AddTransient<IServiceUser, ServiceUser>();


// Security
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(
            new ResponseCacheAttribute
            {
                NoStore = true,
                Location = ResponseCacheLocation.None,
            }
        );
});
builder.Services.Configure<AppConfig>(builder.Configuration);


// config Automapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<CardProfile>();
    config.AddProfile<CountryProfile>();
    config.AddProfile<ClientProfile>();
    config.AddProfile<NftProfile>();
    config.AddProfile<InvoiceProfile>();
    config.AddProfile<ClientNftProfile>();
    config.AddProfile<UserProfile>();
    config.AddProfile<RoleProfile>();

});




// Config Connection to SQLServer DataBase
builder.Services.AddDbContext<RareNFTsContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDataBase"));

if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

// Logger
var logger = new LoggerConfiguration()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                    .Enrich.FromLogContext()
                    .WriteTo.Console(LogEventLevel.Verbose)
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File(@"Logs\Info-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.File(@"Logs\Debug-.log", shared: true, encoding: System.Text.Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(@"Logs\Warning-.log", shared: true, encoding: System.Text.Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File(@"Logs\Error-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.File(@"Logs\Fatal-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .CreateLogger();

builder.Host.UseSerilog(logger);

// Configura las culturas soportadas y la cultura predeterminada aquí
var supportedCultures = new[] { "en-US"};
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

var app = builder.Build();

// Configura el middleware para usar las opciones de localización configuradas
app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // Error control Middleware
    app.UseMiddleware<ErrorHandlingMiddleware>();
}


// Error access control 
app.UseStatusCodePagesWithReExecute("/error/{0}");

//Add support to logging request with SERILOG
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Activate Antiforgery DEBE COLOCARSE ACA!
app.UseAntiforgery();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{email?}");

app.Run();
