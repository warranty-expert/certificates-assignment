using InsuranceCertificates.Domain.Models;

namespace InsuranceCertificates.Domain.Contracts;

public interface IPremiumLookupRepository
{
    Task<List<PremiumLookupEntry>> GetPremiumLookupTableAsync();
}
