using InsuranceCertificates.Domain.Models;

namespace InsuranceCertificates.Domain.Contracts;

public interface ICertificateRepository
{
    Task<Certificate?> GetLastCertificateAsync();

    Task InsertAsync(Certificate certificate);

    Task<IEnumerable<Certificate>> GetAllCertificatesAsync();
}
