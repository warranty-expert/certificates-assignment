using InsuranceCertificates.Domain.Contracts;
using InsuranceCertificates.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceCertificates.Data.Repositories;

public class CertificateRepository : ICertificateRepository
{
    private readonly AppDbContext _context;

    public CertificateRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Certificate?> GetLastCertificateAsync()
    {
        return await _context.Certificates.OrderByDescending(c => c.Number).FirstOrDefaultAsync();
    }

    public async Task InsertAsync(Certificate certificate)
    {
        await _context.Certificates.AddAsync(certificate);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Certificate>> GetAllCertificatesAsync()
    {
        return await _context.Certificates
            .Include(c => c.Customer)
            .ToListAsync();
    }
}
