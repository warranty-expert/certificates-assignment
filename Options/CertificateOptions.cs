namespace InsuranceCertificates.Options
{
    public class CertificateOptions
    {
        public int MinAllowedCustomerAge { get; set; }
        public int YearsOfCertificateValidity { get; set; }
        public List<InsuredSumRange> InsuredSumRanges { get; set; } = new List<InsuredSumRange>();
    }
}
