using InsuranceCertificates.Domain;
using InsuranceCertificates.Domain.Contracts;
using InsuranceCertificates.Domain.Models;
using InsuranceCertificates.Utils;

namespace InsuranceCertificates.UseCases.CreateCertificate;

public class CreateCertificateUseCase
{
    private readonly CertificateNumberManagement _numberManagement;
    ICertificateRepository _certificateRepository;
    private readonly IPremiumLookupRepository _premiumLookupRepository;

    public CreateCertificateUseCase(
        IPremiumLookupRepository premiumLookupRepository,
        ICertificateRepository certificateRepository,
        CertificateNumberManagement numberManagement)
    {
        _numberManagement = numberManagement;
        _certificateRepository = certificateRepository;
        _premiumLookupRepository = premiumLookupRepository;
    }

    public async Task<Result<Certificate, string>> ExecuteAsync(CertificateInputModel inputModel, DateTime calculationDateTime)
    {
        if (!ValidateCustomerAge(inputModel, calculationDateTime))
            return Result.Fail<Certificate, string>("Customer age is less than 18 years.");

        var certificateNumber = await _numberManagement.GenerateNextCertificateNumberAsync();

        if (certificateNumber.IsFailure)
            return Result.Fail<Certificate, string>(certificateNumber.Error);

        var premiumCalculationResult = await CalculateCertificatePremium(inputModel.InsuredSum);
        if (premiumCalculationResult.IsFailure)
            return Result.Fail<Certificate, string>(premiumCalculationResult.Error);

        var certificate = new Certificate
        {
            Number = certificateNumber.Value,
            CreationDate = DateTime.Now,
            ValidFrom = calculationDateTime,
            ValidTo = CalculateEndDate(calculationDateTime),
            Customer = new Customer
            {
                // Not enough data to assure uniqueness of customer, so each certificate has its own customer entry in DB.
                // In real life personal IDs are collected and customer entries are reused.
                Name = inputModel.CustomerName,
                DateOfBirth = inputModel.CustomerDateOfBirth
            },
            InsuredItem = inputModel.InsuredItem,
            InsuredSum = inputModel.InsuredSum,
            CertificateSum = premiumCalculationResult.Value
        };

        await _certificateRepository.InsertAsync(certificate);
        return Result.Ok<Certificate, string>(certificate);
    }

    private bool ValidateCustomerAge(CertificateInputModel inputModel, DateTime calculationDateTime)
    {
        return inputModel.CustomerDateOfBirth.AddYears(18) <= calculationDateTime;
    }

    private DateTime CalculateEndDate(DateTime calculationDateTime)
    {
        var plusOneYear = calculationDateTime.AddYears(1);
        return new DateTime(plusOneYear.Year, plusOneYear.Month, plusOneYear.Day, 0, 0, 0);
    }

    private async Task<Result<decimal, string>> CalculateCertificatePremium(decimal insuredSum)
    {
        var premiumLookupTable = await _premiumLookupRepository.GetPremiumLookupTableAsync();
        var result = premiumLookupTable
            .Where(x => x.SumFrom <= insuredSum && x.SumTo >= insuredSum)
            .FirstOrDefault();

        if (result == null)
            return Result.Fail<decimal, string>("Insurance sum is out of bounds.");

        return Result.Ok<decimal, string>(result.Premium);
    }
}
