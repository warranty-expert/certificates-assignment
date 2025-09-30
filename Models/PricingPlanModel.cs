namespace InsuranceCertificates.Models;

public class PricingPlanModel
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public decimal? Price { get; init; }
    public bool? IsRecommended { get; init; }
    public PricingPlanModel[]? Children { get; init; }
}

public static class PricingPlanExtensions
{
    public static PricingPlanModel ToModel(this Domain.PricingPlan plan)
    {
        return new PricingPlanModel
        {
            Id = plan.Id,
            Name = plan.Name,
            Price = plan.Price,
            IsRecommended = plan.IsRecommended,
            Children = plan.Children?.Select(c => c.ToModel()).ToArray()
        };
    }
}
