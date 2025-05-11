using InsuranceCertificates.Domain.Contracts;
using InsuranceCertificates.Utils;

namespace InsuranceCertificates.Domain;

public class CertificateNumberManagement
{
    private readonly ICertificateRepository _certificateRepository;

    public CertificateNumberManagement(ICertificateRepository certificateRepository)
    {
        _certificateRepository = certificateRepository;
    }
    
    public async Task<Result<string, string>> GenerateNextCertificateNumberAsync()
    {
        // This is naive implementation, prone to number duplication.
        // DB sequence should be used in real life, however InMemory database does not support sequences.

        // TODO: move hardcoded messages to resources, translate.

        var lastCertificate = await _certificateRepository.GetLastCertificateAsync();

        if (lastCertificate == null)
        {
            return Result.Ok<string, string>(FormatNumber(1));
        }

        if (!int.TryParse(lastCertificate.Number, out var lastNumber))
            return Result.Fail<string, string>("Invalid last certificate number.");

        var result = FormatNumber(lastNumber + 1);

        if (result.Length != 5)
            return Result.Fail<string, string>("We are out of certificate numbers.");

        return Result.Ok<string, string>(result);
    }

    private static string FormatNumber(int number) => number.ToString("D5");
}
