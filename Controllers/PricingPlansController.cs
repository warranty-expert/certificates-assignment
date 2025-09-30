using InsuranceCertificates.Data;
using InsuranceCertificates.Models;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceCertificates.Controllers;

[ApiController]
[Route("[controller]")]
public class PricingPlansController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public PricingPlansController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [HttpGet]
    public List<PricingPlanModel> Get()
    {
        var plans = _appDbContext.PricingPlans.FirstOrDefault();
        if (plans == null)
        {
            return [];
        }
        return [plans.ToModel()];
    }
}
