using InsuranceCertificates.Models;
using InsuranceCertificates.Validators;
using OptionsExtension = Microsoft.Extensions.Options.Options;
using NUnit.Framework;
using InsuranceCertificates.Options;
using Microsoft.Extensions.Options;
using InsuranceCertificates.Constants;

namespace InsuranceCertificates.Tests.Validators
{
    [TestFixture]
    public class NewCertificateModelValidatorTests
    {
        private static IOptions<CertificateOptions> certificateConfiguration;

        [OneTimeSetUp]
        public static void Init()
        {
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

        [TestCase(17)]
        [TestCase(0)]
        [TestCase(-18)]
        public void CustomerHasInvalidAge_ReturnsError(int age)
        {
            var validator = new NewCertificateModelValidator(certificateConfiguration);
            var certificateModel = GetNewCertificateModel();
            certificateModel.CustomerDateOfBirth = DateTime.UtcNow.AddYears(-age);
            var errorMessage = string.Format(ValidationMessages.CustomerMustHaveAllowedAge, certificateConfiguration.Value.MinAllowedCustomerAge);

            var validationResult = validator.Validate(certificateModel);

            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Errors.Any(e => string.Equals(e.ErrorMessage, errorMessage)), Is.True);
        }

        [TestCase(18)]
        [TestCase(20)]
        [TestCase(64)]
        public void CustomerHasValidAge_NoErrors(int age)
        {
            var validator = new NewCertificateModelValidator(certificateConfiguration);
            var certificateModel = GetNewCertificateModel();
            certificateModel.CustomerDateOfBirth = DateTime.UtcNow.AddYears(-age);

            var validationResult = validator.Validate(certificateModel);

            Assert.That(validationResult.IsValid, Is.True);
        }

        [Test]
        public void InsuredSumDoesNotFitAnyRange_ReturnsError()
        {
            var validator = new NewCertificateModelValidator(certificateConfiguration);
            var certificateModel = GetNewCertificateModel();
            certificateModel.InsuredSum = 111;

            var validationResult = validator.Validate(certificateModel);

            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Errors.Any(e => string.Equals(e.ErrorMessage, ValidationMessages.InsuredSumDoesNotFitAnyRange)), Is.True);
        }

        [Test]
        public void CustomerNameIsEmpty_ReturnsError()
        {
            var validator = new NewCertificateModelValidator(certificateConfiguration);
            var certificateModel = GetNewCertificateModel();
            certificateModel.CustomerName = string.Empty;

            var validationResult = validator.Validate(certificateModel);

            Assert.That(validationResult.IsValid, Is.False);
        }

        [Test]
        public void InsuredItemIsEmpty_ReturnsError()
        {
            var validator = new NewCertificateModelValidator(certificateConfiguration);
            var certificateModel = GetNewCertificateModel();
            certificateModel.InsuredItem = string.Empty;

            var validationResult = validator.Validate(certificateModel);

            Assert.That(validationResult.IsValid, Is.False);
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
