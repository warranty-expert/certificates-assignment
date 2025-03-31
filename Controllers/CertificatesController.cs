using FluentValidation;
using InsuranceCertificates.Data;
using InsuranceCertificates.Interfaces;
using InsuranceCertificates.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsuranceCertificates.Controllers;

[ApiController]
[Route("[controller]")]
public class CertificatesController : ControllerBase
{
    private readonly AppDbContext _appDbContext;
    private readonly ICertificateService _certificateService;
    private readonly IValidator<NewCertificateModel> _validator;

    public CertificatesController(AppDbContext appDbContext, 
        ICertificateService certificateService,
        IValidator<NewCertificateModel> validator)
    {
        _appDbContext = appDbContext;
        _certificateService = certificateService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IEnumerable<CertificateModel>> Get()
    {
        return await _appDbContext.Certificates.Select(c => new CertificateModel
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
        }).ToListAsync();
    }

    [HttpPost]
    public IActionResult Create([FromBody] NewCertificateModel certificate)
    {
        var validationResult = _validator.Validate(certificate);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        _certificateService.CreateCertificate(certificate);

        return Ok();
    }
}