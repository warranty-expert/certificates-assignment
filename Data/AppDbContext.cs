using InsuranceCertificates.Domain;
using Microsoft.EntityFrameworkCore;

namespace InsuranceCertificates.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Certificate> Certificates => Set<Certificate>();
    public DbSet<Customer> Customers => Set<Customer>();
    public List<PricingPlan> PricingPlans => GetPricingPlans();

    private List<PricingPlan> GetPricingPlans()
    {
        return [
        CreatePlan(10, name: "All plans", children:
        [
            CreatePlan(20, name: "Basic", children:
            [
                CreatePlan(30, name: "Student", price: 110, isRecommended: true),
                CreatePlan(40, name: "Individual", price: 105, isRecommended: false),
                CreatePlan(50, name: "Student Plus", price: 140)
            ]),
            CreatePlan(60, name: "Standard", children:
            [
                CreatePlan(70, name: "Family", price: 180, isRecommended: false),
                CreatePlan(80, name: "Family Plus", price: 130, isRecommended: true),
                CreatePlan(90, name: "Individual", price: 120, isRecommended: true)
            ]),
            CreatePlan(100, name: "Premium", children:
            [
                CreatePlan(110, name: "Business", price: 190, isRecommended: false),
                CreatePlan(120, name: "Individual", price: 170, isRecommended: true)
            ]),
            CreatePlan(130, name: "Enterprise", children:
            [
                CreatePlan(140, name: "Corporate", price: 200, isRecommended: true),
                CreatePlan(150, name: "Custom", price: 280, isRecommended: true)
            ])
        ]) ];
    }

    private static PricingPlan CreatePlan(int id, string name, decimal? price = null, bool? isRecommended = null, PricingPlan[]? children = null)
    {
        return new PricingPlan
        {
            Id = id,
            Name = name,
            Price = price,
            IsRecommended = isRecommended,
            Children = children
        };
    }
}
