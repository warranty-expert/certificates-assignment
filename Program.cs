using InsuranceCertificates.Data;
using InsuranceCertificates.Domain;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("database"));

var app = builder.Build();

FeedCertificates(app.Services);

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


void FeedCertificates(IServiceProvider provider)
{
    using var scope = provider.CreateScope();
    var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    appDbContext.Certificates.Add(new Certificate()
    {
        Number = "1",
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

    appDbContext.SaveChanges();
}