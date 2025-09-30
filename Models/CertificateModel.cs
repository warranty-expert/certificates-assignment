namespace InsuranceCertificates.Models;

public class CertificateModel
{
    public required string Number { get; set; }

    public required DateTime CreationDate { get; set; }

    public required DateTime ValidFrom { get; set; }

    public required DateTime ValidTo { get; set; }

    public required string CustomerName { get; set; }

    public required DateTime CustomerDateOfBirth { get; set; }

    public required string InsuredItem { get; set; }

    public required decimal InsuredSum { get; set; }

    public required decimal CertificateSum { get; set; }
}
