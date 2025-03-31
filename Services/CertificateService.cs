using InsuranceCertificates.Constants;
using InsuranceCertificates.Data;
using InsuranceCertificates.Domain;
using InsuranceCertificates.Interfaces;
using InsuranceCertificates.Models;
using InsuranceCertificates.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace InsuranceCertificates.Services
{
    public class CertificateService : ICertificateService
    {
        private static readonly object _lock = new();
        private readonly AppDbContext _appDbContext;
        private readonly CertificateOptions _certificateConfiguration;

        public CertificateService(AppDbContext appDbContext, IOptions<CertificateOptions> certificateOptions)
        {
            _appDbContext = appDbContext;
            _certificateConfiguration = certificateOptions.Value;
        }

        public Certificate CreateCertificate(NewCertificateModel certificateModel)
        {
            var certificateCreationDate = DateTime.UtcNow;
            var certificateSum = GetCertificateSum(certificateModel.InsuredSum);

            var certificate = new Certificate
            {
                CertificateSum = certificateSum,
                CreationDate = certificateCreationDate,
                InsuredSum = certificateModel.InsuredSum,
                InsuredItem = certificateModel.InsuredItem,
                ValidFrom = certificateCreationDate,
                ValidTo = certificateCreationDate.AddYears(1).Date,
                Customer = new Customer
                {
                    DateOfBirth = certificateModel.CustomerDateOfBirth,
                    Name = certificateModel.CustomerName
                }
            };

            lock (_lock)
            {
                var certificateNumber = GenerateCertificateNumber();
                certificate.Number = certificateNumber;

                _appDbContext.Certificates.Add(certificate);
                _appDbContext.SaveChanges();
            }

            return certificate;
        }

        private decimal GetCertificateSum(decimal insuredSum)
        {
            var certificateSum = _certificateConfiguration.InsuredSumRanges
                .FirstOrDefault(x => insuredSum >= x.MinInsuredSum && insuredSum <= x.MaxInsuredSum)?.CertificateSum
                ?? throw new Exception(ValidationMessages.InsuredSumDoesNotFitAnyRange);

            return certificateSum;
        }

        private string GenerateCertificateNumber()
        {
            var lastNumber = _appDbContext.Certificates
                .AsNoTracking()
                .LastOrDefault()
                ?.Number;

            int nextNumber = lastNumber != null
                ? int.Parse(lastNumber) + 1
                : 1;

            return nextNumber.ToString("D5");
        }
    }
}
