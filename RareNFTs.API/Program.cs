using Microsoft.EntityFrameworkCore;
using RareNFTs.Application.Profiles;
using RareNFTs.Application.Services.Implementations;
using RareNFTs.Application.Services.Interfaces;
using RareNFTs.Infraestructure.Data;
using RareNFTs.Infraestructure.Repository.Implementation;
using RareNFTs.Infraestructure.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

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

});

// Config Connection to SQLServer DataBase
builder.Services.AddDbContext<RareNFTsContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDataBase"));

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Configure Swagger 
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
