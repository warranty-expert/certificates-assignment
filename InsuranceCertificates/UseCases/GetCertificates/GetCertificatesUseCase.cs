using InsuranceCertificates.Domain.Contracts;
using InsuranceCertificates.Models;

namespace InsuranceCertificates.UseCases.GetCertificates
{
    public class GetCertificatesUseCase
    {
        private readonly ICertificateRepository _certificateRepository;

        public GetCertificatesUseCase(ICertificateRepository certificateRepository)
        {
            _certificateRepository = certificateRepository;
        }

        public async Task<IEnumerable<CertificateOutputModel>> ExecuteAsync()
        {
            var certificates = await _certificateRepository.GetAllCertificatesAsync();
            return certificates.Select(c => new CertificateOutputModel
            {
                Number = c.Number,
                CreationDate = c.CreationDate,
                ValidFrom = c.ValidFrom,
                ValidTo = c.ValidTo,
                CustomerName = c.Customer.Name,
                CustomerDateOfBirth = c.Customer.DateOfBirth,
                InsuredItem = c.InsuredItem,
                InsuredSum = c.InsuredSum,
                CertificateSum = c.CertificateSum
            });
        }
    }
}
