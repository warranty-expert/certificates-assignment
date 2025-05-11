﻿using InsuranceCertificates.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceCertificates.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Certificate> Certificates => Set<Certificate>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<PremiumLookupEntry> PremiumLookupEntries => Set<PremiumLookupEntry>();
}
