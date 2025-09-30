namespace InsuranceCertificates.Domain;

public class PricingPlan
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public decimal? Price { get; init; }
    public bool? IsRecommended { get; init; }
    public PricingPlan[]? Children { get; init; }
}
