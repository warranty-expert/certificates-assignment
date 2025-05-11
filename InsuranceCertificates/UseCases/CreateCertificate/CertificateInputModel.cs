namespace InsuranceCertificates.UseCases.CreateCertificate;

public class CertificateInputModel
{
    public string CustomerName { get; set; }

    public DateTime CustomerDateOfBirth { get; set; }

    public string InsuredItem { get; set; }

    public decimal InsuredSum { get; set; }
}
