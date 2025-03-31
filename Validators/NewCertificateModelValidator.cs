using FluentValidation;
using InsuranceCertificates.Constants;
using InsuranceCertificates.Models;
using InsuranceCertificates.Options;
using Microsoft.Extensions.Options;

namespace InsuranceCertificates.Validators
{
    public class NewCertificateModelValidator : AbstractValidator<NewCertificateModel>
    {
        private readonly CertificateOptions _certificateConfiguration;
        public NewCertificateModelValidator(IOptions<CertificateOptions> certificateOptions) 
        {
            _certificateConfiguration = certificateOptions.Value;

            RuleFor(x => x.CustomerDateOfBirth)
                .NotEmpty()
                .Must(IsCustomerAgeAllowed)
                .WithMessage(string.Format(ValidationMessages.CustomerMustHaveAllowedAge, _certificateConfiguration.MinAllowedCustomerAge));

            RuleFor(x => x.InsuredSum)
                .NotEmpty()
                .Must(IsInsuredSumInValidRange)
                .WithMessage(ValidationMessages.InsuredSumDoesNotFitAnyRange);

            RuleFor(x => x.InsuredItem)
                .NotEmpty();

            RuleFor(x => x.CustomerName)
                .NotEmpty();

        }

        private bool IsCustomerAgeAllowed(DateTime dateOfBirth)
        {
            var today = DateTime.UtcNow;
            var age = today.Year - dateOfBirth.Year;

            if (dateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }

            return age >= _certificateConfiguration.MinAllowedCustomerAge;
        }

        private bool IsInsuredSumInValidRange(decimal insuredSum)
        {
            return _certificateConfiguration.InsuredSumRanges.Any(i => insuredSum >= i.MinInsuredSum && insuredSum <= i.MaxInsuredSum);
        }
    }
}
