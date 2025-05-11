namespace InsuranceCertificates.Domain.Models;

public class PremiumLookupEntry
{
    public int Id { get; set; }

    public decimal SumFrom { get; set; }

    public decimal SumTo { get; set; }

    public decimal Premium { get; set; }
}
