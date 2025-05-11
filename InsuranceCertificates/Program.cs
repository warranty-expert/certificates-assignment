using InsuranceCertificates.Data;
using InsuranceCertificates.Data.Repositories;
using InsuranceCertificates.Domain.Models;
using InsuranceCertificates.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using InsuranceCertificates.UseCases.CreateCertificate;
using InsuranceCertificates.Domain;
using InsuranceCertificates.UseCases.GetCertificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("database"));

builder.Services.AddScoped<ICertificateRepository, CertificateRepository>();
builder.Services.AddScoped<IPremiumLookupRepository, PremiumLookupRepository>();
builder.Services.AddScoped<CertificateNumberManagement>();
builder.Services.AddScoped<CreateCertificateUseCase>();
builder.Services.AddScoped<GetCertificatesUseCase>();

var app = builder.Build();

SeedDb(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();


void SeedDb(IServiceProvider provider)
{
    using var scope = provider.CreateScope();
    var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    appDbContext.Certificates.Add(new Certificate()
    {
        Number = "00001",
        CreationDate = DateTime.UtcNow,
        ValidFrom = DateTime.UtcNow,
        ValidTo = DateTime.UtcNow.AddYears(1),
        CertificateSum = 200,
        InsuredItem = "Apple Iphone 14 PRO",
        InsuredSum = 999,
        Customer = new Customer()
        {
            Name = "Customer 1",
            DateOfBirth = new DateTime(2000, 1, 1)
        }
    });

    appDbContext.PremiumLookupEntries.Add(new PremiumLookupEntry()
    {
        SumFrom = 20,
        SumTo = 50,
        Premium = 8
    });

    appDbContext.PremiumLookupEntries.Add(new PremiumLookupEntry()
    {
        SumFrom = 50.01M,
        SumTo = 100,
        Premium = 15
    });

    appDbContext.PremiumLookupEntries.Add(new PremiumLookupEntry()
    {
        SumFrom = 100.01M,
        SumTo = 200,
        Premium = 25
    });

    appDbContext.SaveChanges();
}
