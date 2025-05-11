using InsuranceCertificates.Domain;
using InsuranceCertificates.Domain.Contracts;
using InsuranceCertificates.Domain.Models;
using InsuranceCertificates.UseCases.CreateCertificate;
using Moq;

namespace InsuranceCertificates.Test;

public class CreateCertificateTests
{
    private readonly CreateCertificateUseCase _createCertificateUseCase;
    private readonly CertificateNumberManagement _certificateNumberManagement;
    private readonly Mock<ICertificateRepository> _certificateRepository;
    private readonly Mock<IPremiumLookupRepository> _premiumLookupRepository;

    public CreateCertificateTests()
    {
        _certificateRepository = new Mock<ICertificateRepository>();
        _premiumLookupRepository = new Mock<IPremiumLookupRepository>();
        _certificateNumberManagement = new CertificateNumberManagement(_certificateRepository.Object);
        _createCertificateUseCase = new CreateCertificateUseCase(_premiumLookupRepository.Object, _certificateRepository.Object, _certificateNumberManagement);
        SetupDefaultPremiumLookupTable();
    }

    [Fact]
    public async Task CustomerAge_SuccessIfOlderThan18yo()
    {
        // Customer age must be 18 or older

        // Given new insurance certificate request
        var inputModel = CreateDefaultInsuranceCertificateInputModel();

        // When customer age is exactly 18 years or more
        var result = await _createCertificateUseCase.ExecuteAsync(inputModel, GetDefaultCalculationDate());

        // Then certificate is created
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task CustomerAge_FailIfYoungerThan18yo()
    {
        // Customer age must be 18 or older

        // Given new insurance certificate request
        var inputModel = CreateDefaultInsuranceCertificateInputModel();

        // When customer age is less than 18 years
        var result = await _createCertificateUseCase.ExecuteAsync(inputModel, GetDefaultCalculationDate().AddDays(-1));

        // Then certificate is either created or null
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task CertificateNumber_IsFiveDigits()
    {
        // Certificate number consists of five digits

        // Given new insurance certificate request
        var inputModel = CreateDefaultInsuranceCertificateInputModel();

        // When certificate is created
        var result = await _createCertificateUseCase.ExecuteAsync(inputModel, GetDefaultCalculationDate());

        // Then insurance certificate number is five digits
        Assert.True(result.IsSuccess);
        Assert.Equal(5, result.Value?.Number.Length);
        Assert.True(int.TryParse(result.Value?.Number, out _));
    }

    [Fact]
    public async Task CertificateNumber_IsIncremental()
    {
        // Certificate number is incremental

        _certificateRepository
            .Setup(x => x.GetLastCertificateAsync())
            .Returns(Task.FromResult(new Certificate
            {
                Number = "00041"
            }));

        // Given new insurance certificate request
        var inputModel = CreateDefaultInsuranceCertificateInputModel();

        // When max existing certificate number is 41
        var result = await _createCertificateUseCase.ExecuteAsync(inputModel, GetDefaultCalculationDate());

        // Then next certificate number is 42
        Assert.True(result.IsSuccess);
        Assert.Equal("00042", result!.Value?.Number);
    }

    [Fact]
    public async Task CertificateNumber_ErrorOnOutOfNumbers()
    {
        // Certificate numbers are limited to 5 digits

        _certificateRepository
            .Setup(x => x.GetLastCertificateAsync())
            .Returns(Task.FromResult(new Certificate
            {
                Number = "99999"
            }));

        // Given new insurance certificate request
        var inputModel = CreateDefaultInsuranceCertificateInputModel();

        // When max existing certificate number is 99999
        var result = await _createCertificateUseCase.ExecuteAsync(inputModel, GetDefaultCalculationDate());

        // Then certificate cannot be created, error is returned
        Assert.True(result.IsFailure);
        Assert.True(result.Error.Length > 0);
    }

    [Fact]
    public async Task CertificateValidityDates_AreSetCorrectly()
    {
        // Certificate start and end dates are set correctly:
        // - ValidFrom is set to the date of creation
        // - ValidTo is set to one year from the date of calculation

        // Given new insurance certificate request
        var inputModel = CreateDefaultInsuranceCertificateInputModel();

        // When certificate is created
        var startDate = GetDefaultCalculationDate();
        var result = await _createCertificateUseCase.ExecuteAsync(inputModel, startDate);

        // Then certificate dates are set correctly
        var plusOneYear = startDate.AddYears(1);
        var endDate = new DateTime(plusOneYear.Year, plusOneYear.Month, plusOneYear.Day, 0, 0, 0);

        Assert.True(result.IsSuccess);
        Assert.Equal(startDate, result.Value?.ValidFrom);
        Assert.Equal(endDate, result.Value?.ValidTo);
    }

    [Fact]
    public async Task PremiumCalculation_SuccessWhenWithinBounds()
    {
        // Premium is calculated correctly

        // Given new insurance certificate request
        var inputModel = CreateDefaultInsuranceCertificateInputModel();

        // When certificate is created
        var result = await _createCertificateUseCase.ExecuteAsync(inputModel, GetDefaultCalculationDate());

        // Then premium is calculated correctly
        Assert.True(result.IsSuccess);
        Assert.Equal(25, result.Value?.CertificateSum);
    }

    [Fact]
    public async Task PremiumCalculation_FailWhenOutOfBounds()
    {
        // Premium is calculated correctly

        // Given new insurance certificate request
        var inputModel = CreateDefaultInsuranceCertificateInputModel();

        inputModel.InsuredSum = 1000; // Out of bounds

        // When certificate is created
        var result = await _createCertificateUseCase.ExecuteAsync(inputModel, GetDefaultCalculationDate());

        // Then premium is calculated correctly
        Assert.True(result.IsFailure);
        Assert.True(result.Error.Length > 0);
    }

    private CertificateInputModel CreateDefaultInsuranceCertificateInputModel()
    {
        return new CertificateInputModel
        {
            CustomerName = "Warranty Extender",
            CustomerDateOfBirth = GetDefaultCustomerBirthDate(),
            InsuredItem = "aPhone",
            InsuredSum = 158,
        };
    }

    private void SetupDefaultPremiumLookupTable()
    {
        _premiumLookupRepository.Setup(x => x.GetPremiumLookupTableAsync())
            .Returns(Task.FromResult(new List<PremiumLookupEntry>
            {
                new PremiumLookupEntry()
                    {
                        SumFrom = 20,
                        SumTo = 50,
                        Premium = 8
                    },
                new PremiumLookupEntry()
                    {
                        SumFrom = 50.01M,
                        SumTo = 100,
                        Premium = 15
                    },
                new PremiumLookupEntry()
                {
                    SumFrom = 100.01M,
                    SumTo = 200,
                    Premium = 25
                }
            }));
    }

    private DateTime GetDefaultCustomerBirthDate() => new DateTime(2004, 2, 29);

    private DateTime GetDefaultCalculationDate() => new DateTime(2022, 2, 28, 12, 28, 55); // Default is exactly 18 year old
}
