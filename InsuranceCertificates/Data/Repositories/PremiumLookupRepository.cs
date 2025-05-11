using InsuranceCertificates.Domain.Contracts;
using InsuranceCertificates.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceCertificates.Data.Repositories;

public class PremiumLookupRepository : IPremiumLookupRepository
{
    private readonly AppDbContext _context;

    public PremiumLookupRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PremiumLookupEntry>> GetPremiumLookupTableAsync()
    {
        // TODO: add caching
        return await _context.PremiumLookupEntries.ToListAsync();
    }
}
