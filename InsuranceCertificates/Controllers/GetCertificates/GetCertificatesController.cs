using InsuranceCertificates.Data;
using InsuranceCertificates.Models;
using InsuranceCertificates.UseCases.GetCertificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsuranceCertificates.Controllers.GetCertificates;

[ApiController]
public class GetCertificatesController : ControllerBase
{
    private readonly GetCertificatesUseCase _getCertificatesUseCase;

    public GetCertificatesController(GetCertificatesUseCase getCertificatesUseCase)
    {
        _getCertificatesUseCase = getCertificatesUseCase;
    }

    [HttpGet("certificates")]
    public async Task<IEnumerable<CertificateOutputModel>> Get()
    {
        return await _getCertificatesUseCase.ExecuteAsync();
    }
}
