using InsuranceCertificates.Domain.Contracts;
using InsuranceCertificates.UseCases.CreateCertificate;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceCertificates.Controllers.CreateCertificate;

public class CreateCertificateController : Controller
{
    private readonly CreateCertificateUseCase _useCase;

    public CreateCertificateController(CreateCertificateUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpPost("certificates")]
    public async Task<IActionResult> Create([FromBody] CreateCertificateRequestModel requestModel)
    {
        var inputModel = new CertificateInputModel
        {
            CustomerName = requestModel.CustomerName,
            CustomerDateOfBirth = requestModel.CustomerDateOfBirth,
            InsuredItem = requestModel.InsuredItem,
            InsuredSum = requestModel.InsuredSum
        };
        var result = await _useCase.ExecuteAsync(inputModel, DateTime.Now);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }
}
