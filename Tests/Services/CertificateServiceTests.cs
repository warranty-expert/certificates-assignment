using InsuranceCertificates.Data;
using InsuranceCertificates.Models;
using InsuranceCertificates.Options;
using InsuranceCertificates.Services;
using OptionsExtension = Microsoft.Extensions.Options.Options;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Microsoft.Extensions.Options;
using InsuranceCertificates.Constants;

namespace InsuranceCertificates.Tests.Services
{
    [TestFixture]
    public class CertificateServiceTests
    {
        private static DbContextOptions<AppDbContext> dbOptions;
        private static IOptions<CertificateOptions> certificateConfiguration;

        [OneTimeSetUp]
        public static void Init()
        {
            dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            certificateConfiguration = OptionsExtension.Create(new CertificateOptions
            {
                MinAllowedCustomerAge = 18,
                InsuredSumRanges = new List<InsuredSumRange>
                {
                    new() {
                        MinInsuredSum = 0.00m,
                        MaxInsuredSum = 20.00m,
                        CertificateSum = 5
                    },
                    new() {
                        MinInsuredSum = 20.01m,
                        MaxInsuredSum = 40.00m,
                        CertificateSum = 5
                    }
                },
            });
        }


        [Test]
        public void CreateCertificate_InsuredSumDoesNotFitAnyRange_ThrowsError()
        {
            using var dbContext = new AppDbContext(dbOptions);
            var service = new CertificateService(dbContext, certificateConfiguration);
            var certificateModel = GetNewCertificateModel();
            certificateModel.InsuredSum = -10;

            var exception = Assert.Throws<Exception>(
                () => service.CreateCertificate(certificateModel));

            Assert.That(exception, Is.Not.Null);
            Assert.That(exception?.Message, Is.EqualTo(ValidationMessages.InsuredSumDoesNotFitAnyRange));
        }

        [Test]
        public void CreateCertificate_SeveralRequests_CertificateNumberIsUnique()
        {
            using var dbContext = new AppDbContext(dbOptions);
            var service = new CertificateService(dbContext, certificateConfiguration);
            var certificateModel = GetNewCertificateModel();
            var secondCertificateModel = GetNewCertificateModel();

            var firstCertificate = service.CreateCertificate(certificateModel);
            var secondCertificate = service.CreateCertificate(secondCertificateModel);

            Assert.That(firstCertificate, Is.Not.Null);
            Assert.That(secondCertificate, Is.Not.Null);
            Assert.That(firstCertificate.Number, Is.Not.EqualTo(secondCertificate.Number));
        }
        
        private static NewCertificateModel GetNewCertificateModel()
        {
            return new NewCertificateModel
            {
                CustomerName = "Test customer",
                InsuredItem = "Test insured item",
                InsuredSum = 20,
                CustomerDateOfBirth = DateTime.UtcNow.AddYears(-20),
            };
        }
    }
}
