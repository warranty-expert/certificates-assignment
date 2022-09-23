using InsuranceCertificates.Data;
using InsuranceCertificates.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsuranceCertificates.Controllers;

[ApiController]
[Route("[controller]")]
public class CertificatesController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public CertificatesController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
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
    public IActionResult Create()
    {
        return Ok();
    }
}