namespace InsuranceCertificates.Controllers.CreateCertificate;

public class CreateCertificateRequestModel
{
    public DateTime CustomerDateOfBirth { get; set; }
    public string CustomerName { get; set; }
    public string InsuredItem { get; set; }
    public decimal InsuredSum { get; set; }
}
